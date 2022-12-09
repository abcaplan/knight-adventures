using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject particles;

    public void Spawn() {
        Instantiate(particles, transform.position, Quaternion.identity);
    }
}
