using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("로밍 설정")]
    [SerializeField] private float moveSpeed = 2f;      // 이동 속도
    [SerializeField] private float roamDistance = 3f;  // 시작 위치 기준 좌/우 로밍 거리

    [Header("감지 및 추적 설정")]
    [SerializeField] private float detectDistance = 5f; // 전방 레이저 감지 거리
    [SerializeField] private float stopDistance = 1.5f;  // 플레이어 근처에서 멈춰설 거리
    [SerializeField] private float deaggroDistance = 10f; // 너무 멀어지면 추적 해제(로밍 복귀) 거리
    [SerializeField] private LayerMask playerLayer;     // 플레이어 레이어

    [Header("기즈모 설정")]
    [SerializeField] private float roamGizmoOffsetY = -0.5f; // 로밍 기즈모 Y축 높이 조절

    private Vector3 startPosition;
    private Rigidbody2D rb;
    private bool movingRight = true;

    private Transform playerTransform;
    private bool isChasing = false; // 추적 상태 플래그

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Update()
    {
        if (isChasing)
        {
            HandleChaseLogic();
        }
        else
        {
            HandleRoamLogic();
            DetectPlayer(); // 평상시 전방 감지
        }
    }

    private void FixedUpdate()
    {
        if (isChasing)
        {
            if (playerTransform == null) return;

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // 멈춤 거리보다 멀면 플레이어쪽으로 이동
            if (distanceToPlayer > stopDistance)
            {
                float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
                rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            }
            else
            {
                // 일정 거리 안으로 가까워지면 멈춤
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }
        else
        {
            // 평상시 좌우 로밍 이동
            float direction = movingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
    }

    // 평상시 좌우 로밍 반환점 체크
    private void HandleRoamLogic()
    {
        float currentX = transform.position.x;
        float leftLimit = startPosition.x - roamDistance;
        float rightLimit = startPosition.x + roamDistance;

        if (movingRight && currentX >= rightLimit)
        {
            movingRight = false;
            Flip();
        }
        else if (!movingRight && currentX <= leftLimit)
        {
            movingRight = true;
            Flip();
        }
    }

    // 전방 레이저 감지
    private void DetectPlayer()
    {
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector2 rayDirection = Vector2.right * facingDirection;

        // 바라보는 방향으로 Raycast 발사
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, detectDistance, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isChasing = true;
            playerTransform = hit.transform;
        }
    }

    // 플레이어 추적 상태 관리 및 바라보기 처리
    private void HandleChaseLogic()
    {
        if (playerTransform == null)
        {
            isChasing = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // 일정 거리 이상 멀어지면 어그로 풀고 로밍으로 복귀
        if (distanceToPlayer > deaggroDistance)
        {
            isChasing = false;
            playerTransform = null;
            return;
        }

        // 플레이어가 있는 방향을 계속 바라보도록 설정
        float directionX = playerTransform.position.x - transform.position.x;
        if (Mathf.Abs(directionX) > 0.1f)
        {
            movingRight = directionX > 0;
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = movingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = Application.isPlaying ? startPosition : transform.position;

        // 1. 좌우 로밍 범위 표기 (청록색 선 & 끝점)
        Vector3 gizmoCenter = origin + new Vector3(0f, roamGizmoOffsetY, 0f);
        Vector3 leftBound = new Vector3(gizmoCenter.x - roamDistance, gizmoCenter.y, gizmoCenter.z);
        Vector3 rightBound = new Vector3(gizmoCenter.x + roamDistance, gizmoCenter.y, gizmoCenter.z);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(leftBound, rightBound);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftBound, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rightBound, 0.2f);

        // 2. 전방 레이저 감지 거리 표기 (노란색)
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector2 rayDirection = Vector2.right * facingDirection;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, rayDirection * detectDistance);

        // 3. 플레이어 멈춤 사거리 표기 (빨간색 원)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}