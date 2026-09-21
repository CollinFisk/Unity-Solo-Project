using UnityEngine;

public class Interactions : MonoBehaviour
{
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            player.pickupObj = other.gameObject;
        }
        if(other.gameObject.tag == "Ammo")
        {
            player.pickupObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            player.pickupObj = null;
        }
    }
}
