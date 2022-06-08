using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Params
    public float movmentSpeed = 4000f;
    [Range(0, 0.5f)][SerializeField] public float movementSmoothing = 0.01f;
    [SerializeField] GameObject otherPlayer;
    [SerializeField] CameraFollowingHandler cameraHandler;

    [SerializeField] InputActionMap actionMap;
    [SerializeField] Rigidbody2D rigidBody;

    private Vector3 velocity = Vector3.zero;
    private float verticalMovment = 0f;
    private float horizontalMovement = 0f;

    private float moveLeft = 0f;
    private float moveRight = 0f;
    private float moveDown = 0f;
    private float moveUp = 0f;

    private bool isOtherPlayerInGame = false;

    [SerializeField]
    private PlayerPickUpItem playerPickUpItem;

    [SerializeField]
    private Button mobileUp, mobileDown, mobileLeft, mobileRight;

    
    void Start()
    {
        SetupMovementKeys();
    }

    
    void Update()
    {
        
    }

    void FixedUpdate() {
        moveUp = movmentSpeed * actionMap["Up"].ReadValue<float>();
        moveDown = movmentSpeed * actionMap["Down"].ReadValue<float>();
        moveLeft = movmentSpeed * actionMap["Left"].ReadValue<float>();
        moveRight = movmentSpeed * actionMap["Right"].ReadValue<float>();
        Vector2 targetVelocity = new Vector2(moveRight - moveLeft, moveUp - moveDown);
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, movementSmoothing);
        // if (Input.touchCount > 0) {
        //     Vector2 touch = Input.GetTouch(0).position;
        //     touch.x -= Screen.width/2;
        //     touch.y -= Screen.height/2;
        //     touch = touch/20;
        //     touch.y = touch.y * 4;
        //     // Debug.Log($"{touch}");
        //     rigidBody.AddForce(touch);
        // }
    }

    public void addForceRight() {
        rigidBody.AddForce(new Vector2(1, 0) * 300);
    }

    public void addForceLeft() {
        rigidBody.AddForce(new Vector2(-1, 0) * 300);
    }

    public void addForceUp() {
        rigidBody.AddForce(new Vector2(0, 1) * 300);
    }

    public void addForceDown() {
        rigidBody.AddForce(new Vector2(0, -1) * 300);
    }

    private void OnEnable() {
        actionMap.Enable();
    }

    private void OnDisable() {
        actionMap.Disable();
    }

    private void SetupMovementKeys() {
        if (otherPlayer != null)
        {
            actionMap["OtherPlayer"].performed += ctx => ToggleOtherPlayer();
        }
        
        // actionMap["Up"].started += cts => MovePlayer(0, 1);
        // actionMap["Up"].canceled += cts => StopPlayer();
        // actionMap["Down"].started += cts => MovePlayer(0, -1);
        // actionMap["Down"].canceled += cts => StopPlayer();
        // actionMap["Left"].started += cts => MovePlayer(-1, 0);
        // actionMap["Left"].canceled += cts => StopPlayer();
        // actionMap["Right"].started += cts => MovePlayer(1, 0);
        // actionMap["Right"].canceled += cts => StopPlayer();
    }

    private void ToggleOtherPlayer()
    {
        isOtherPlayerInGame = !isOtherPlayerInGame;
        if(isOtherPlayerInGame)
        {
            otherPlayer.SetActive(true);
            cameraHandler.SetupOtherTarget(otherPlayer.transform);
            otherPlayer.transform.position = transform.position;
        }
        else
        {
            otherPlayer.SetActive(false);
            cameraHandler.SetupOtherTarget(null);
        }
    }

    

    void MovePlayer(int xMultiplier, int yMultiplier) {
        // Debug.Log(movmentSpeed * Time.deltaTime * xMultiplier);
        // Debug.Log(movmentSpeed * Time.deltaTime * yMultiplier);
        verticalMovment = verticalMovment + movmentSpeed * yMultiplier;
        horizontalMovement = horizontalMovement + movmentSpeed * xMultiplier;
        //var  = new targetVelocityVector2(movmentSpeed * Time.deltaTime * xMultiplier, movmentSpeed * Time.deltaTime * yMultiplier);
        //rigidBody.velocity = rigidBody.velocity + movmentSpeed * Time.deltaTime * new Vector2(xMultiplier, yMultiplier);
        //rigidBody.AddForce(new Vector2(movmentSpeed * Time.deltaTime * xMultiplier, movmentSpeed * Time.deltaTime * yMultiplier));
        //rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    void StopPlayer() {
        verticalMovment = 0;
        horizontalMovement = 0;
    }

}
