using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMover : MonoBehaviour
{
    private const float MoveAnimationDampTime = 0.1f;

    
    [Header("Character")]
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Rigidbody characterRigidbody;
    
    [Header("MoveSpeed")]
    [SerializeField] private float forwardSpeed = 4f;
    [SerializeField] private float backwardSpeed = 2f;
    [SerializeField] private float sideSpeed = 5f;
    [SerializeField] private float sprintSpeed = 3f;
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    
    [Header("Rotation")]
    [SerializeField] private Transform cameraTransform;

    private Vector2 moveDirection;
    private float speedX;
    private float speedY;
    private float forwardMaxSpeed => forwardSpeed + sprintSpeed;

    private void Start()
    {
        Application.targetFrameRate = 240;
    }

    private void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Vertical");
        moveDirection.y = Input.GetAxisRaw("Horizontal");
        var sprintActive = Input.GetKey(KeyCode.LeftShift);

        var speedXMul = GetSpeedXMultiplier(moveDirection.x, sprintActive);
        
        speedX = moveDirection.x * speedXMul;
        speedY = moveDirection.y * sideSpeed;
    }

    private void FixedUpdate()
    {
        Move(speedX, speedY);
    }

    private void LateUpdate()
    {
        UpdateAnimation(speedX, speedY);
        
        characterTransform.transform.eulerAngles = new Vector3(characterTransform.transform.eulerAngles.x, cameraTransform.eulerAngles.y, characterTransform.transform.eulerAngles.z);
    }

    private void Move(float speedX, float speedY)
    {
        var moveXDelta = speedX * Time.fixedDeltaTime;
        var moveYDelta = speedY * Time.fixedDeltaTime;
        var forwardNormalized = characterTransform.transform.forward.normalized;
        var rightNormalized = characterTransform.transform.right.normalized;
        
        var resultMovement = characterRigidbody.position + (rightNormalized * moveYDelta) + (forwardNormalized * moveXDelta);
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
}