using UnityEngine;


public class KillCollision : MonoBehaviour
{
    PlayerController player;
    Enemy enemy;
    RangedEnemies rangedEnemy;
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.health = 0;
        }
        if (other.gameObject.tag == "Enemy")
        {
            enemy.enemyHealth = 0;
        }
        if (other.gameObject.tag == "RangedEnemy")
        {
            rangedEnemy.enemyHealth = 0;
        }
        

        else
            Destroy(other.gameObject);
    }
}
