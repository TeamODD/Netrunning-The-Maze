using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    [SerializeField] private Transform groundCheck; // 바닥 감지 위치
    [SerializeField] private float checkRadius = 0.2f; // 감지 범위 크기
    [SerializeField] private LayerMask groundLayer; // 바닥으로 인식할 레이어

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 좌/우 방향키 및 A/D 입력 받기 (-1.0 ~ 1.0)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 바닥에 닿아있는지 체크 (발바닥 위치 원 범위 안에 groundLayer가 있는지)
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }

        // 점프 입력 (Space 키를 누르는 순간 + 바닥에 발이 닿아있을 때만)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Time.deltaTime 쓴것 같은 효과의 Update
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    // 에디터 씬 뷰에서 바닥 감지 범위를 빨간색 원으로 그려주는 용도
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}