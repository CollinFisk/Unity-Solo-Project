using UnityEngine;

public class ProjectileCollisions : MonoBehaviour
{
    public Transform Explosion;

    public void Start()
    {
        Explosion = transform.Find("Explosion");
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Destroy(other.transform);
        }

        if (other.gameObject.tag == "explodingProjectile")
        {
            Destroy(other.transform);

            if (Explosion != null && other.contactCount > 0)
            {
                ContactPoint contact = other.GetContact(0);

                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                Instantiate(Explosion, pos, rot);
            }
            Destroy(gameObject);
        }
    }
}
