using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Fix the Player movement speed
    public float acc = 0; // acceleration

    public float friction = 0;

    //2. Assign the new Rigidbody variable
    private Rigidbody rb;

    //Movement Variables
    private float movementX;
    private float movementY;


    private void FixedUpdate() 
    {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

        // Add force to the player
        rb.AddForce(movement * acc);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent <Rigidbody>();
    }

    //Function plays on movement?
    void OnMove (InputValue movementValue) 
    {
        //Applying input data to the Player
        //1. Set the movement vector
        Vector2 movementVector = movementValue.Get<Vector2>();

        //Assign the movement variables
        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    //3. Add a FixedUpdate function

    // Update is called once per frame
    // void Update()
    // {
        
    // }


}