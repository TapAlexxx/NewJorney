using System;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private float forwardSpeed = 4f;
    [SerializeField] private float backwardSpeed = 2f;
    [SerializeField] private float sideSpeed = 5f;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private Animator animator;
    
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

    private void LateUpdate()
    {
        Move(speedX, speedY);
        UpdateAnimation(speedX, speedY);
        
        character.transform.eulerAngles = new Vector3(character.transform.eulerAngles.x, cameraTransform.eulerAngles.y, character.transform.eulerAngles.z);
    }

    private void Move(float speedX, float speedY)
    {
        var moveXDelta = speedX * Time.deltaTime;
        var moveYDelta = speedY * Time.deltaTime;
        var forwardNormalized = character.transform.forward.normalized;
        var rightNormalized = character.transform.right.normalized;
        
        var resultMovement = (rightNormalized * moveYDelta) + (forwardNormalized * moveXDelta);
        character.transform.position += resultMovement;
    }

    private void UpdateAnimation(float speedX, float speedY)
    {
        animator.SetFloat("moveDirectionX", speedX / (moveDirection.x >= 0 ? forwardMaxSpeed : backwardSpeed), 0.1f, Time.deltaTime);
        animator.SetFloat("moveDirectionY", speedY / sideSpeed, 0.1f, Time.deltaTime);
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