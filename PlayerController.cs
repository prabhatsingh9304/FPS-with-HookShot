using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform look;
    public float mouseSensitivity=1f;
    public float moveSpeed= 0.5f , runSpeed = 20f;
    public float jumpForce=10f;
    public Vector3 characterVelocityMomentum;
    private float activeSpeed;
    private float verticalRotationS;
    public bool invertMouse;
    public CharacterController charCtrl;
    private Vector3 movement;
    private Camera cam;
    public Transform GroundCheckPoint;
    private bool isGrounded;
    public LayerMask groundLayer;
    Hook hook;
    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        hook = GetComponent<Hook>();
    }

    // Update is called once per frame
    void Update()
    {
        playerLook();
        if(hook.gameState == Hook.GameState.Normal)
        {
            playerMove();
        }        
    }
    void LateUpdate()
    {
        cam.transform.position = look.transform.position;
        cam.transform.rotation = look.transform.rotation;

    }
    void playerLook()
    {
        Vector2 mouseInput= new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y + mouseInput.x,transform.rotation.z);
        verticalRotationS = verticalRotationS + mouseInput.y;
        verticalRotationS = Mathf.Clamp(verticalRotationS,-60,60);
        if(invertMouse)
        {
            look.rotation = Quaternion.Euler(verticalRotationS,look.rotation.eulerAngles.y,look.rotation.eulerAngles.z);
        }
        else
        {
            look.rotation = Quaternion.Euler(-verticalRotationS,look.rotation.eulerAngles.y,look.rotation.eulerAngles.z);
        }
        
    }
    void playerMove()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            activeSpeed = runSpeed;
        }
        else
        {
            activeSpeed = moveSpeed;
        }
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"),0f,Input.GetAxisRaw("Vertical"));
        float yVelociy= movement.y;
        movement=((moveInput.x * transform.right)+ (moveInput.z * transform.forward)).normalized;
        movement.y =yVelociy;
        if(charCtrl.isGrounded)
        {
            ResetGravity();
        }
        isGrounded =Physics.Raycast(GroundCheckPoint.position,Vector3.down,0.25f,groundLayer);
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            movement.y = jumpForce;
        }
        movement.y += Physics.gravity.y * Time.deltaTime;
        movement += characterVelocityMomentum;
        charCtrl.Move(movement * activeSpeed * Time.deltaTime);
        //Dampen momentum
        if(characterVelocityMomentum.magnitude>=0f)
        {
            float momentumDrag =3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if(characterVelocityMomentum.magnitude < .0f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }
    }
    public void ResetGravity()
    {
        movement.y=0f;
    }
 
}
