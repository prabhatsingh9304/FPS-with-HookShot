using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerShootingPoint;
    public GameObject testHookPoint;
    public Transform hookShotTrasform;
    
    public LayerMask hookLayer;
    public float maxHookDistance;
    private Vector3 hookShotPostion;
    private float hookShotThrowSpeed;
    private float hookShotSize;
    Vector3 hookShotDir;
    float hookShotDistance;
    float hookShotSpeed;
    private PlayerController charcterCtrl;
    public enum GameState{
        Normal,
        HookFlyingPlayer,
        HookShotTrown,
        HookSwing
    }
    public GameState gameState;
    void Awake()
    {
        hookShotTrasform.gameObject.SetActive(false);
        charcterCtrl = GetComponent<PlayerController>();
        gameState = GameState.Normal;
                
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState)
        {
            case GameState.Normal:
                HookShotStart();
                break;
            case GameState.HookShotTrown:
                HookShotThrownFun();
                break;
            case GameState.HookFlyingPlayer:
                HookMovement();
                break;
            case GameState.HookSwing:
                HookSwingFun();
                break;
            default:
                return;
        }
        
    }
    void HookShotStart()
    {
        if(HookShotInput())
        {
            ShootHookShot();
        }
    }
    void HookShotThrownFun()
    {
        hookShotTrasform.LookAt(hookShotPostion);
        hookShotThrowSpeed = 50f; 
        hookShotSize += hookShotThrowSpeed * Time.deltaTime;
        hookShotTrasform.localScale = new Vector3(0.05f,0.05f,hookShotSize);
        if(hookShotSize >= Vector3.Distance(transform.position,hookShotPostion))
        {
            gameState = GameState.HookFlyingPlayer;
            //cameraFov.SetCameraFov(HOOKSHOT_FOV);
            
        }
    }
    void HookMovement()
    {
        hookShotTrasform.LookAt(hookShotPostion);
        hookShotDir = (hookShotPostion - transform.position).normalized;
        hookShotDistance = Vector3.Distance(playerShootingPoint.transform.position,hookShotPostion);
        hookShotSpeed = Mathf.Clamp(hookShotDistance,10f,40f);
        charcterCtrl.charCtrl.Move(hookShotDir * hookShotSpeed * Time.deltaTime);
        if(hookShotDistance <=2f)
        {
            //gameState = GameState.Normal;
            gameState = GameState.HookSwing;
        }
        if(HookShotInput())
        {
            gameState = GameState.Normal;
        }
        if(JumpInputFun())
        {
            HookJumpCancel();
        }

    }
    void HookSwingFun()
    {
        charcterCtrl.ResetGravity();
        hookShotTrasform.gameObject.SetActive(false);
        if(HookShotInput())
        {
            ShootHookShot();
        }
        else if(JumpInputFun())
        {
            HookJumpCancel();
        }     
    }
    bool HookShotInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    bool JumpInputFun()
    {
        return Input.GetButtonDown("Jump");
    }
    void ShootHookShot()
    {
        if(Physics.Raycast(playerShootingPoint.transform.position,playerShootingPoint.transform.forward,out RaycastHit hit))
        {
            hookShotPostion= hit.point;
            testHookPoint.transform.position = hookShotPostion;
            hookShotSize= 0f;
            hookShotTrasform.gameObject.SetActive(true);
            hookShotTrasform.localScale = Vector3.zero;
            gameState = GameState.HookShotTrown;
        }
    }
    void HookJumpCancel()
    {
        //Cancel Hookshot with a jump
            float momentumExtraSpeed = 0.5f;
            charcterCtrl.characterVelocityMomentum = hookShotDir * momentumExtraSpeed;
            float jumpSpeed=0.1f;
            charcterCtrl.characterVelocityMomentum += Vector3.up * jumpSpeed;
            hookShotTrasform.gameObject.SetActive(false);
            gameState = GameState.Normal;
            charcterCtrl.ResetGravity();
    }
}
