using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Fix the Player movement speed
    public float acc = 0; // acceleration

    public float jumpAmount = 0; 

    public float gravityScale = 0;

    //2. Assign the new Rigidbody variable
    private Rigidbody rb;

    //Movement Variables
    private float movementX;
    private float movementY;

    private bool isGrounded;


    private void FixedUpdate() 
    {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY) * acc;
        Vector3 gravity = Physics.gravity * (gravityScale - 1) * rb.mass;

        // Add force to the player
        Vector3 force = movement + gravity;
        rb.AddForce(force);
    }

    private void Update() 
    {   
        bool grounded = (Physics.Raycast(rb.position, Vector3.down, 1f)); // raycast down to look for ground is not detecting ground? only works if allowing jump when grounded = false; // return "Ground" layer as layer
        if (Input.GetKeyDown(KeyCode.Space) && grounded) 
        {
            rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
        }
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
        // negative due to camera position
        movementX = - movementVector.x;
        movementY = - movementVector.y;

    }

    //3. Add a FixedUpdate function

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    // void OnTriggerStay(Collider other)
    // {
    //     if (other.transform.tag == "ground")
    //     {
    //         isGrounded = true;
    //         Debug.Log("Grounded");
    //     }
    //     else
    //     {
    //         isGrounded = false;
    //         Debug.Log("Not Grounded!");
    //     }
    // }

}