using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove;

    [Header("Moving")]
    public float walkingSpeed;
    public float runningSpeed;

    [Header("Jump")]
    public float jumpForce;
    public float gravity;

    [Header("Rotation")]
    public Camera cameraMain;
    public float lookSpeed;
    public float lookXlimit;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX;


    private void Start()
    {
        characterController=GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedZ = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;

        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedZ) + (right * curSpeedX);

        if(Input.GetKey(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            Debug.Log("Space");
            moveDirection.y = jumpForce;  
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if(!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if(canMove)
        {
            rotationX+=Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXlimit, lookXlimit);
            cameraMain.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed,0);
        }
    }


}
