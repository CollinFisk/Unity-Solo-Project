using UnityEngine;

public class KillCollision : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
    }
}
