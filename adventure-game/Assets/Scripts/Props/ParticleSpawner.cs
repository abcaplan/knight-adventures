using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject particles;

    private void Spawn() {
        Instantiate(particles, transform.position, Quaternion.identity);
    }
}
