using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 및 점프 설정")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 5f;

    [Header("조작감 개선 설정")]
    [SerializeField] private float jumpBufferTime = 0.1f; // 착지 직전 점프 예약 시간
    [SerializeField] private float coyoteTime = 0.1f;     // 절벽에서 떨어진 후 점프 허용 시간

    [Header("대시 설정")]
    [SerializeField] private float dashSpeed = 8f;      // 대시할 때 속도
    [SerializeField] private float dashDuration = 0.2f;  // 대시 지속 시간
    [SerializeField] private float dashCooldown = 0.5f;    // 대시 재사용 대기시간

    [Header("바닥 감지 설정")]
    [SerializeField] private Transform groundCheck;      // 바닥 감지 위치
    [SerializeField] private float checkRadius = 0.1f;   // 감지 범위 크기
    [SerializeField] private LayerMask groundLayer;      // 바닥으로 인식할 레이어

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;

    private bool isDashing;
    private bool canDash = true;
    private float facingDirection = 1f;

    // 타이머 변수
    private float jumpBufferCounter;
    private float coyoteTimeCounter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 대시 중일 때는 이동 및 점프 입력 무시
        if (isDashing) return;

        // 좌/우 방향키 및 A/D 입력 받기 (-1.0 ~ 1.0)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 입력이 있을 때 캐릭터 바라보는 방향 설정
        if (horizontalInput != 0)
        {
            facingDirection = Mathf.Sign(horizontalInput);
            transform.localScale = new Vector3(facingDirection, transform.localScale.y, transform.localScale.z);
        }

        #region 점프 로직

        // 1. 바닥 감지
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }

        // 2. 코요테 타임 타이머 제어
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // 땅에 있을 땐 계속 최대 시간으로 충전
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // 공중에 뜨면 카운트다운 시작
        }

        // 3. 점프 입력 버퍼 타이머 제어
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // 4. 점프 버퍼, 코요테 타임 조건을 만족하면 점프
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // 점프를 썼으니 두 타이머 모두 0으로 초기화 (공중 연속 점프 방지)
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        #endregion

        // 대시 입력 (LeftShift 키)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    // Time.deltaTime 쓴것 같은 효과의 Update
    private void FixedUpdate()
    {
        // 대시 중일 때: 바라보던 방향으로 대시 속도 적용
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(facingDirection * dashSpeed, rb.linearVelocity.y);
            return;
        }

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

    // 대시 처리 코루틴 (시간 제어용)
    private System.Collections.IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        // 대시 지속 시간만큼 대기 후 대시 종료
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        // 쿨타임 대기 후 다음 대시 가능 상태로 변경
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}