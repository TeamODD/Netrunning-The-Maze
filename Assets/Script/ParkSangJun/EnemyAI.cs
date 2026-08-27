using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("로밍 설정")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float roamDistance = 3f;

    [Header("감지 레이저 및 어그로 설정")]
    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private float deaggroDistance = 10f;
    [SerializeField] private LayerMask playerLayer;

    [Header("공격 감지 (원) 설정")]
    [SerializeField] private Vector2 attackCheckOffset = new Vector2(0.8f, 0f); // 원 중심 위치 오프셋
    [SerializeField] private float attackCheckRadius = 0.8f;                     // 공격 시도 원 반지름

    [Header("기즈모 설정")]
    [SerializeField] private float roamGizmoOffsetY = -0.5f;

    private Vector3 startPosition;
    private Rigidbody2D rb;
    private EnemyAttack enemyAttack;
    private Transform playerTransform;

    private bool movingRight = true;
    private bool isChasing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponent<EnemyAttack>();
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
            DetectPlayerRaycast();
        }
    }

    private void FixedUpdate()
    {
        // 1. 공격 범위 원 안에 플레이어가 들어왔는지 체크
        if (CheckPlayerInAttackRange())
        {
            // 공격 범위 안이면 멈추고 공격 실행
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            if (enemyAttack != null && enemyAttack.CanAttack)
            {
                enemyAttack.ExecuteAttack();
            }
            return;
        }

        // 2. 이동 로직
        if (isChasing)
        {
            if (playerTransform == null) return;

            float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            float direction = movingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
    }

    // 원형 범위(OverlapCircle)로 공격 가능 플레이어 감지
    private bool CheckPlayerInAttackRange()
    {
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector2 circleCenter = (Vector2)transform.position + new Vector2(attackCheckOffset.x * facingDirection, attackCheckOffset.y);

        Collider2D hit = Physics2D.OverlapCircle(circleCenter, attackCheckRadius, playerLayer);
        return hit != null && hit.CompareTag("Player");
    }

    private void DetectPlayerRaycast()
    {
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector2 rayDirection = Vector2.right * facingDirection;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, detectDistance, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isChasing = true;
            playerTransform = hit.transform;
        }
    }

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

    private void HandleChaseLogic()
    {
        if (playerTransform == null)
        {
            isChasing = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > deaggroDistance)
        {
            isChasing = false;
            playerTransform = null;
            return;
        }

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

        // 1. 좌우 로밍 범위 (청록색)
        Vector3 gizmoCenter = origin + new Vector3(0f, roamGizmoOffsetY, 0f);
        Vector3 leftBound = new Vector3(gizmoCenter.x - roamDistance, gizmoCenter.y, gizmoCenter.z);
        Vector3 rightBound = new Vector3(gizmoCenter.x + roamDistance, gizmoCenter.y, gizmoCenter.z);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(leftBound, rightBound);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftBound, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rightBound, 0.2f);

        // 2. 전방 레이저 (노란색)
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector2 rayDirection = Vector2.right * facingDirection;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, rayDirection * detectDistance);

        // 3. 공격 감지 원 (주황색)
        Vector2 circleCenter = (Vector2)transform.position + new Vector2(attackCheckOffset.x * facingDirection, attackCheckOffset.y);
        Gizmos.color = new Color(1f, 0.5f, 0f); // Magenta / Orange
        Gizmos.DrawWireSphere(circleCenter, attackCheckRadius);
    }
}