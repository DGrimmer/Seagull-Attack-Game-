using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float Speed{get{return speed;} set{speed = value;}}
    [SerializeField] private float speed = 300.0F;
    public float lookSensitivity = 5;
    public float rotateSpeed = 3.0F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    private float xAxisClamp = 0.0f;
    private CharacterController controller;
    [SerializeField] private Camera cam;
    // for keeping track of rotaions to make clamping easier.
    private float camRotateX = 0;
    float rotateY = 0;
    
    private void Start() 
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 horizontalMove = moveX * transform.right;
        Vector3 verticalMove = moveY * transform.forward;

        velocity = (horizontalMove + verticalMove).normalized * speed ;
        
        // player rotation
        rotateY += Input.GetAxis("Mouse X") * lookSensitivity;
        // Camera rotation
        camRotateX += Input.GetAxis("Mouse Y")* lookSensitivity;
        camRotateX = Mathf.Clamp(camRotateX, -90f, 90f);

        // Unlock cursor
        if(Input.GetKeyDown(KeyCode.Escape)){
            Cursor.lockState = CursorLockMode.None;
        }


        // handle playermovement outside fixed update and use Time.unscaledDeltaTime to better work with slowmotion
        controller.Move(velocity * Time.unscaledDeltaTime);
        transform.position = new Vector3(transform.position.x, 1.22f, transform.position.z);

    }
    private void FixedUpdate() 
    {
        cam.transform.localRotation = Quaternion.Euler(-camRotateX, 0, 0);
        transform.rotation = Quaternion.Euler(0, rotateY, 0);
    }
}
