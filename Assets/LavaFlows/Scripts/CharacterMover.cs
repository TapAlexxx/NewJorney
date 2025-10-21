using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private float forwardSpeed = 6f;
    [SerializeField] private float backwardSpeed = 3f;
    [SerializeField] private float sideSpeed = 5f;
    [SerializeField] private Animator animator;

    private Vector2 moveDirection;
    
    private void Update()
    {
        moveDirection.x = Input.GetAxis("Vertical");
        moveDirection.y = Input.GetAxis("Horizontal");

        var speedX = moveDirection.x * (moveDirection.x >= 0 ? forwardSpeed : backwardSpeed);
        var speedY = moveDirection.y * sideSpeed;
        Move(speedX, speedY);
        
        animator.SetFloat("moveDirectionX", moveDirection.x);
        animator.SetFloat("moveDirectionY", moveDirection.y);
    }

    private void Move(float speedX, float speedY)
    {
        transform.position += new Vector3(speedY * Time.deltaTime, 0, speedX * Time.deltaTime);
    }
}