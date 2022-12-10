using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Player Stats")]
    [SerializeField] public float baseSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallGravity;
    [SerializeField] private float normalGravity;    
    private int score = 0;
    public float currentSpeed;
    private float horizontalInput;

    [Header ("Text")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text powerUpText;

    [Header ("Collider & Size")]
    private BoxCollider2D boxCollider;
    [SerializeField] private Vector2 normalSize;
    [SerializeField] private Vector2 crouchedSize;
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite crouch;
    private SpriteRenderer spriteRenderer;

    [Header ("Coyote Jump")]
    [SerializeField] private float coyoteTime; // How much time the player can stay in the air and allow to jump
    private float coyoteCounter; // How much time passes after player leaves ground/platform

    [Header ("Wall Jump")]
    [SerializeField] private float wallJumpX; // Horizontal Wall Jump force
    [SerializeField] private float wallJumpY; // Vertical Wall Jump force

    [Header ("Multiple Jumps")]
    [SerializeField] private float extraJumpsDuration;
    public int extraJumps;
    private int jumpCounter;
    private bool isExtraJumps = false;
    private float extraJumpsTime = 0;
    
    [Header ("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask propsLayer;
    [SerializeField] private LayerMask iceLayer;
    [SerializeField] private LayerMask mudLayer;
    [SerializeField] private LayerMask ceilingLayer;

    [Header ("Crouching")]
    [SerializeField] private float crouchingSpeed;
    public bool isCrouching { get; private set; }

    [Header ("Dashing")]
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    private bool canDash = true;
    private bool isDashing = false;

    [Header ("Terrain Stats Changes")]
    [SerializeField] private float iceSpeed;
    [SerializeField] private float mudSpeed;
    [SerializeField] private float iceGravity;

    [Header ("Audio")]
    [SerializeField] private AudioClip jumpSound;

    [Header ("Particle System")]
    [SerializeField] private ParticleSystem dust;
    
    private Rigidbody2D body;
    private Animator anim;

    private void Awake() {
        currentSpeed = baseSpeed;
        // References
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = normal;
        normalSize = boxCollider.size;
    }

    private void Update() {
        // Update score
        scoreText.text = "Score: " + score;

        // Make Power Up Text Appear
        extraJumpsTime = Mathf.Max(0.0f, extraJumpsTime - Time.deltaTime);
        if (isExtraJumps) {
            powerUpText.gameObject.SetActive(true);
            powerUpText.text = "Extra Jump: " + Mathf.Round(extraJumpsTime).ToString() + " seconds";
        } else {
            powerUpText.gameObject.SetActive(false);
        }

        // Stop any player movement while dashing to avoid bugs
        if (isDashing) {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        // Adjust player character when moving left-right
        if (horizontalInput > 0.01f) {
            CreateDust();
            transform.localScale = new Vector2(1, 1);
        } else if (horizontalInput < -0.01f) {
            CreateDust();
            transform.localScale = new Vector3(-1, 1);
        }
        
        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Crouch Activation
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && canAttack()) {
            spriteRenderer.sprite = crouch;
            boxCollider.size = crouchedSize;
            isCrouching = true;
            anim.SetBool("crouch", isCrouching);
        }

        // Crouch Deactivation
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)){
            if (!hasCollisionAbovePlayer()) {
                spriteRenderer.sprite = normal;
                boxCollider.size = normalSize;
                isCrouching = false;
                anim.SetBool("crouch", isCrouching);
            }
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.N) && canDash) {
            anim.SetBool("isDashing", true);
            StartCoroutine(Dash());
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        // Adjustable Jump
        if(Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0) {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }

        if (onWall()) {
            body.gravityScale = wallGravity;
            body.velocity = Vector2.zero;
        } else {
            // Prevent velocity and gravity change during dashing
            if (!isDashing){
                body.gravityScale = normalGravity;
                body.velocity = new Vector2(horizontalInput * currentSpeed, body.velocity.y);
            }

            if (isGrounded()) {
                coyoteCounter = coyoteTime;
                jumpCounter = extraJumps;
            } else {
                coyoteCounter -= Time.deltaTime;
            }
        }

        // Adjust speed when on ice/mud
        if (isOnIce() && !isCrouching){
            currentSpeed = iceSpeed;
            //body.gravityScale = iceGravity;
        } else if (isOnMud() && !isCrouching){
            currentSpeed = mudSpeed;
            //body.gravityScale = normalGravity;
        } else if (isCrouching){
            currentSpeed = crouchingSpeed;
            //body.gravityScale = normalGravity;
        } else {
            currentSpeed = baseSpeed;
            //body.gravityScale = normalGravity;
        }

    }

    private void Jump() {
        if (coyoteCounter < 0 && !onWall() && jumpCounter <= 0) return;

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall()){
            WallJump();
        } else {
            if (isGrounded()) {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            } else {
                if (coyoteCounter > 0) {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                } else {
                    if (jumpCounter > 0) {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            // Avoid double jump -> Reset counter
            coyoteCounter = 0;
        }
    }

    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        body.gravityScale = originalGravity;
        isDashing = false;
        anim.SetBool("isDashing", false);
        
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private IEnumerator ExtraJumpsTime() {
        extraJumpsTime = extraJumpsDuration;
        extraJumps = 1;
        isExtraJumps = true;
        yield return new WaitForSeconds(extraJumpsDuration);
        isExtraJumps = false;
        extraJumps = 0;
    }

    private void WallJump() {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
    }

    private bool isGrounded() {
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer); // under the player is ground layer
        RaycastHit2D raycastHitProps = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, propsLayer); // under the player is any prop layer
        RaycastHit2D raycastHitIce = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, iceLayer); // under the player is ice
        RaycastHit2D raycastHitMud = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, mudLayer); // under the player is mud
        return (raycastHitGround.collider != null) || (raycastHitProps.collider != null) || (raycastHitIce.collider != null) || (raycastHitMud.collider != null);
    }

    private bool onWall() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack() {
        return horizontalInput < 0.75f && horizontalInput > -0.75f && isGrounded() && !onWall();
    }

    private void CreateDust(){
        dust.Play();
    }

    private bool isOnIce() {
        RaycastHit2D raycastHitIce = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, iceLayer); // under the player is ice
        return raycastHitIce.collider != null;
    }

    private bool isOnMud() {
        RaycastHit2D raycastHitMud = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, mudLayer); // under the player is mud
        return raycastHitMud.collider != null;
    }

    private bool hasCollisionAbovePlayer() {
        RaycastHit2D raycastHitWall = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, wallLayer);
        RaycastHit2D raycastHitCeiling = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, ceilingLayer);
        return (raycastHitWall.collider != null) || (raycastHitCeiling.collider != null);
    }

    public void AddScore(int _score) {
        score += _score;
    }

    public void AddMelonPowerUp() {
        StartCoroutine(ExtraJumpsTime());
    }

    // For testing
    
    void OnGUI() {
        if (true) {
            GUI.Label(new Rect(0, 0, 256, 32), "jumps: " + extraJumps.ToString());
            GUI.Label(new Rect(0, 16, 256, 32), "jumps time: " + extraJumpsDuration.ToString());
        }
    }
    
}
