using UnityEngine;

public class ProjectileCollisions : MonoBehaviour
{
    public Transform Explosion;

    public float expTime = 0.3f;

    public void Start()
    {
        Explosion = transform.Find("Explosion");
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "explodingProjectile")
        {
            Destroy(other.gameObject);
            //position will be where collision happened
            GameObject p = Instantiate(Explosion, position, transform.rotation);
            Destroy(p, expTime);
            Destroy(gameObject);
        }
    }
}
