using UnityEngine;

public class SpikeHead : EnemyDamage
{
    [Header ("Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;    
    [SerializeField] private LayerMask playerLayer;
    private Vector3 destination;
    private Vector3[] directions = new Vector3[4];
    private float checkTimer;
    private bool attacking;

    [Header ("Audio")]
    [SerializeField] private AudioClip impactSound; // Spike Head hits something

    private void OnEnable() {
        StopAttack();
    }

    private void Update() {
        if (attacking) {
            transform.Translate(destination * Time.deltaTime * speed);
        } else {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay) {
                CheckForPlayer();
            }
        }
    }

    private void CheckForPlayer() {
        CalculateDiretions();
        for (int i = 0; i < directions.Length; i++) {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);
        
            if(hit.collider != null && !attacking) {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDiretions() {
        directions[0] = transform.right * range; // right
        directions[1] = -transform.right * range; // left
        directions[2] = transform.up * range; // up
        directions[3] = -transform.up * range; // down
    }

    private void StopAttack() {
        destination = transform.position;
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        SoundManager.instance.PlaySound(impactSound);
        base.OnTriggerEnter2D(collision);
        StopAttack();
    }
}
