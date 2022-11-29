using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Player Stats")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private float jumpPower;
    private float horizontalInput;
    private float currentSpeed;

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
    private bool isCrouching;
    
    private Rigidbody2D body;
    private Animator anim;

    [Header ("Audio")]
    [SerializeField] private AudioClip jumpSound;

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
        horizontalInput = Input.GetAxis("Horizontal");

        // Adjust player character when moving left-right
        if (horizontalInput > 0.01f) {
            transform.localScale = Vector3.one;
        } else if (horizontalInput < -0.01f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Crouch
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
            && canAttack()) {

            spriteRenderer.sprite = crouch;
            boxCollider.size = crouchedSize;
            isCrouching = true;
            currentSpeed = crouchingSpeed;
            anim.SetBool("crouch", isCrouching);
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)){
            spriteRenderer.sprite = normal;
            boxCollider.size = normalSize;
            isCrouching = false;
            currentSpeed = baseSpeed;
            anim.SetBool("crouch", isCrouching);
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
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        } else {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * currentSpeed, body.velocity.y);

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
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
