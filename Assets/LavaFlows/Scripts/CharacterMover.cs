using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    private const float MoveAnimationDampTime = 0.1f;

    
    [Header("Character")]
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Rigidbody characterRigidbody;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;
    
    [Header("MoveSpeed")]
    [SerializeField] private float forwardSpeed = 4f;
    [SerializeField] private float backwardSpeed = 2f;
    [SerializeField] private float sideSpeed = 5f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    
    [Header("Rotation")]
    [SerializeField] private Transform cameraTransform;

    private Vector2 moveDirection;
    private bool sprintActive;
    private float verticalVelocity;

    private float forwardMaxSpeed => forwardSpeed + sprintSpeed;
    private bool isGrounded => Physics.Raycast(characterTransform.position, Vector3.down, groundCheckDistance, groundMask);
    
    
    private void Start()
    {
        Application.targetFrameRate = 240;
    }

    private void Update()
    {

        moveDirection.x = Input.GetAxisRaw("Vertical");
        moveDirection.y = Input.GetAxisRaw("Horizontal");

        sprintActive = Input.GetKey(KeyCode.LeftShift) && isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void FixedUpdate()
    {
        var speed = CalculateSpeed();

        Move(speed.x, speed.y);
    }

    private void LateUpdate()
    {
        var speed = CalculateSpeed();
        UpdateAnimation(speed.x, speed.y);
        
        characterTransform.transform.eulerAngles = new Vector3(characterTransform.transform.eulerAngles.x, cameraTransform.eulerAngles.y, characterTransform.transform.eulerAngles.z);
    }

    private void Move(float speedX, float speedY)
    {
        var moveXDelta = speedX * Time.fixedDeltaTime;
        var moveYDelta = speedY * Time.fixedDeltaTime;
        var forwardNormalized = characterTransform.transform.forward.normalized;
        var rightNormalized = characterTransform.transform.right.normalized;
        
        var resultMovement = characterRigidbody.position + (rightNormalized * moveYDelta) + (forwardNormalized * moveXDelta);


        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = 0;
        else
            verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;
        
        resultMovement.y += verticalVelocity * Time.fixedDeltaTime;

        characterRigidbody.MovePosition(resultMovement);
    }

    private void UpdateAnimation(float speedX, float speedY)
    {
        animator.SetFloat("moveDirectionX", speedX / (moveDirection.x >= 0 ? forwardMaxSpeed : backwardSpeed), MoveAnimationDampTime, Time.deltaTime);
        animator.SetFloat("moveDirectionY", speedY / sideSpeed, MoveAnimationDampTime, Time.deltaTime);
    }

    private float GetSpeedXMultiplier(float moveDirectionX, bool sprintActive)
    {
        if (moveDirectionX >= 0)
        {
            if (sprintActive)
                return forwardSpeed + sprintSpeed;
            return forwardSpeed;
        }
        
        return backwardSpeed;
    }

    private Vector2 CalculateSpeed()
    {
        var speedXMul = GetSpeedXMultiplier(moveDirection.x, sprintActive);
        
        return new Vector2(moveDirection.x * speedXMul, moveDirection.y * sideSpeed);
    }
}