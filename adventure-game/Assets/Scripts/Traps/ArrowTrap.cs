using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header ("Attributes")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;

    [Header ("Audio")]
    [SerializeField] private AudioClip arrowTrapSound;


    private float cooldownTimer;

    private void Update() {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown) {
            Attack();
        }
    }

    private void Attack() {
        cooldownTimer = 0;

        SoundManager.instance.PlaySound(arrowTrapSound);

        arrows[FindArrow()].transform.position = firePoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindArrow() {
        for (int i = 0; i < arrows.Length; i++) {
            if (!arrows[i].activeInHierarchy) {
                return i;
            }
        }
        return 0;
    }
}
