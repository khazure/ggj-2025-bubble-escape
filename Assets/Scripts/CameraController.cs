using UnityEngine;

public class CameraController : MonoBehaviour
{
    //This is gameobject is assigned to the player.
    public GameObject player;
    private Vector3 offset;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Define the offset variable
        offset = transform.position - player.transform.position; 

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Set the camera position in LateUpdate
        transform.position = player.transform.position + offset; 
    }
}