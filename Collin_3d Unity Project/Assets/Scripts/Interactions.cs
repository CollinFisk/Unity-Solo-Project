using System.Collections;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public PlayerController player;
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
        if (other.gameObject.tag == "Weapon")
        {
            canInteract = true;

            if (other.gameObject.transform.IsChildOf(player.transform))
                player.pickupObj = null;
            else
                player.pickupObj = other.gameObject;
        }

        if (other.gameObject.tag == "Ammo")
        {
            canInteract = true;
            player.pickupObj = other.gameObject;
        }

        if (other.gameObject.tag == "collectionReq")
        {
            canInteract = true;
            player.pickupObj = other.gameObject;
        }
        if (other.gameObject.tag == "Door")
        {
            player.pickupObj = other.gameObject;
            if (other.gameObject.GetComponent<Door>().noReq == true)
            {
                canInteract = true;
            }
        }
        if (other.gameObject.tag == "Button")
        {
            player.pickupObj = other.gameObject;
            if (other.gameObject.GetComponent<Door>().buttonReq == true)
            {
                canInteract = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Weapon" || other.gameObject.tag == "Ammo" || other.gameObject.tag == "collectionReq" || other.gameObject.tag == "Door" || other.gameObject.tag == "Button")
        {
            canInteract = false;
            player.pickupObj = null;
        }
    }
}
