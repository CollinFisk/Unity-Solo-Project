using Unity.Cinemachine;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject shield;
    public GameObject protectedCreature;

    public int shieldHealth;

    public float shieldXdistance;
    public float shieldYdistance;
    public float shieldZdistance;

    void Update()
    {
        Vector3 shieldPos = shield.transform.position;
        shieldPos.x = protectedCreature.transform.position.x + shieldXdistance;
        shieldPos.y = protectedCreature.transform.position.y + shieldYdistance;
        shieldPos.z = protectedCreature.transform.position.z + shieldZdistance;
        shield.transform.position = shieldPos;

        if (shieldHealth == 0)
        {
            shield = transform.gameObject;
            Destroy(shield);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            shieldHealth--;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "eprojectile")
        {
            shieldHealth--;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Explosion")
        {
            shieldHealth = 0;
            Destroy(collision.gameObject);
        }


    }
}
