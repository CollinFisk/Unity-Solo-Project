using UnityEngine;

public class Aiming : MonoBehaviour
{
    private Vector3 mouse;
    public float rotationSpeed = 3f;
    private Quaternion lookRotation;
    private Vector3 directionTartget;
    public Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        aiming();
    }

    public void aiming()
    {
        Ray ray = mainCamera.ScreenPointToRay(mouse);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            mouse = hit.point;
        }

        directionTartget = mouse - transform.position.normalized;

        lookRotation = Quaternion.LookRotation(directionTartget);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
