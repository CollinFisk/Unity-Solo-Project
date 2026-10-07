using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{    
    public GameObject Explosion;

    public float expTime = 0.3f;
    public int ObjectHealth;

    private void Update()
    {
        if (ObjectHealth <= 0)
        {
            GameObject p = Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(p, expTime);

        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile" || other.gameObject.tag == "enemyProjectile")
        {
            Destroy(other.gameObject);
            ObjectHealth--;
        }

        if (other.gameObject.tag == "explodingProjectile")
        {
            Destroy(other.gameObject);
            ObjectHealth = 0;
        }
        if (other.gameObject.tag == "Explosion")
        {
            ObjectHealth = 0;
        }
    }
}
