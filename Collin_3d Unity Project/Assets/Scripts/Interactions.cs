using System.Collections;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public PlayerController player;

    public float interactCooldown = 2;
    public bool canInteract = true;

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
            if (canInteract)
            {
                player.pickupObj = other.gameObject;
                canInteract = false;
                StartCoroutine("intCooldown");
            }
            else if (!canInteract)
            {
                player.pickupObj = null;
            }

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

    IEnumerator intCooldown()
    {
        yield return new WaitForSeconds(interactCooldown);

        canInteract = true;
    }
}
