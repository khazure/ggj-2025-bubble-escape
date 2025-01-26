using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Cameras:
    public GameObject Cam1;
    public GameObject Cam2;

    //Fix the Player movement speed
    public float acc = 0; // acceleration

    public float jumpAmount = 0; 

    public float gravityScale = 0;

    public float floatAmount = 0;

    //2. Assign the new Rigidbody variable
    private Rigidbody rb;

    //Movement Variables
    private float movementX;
    private float movementY;

    private bool isGrounded;

    //If we are floating in bubble pad.
    private bool floating;


    // happens during physics updates
    private void FixedUpdate() 
    {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY) * acc;
        Vector3 gravity = Physics.gravity * (gravityScale - 1) * rb.mass;

        // Add force to the player
        Vector3 force = movement + gravity;
        if(floating) {
            rb.AddForce(rb.GetAccumulatedForce() * -1f, ForceMode.Impulse);
            rb.AddForce(movement + Vector3.up * floatAmount);
        } else {
            rb.AddForce(force);
        }
    }

    // happens every tick(?), for things that are not physics based
    private void Update() 
    {   
        bool grounded = (Physics.Raycast(rb.position, Vector3.down, 1f)); // raycast down to look for ground is not detecting ground? only works if allowing jump when grounded = false; // return "Ground" layer as layer
        if (Input.GetKeyDown(KeyCode.Space) && grounded) 
        {
            rb.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.C)) 
        {
            if(Cam1.activeSelf) {
                Cam1.SetActive(false);
                Cam2.SetActive(true);
            } else {
                Cam1.SetActive(true);
                Cam2.SetActive(false);
            }
        }

        // quit the game when esc is pushed. not supposed to be in game character but whatever
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cam1.SetActive(true);
        Cam2.SetActive(false);
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
        if(Cam1.activeSelf) {
            movementX = - movementVector.x;
            movementY = - movementVector.y;
        } else {
            movementX = movementVector.x;
            movementY = movementVector.y;
        }

    }

    //On coliding with a triggerable object.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BubblePad"))
        {
            rb.useGravity = false;
            rb.linearDamping = 0.5f;
            //rb.linearVelocity = Vector3.zero;
            rb.AddForce(rb.GetAccumulatedForce() * -1f, ForceMode.Impulse);
            floating = true;
            //var accumulatedForce = rb.GetAccumulatedForce();
        }

    }
    //On exiting a triggerable object.
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BubblePad"))
        {
            rb.useGravity = true;
            rb.linearDamping = 0f;
            rb.AddForce(rb.GetAccumulatedForce() * -1f, ForceMode.Impulse);
            floating = false;
        }
    }

}