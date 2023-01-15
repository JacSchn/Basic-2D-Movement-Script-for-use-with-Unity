using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //expose to inspector
    [SerializeField] private Transform groundCheck;
    [SerializeField] LayerMask playerMask;

    private bool jump;
    private float horizontalInput;
    private Rigidbody rigidBodyComponent;
    private int superJumpNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //keypress/mouseclick/etc should be here so that they aren't missed
        //Do not put physics in update function
      
        //Check if sapce was pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        //GetAxis takes controller joysticks and key presses, when key is pressed the float size will be changed in positive or negative based on the controls set in edit/project settings/input manager
        //In this case checking if A or D are pressed
        horizontalInput = Input.GetAxis("Horizontal");
    }

    //Fixed Update is called once every physics update by default 100/sec
    void FixedUpdate()
    {
        //horizontal movement
        rigidBodyComponent.velocity = new Vector3(horizontalInput, rigidBodyComponent.velocity.y, rigidBodyComponent.velocity.z);

        //prevent air jump using OverlapSphere
        if (Physics.OverlapSphere(groundCheck.position, 0.1f, playerMask).Length == 0)      //when using a decimal in C# you have to put an f at the end of it
        {
            return;
        }
        //vertical movement
        if (jump)
        {
            float jumpPower = 5f;
            if (superJumpNum > 0)
            {
                jumpPower *= 2;
                superJumpNum--;
            }
            rigidBodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            jump = false;
        }
    }

    //Collect collectable
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
            superJumpNum++;
        }
    }
}
