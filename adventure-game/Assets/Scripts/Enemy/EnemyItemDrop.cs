using UnityEngine;

public class EnemyItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject collectable;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float dropChance;

    private void DropItem(){
        if (Random.value < dropChance) {
            Instantiate(collectable, enemy.transform.position, Quaternion.identity);
        }
    }
}
