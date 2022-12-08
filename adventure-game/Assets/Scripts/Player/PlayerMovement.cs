using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Player Stats")]
    [SerializeField] public float baseSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallGravity;
    private float horizontalInput;
    public float currentSpeed;

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
    [SerializeField] private int extraJumps;
    private int jumpCounter;
    
    [Header ("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask propsLayer;

    [Header ("Crouching")]
    [SerializeField] private float crouchingSpeed;
    public bool isCrouching { get; private set; }

    [Header ("Dashing")]
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    private bool canDash = true;
    private bool isDashing = false;

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
        // Stop any player movement while dashing to avoid bugs
        if (isDashing) {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        // Adjust player character when moving left-right
        if (horizontalInput > 0.01f) {
            CreateDust();
            transform.localScale = Vector3.one;
        } else if (horizontalInput < -0.01f) {
            CreateDust();
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Crouch Activation
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && canAttack()) {
            spriteRenderer.sprite = crouch;
            boxCollider.size = crouchedSize;
            isCrouching = true;
            currentSpeed = crouchingSpeed;
            anim.SetBool("crouch", isCrouching);
        }

        // Crouch Deactivation
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)){
            spriteRenderer.sprite = normal;
            boxCollider.size = normalSize;
            isCrouching = false;
            currentSpeed = baseSpeed;
            anim.SetBool("crouch", isCrouching);
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
                body.gravityScale = 7;
                body.velocity = new Vector2(horizontalInput * currentSpeed, body.velocity.y);
            }

            if (isGrounded()) {
                coyoteCounter = coyoteTime;
                jumpCounter = extraJumps;
            } else {
                coyoteCounter -= Time.deltaTime;
            }
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

    private void WallJump() {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
    }

    private bool isGrounded() {
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer); // under the player is ground layer
        RaycastHit2D raycastHitProps = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, propsLayer); // under the player is any prop layer
        return (raycastHitGround.collider != null) || (raycastHitProps.collider != null);
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

    // For testing
    /*
    void OnGUI() {
        if (true) {
            GUI.Label(new Rect(0, 0, 256, 32), "Dashing: " + isDashing.ToString());
            GUI.Label(new Rect(0, 16, 256, 32), "Gravity: " + body.gravityScale.ToString());
        }
    }
    */
    
}
