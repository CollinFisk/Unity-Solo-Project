using UnityEngine;

public class ProjectileCollisions : MonoBehaviour
{
    public GameObject Explosion;

    public float expTime = 0.3f;

    public void Start()
    {
        Explosion = GameObject.Find("Explosion");
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
            GameObject p = Instantiate(Explosion, other.collider.ClosestPoint(other.gameObject.transform.position), transform.rotation);
            Destroy(p, expTime);
        }
    }
}
