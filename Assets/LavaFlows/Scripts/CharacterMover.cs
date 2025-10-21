using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Animator animator;

    private Vector2 moveDirection;
    
    private void Update()
    {
        moveDirection.x = Input.GetAxis("Vertical");
        moveDirection.y = Input.GetAxis("Horizontal");
        
        Move();
        
        animator.SetFloat("moveDirectionX", moveDirection.x);
        animator.SetFloat("moveDirectionY", moveDirection.y);
    }

    private void Move()
    {
        transform.position += new Vector3(moveDirection.y * moveSpeed * Time.deltaTime, 0, moveDirection.x * moveSpeed * Time.deltaTime);
    }
}