using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePicka : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller;
    
    public Transform playerBody;
    
    private float speed = 12f;
    
    public float gravity = -9.81f;
    
    private Vector3 velocity;
    
    public float jumpHeight = 1f;

    public Transform groundCheck;

    public float groundDist = 0.4f;

    public LayerMask groundMask;

    private bool isGrounded;
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            
            controller.transform.position = new Vector3(playerBody.position.x,0f,playerBody.position.z);
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 moveGlobally = playerBody.right * x + playerBody.forward * z;
        
        controller.Move(moveGlobally * speed * Time.deltaTime);
        
        if(Input.GetKeyDown(KeyCode.Space) )
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime ;
        
        controller.Move(velocity * Time.deltaTime);

    }
    
}
