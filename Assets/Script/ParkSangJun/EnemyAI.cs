using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("로밍 설정")]
    [SerializeField] private float moveSpeed = 2f;      // 이동 속도
    [SerializeField] private float roamDistance = 3f;  // 시작 위치 기준 좌/우 로밍 거리

    [Header("감지 레이저 설정")]
    [SerializeField] private float detectDistance = 5f; // 전방 레이저 거리

    [Header("기즈모 설정")]
    [SerializeField] private float roamGizmoOffsetY = -0.5f; // 로밍 기즈모 Y축 높이 조절 (마이너스 값 = 아래로 이동)

    private Vector3 startPosition;
    private Rigidbody2D rb;
    private bool movingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Update()
    {
        float currentX = transform.position.x;
        float leftLimit = startPosition.x - roamDistance;
        float rightLimit = startPosition.x + roamDistance;

        // 오른쪽 한계선 도달 시 전환
        if (movingRight && currentX >= rightLimit)
        {
            movingRight = false;
            Flip();
        }
        // 왼쪽 한계선 도달 시 전환
        else if (!movingRight && currentX <= leftLimit)
        {
            movingRight = true;
            Flip();
        }
    }

    private void FixedUpdate()
    {
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = movingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    // [수정] OnDrawGizmos -> OnDrawGizmosSelected 로 변경!
    // 이제 하이어라키나 씬 뷰에서 이 에너미를 클릭(선택)했을 때만 기즈모가 보입니다.
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = Application.isPlaying ? startPosition : transform.position;

        // 1. 좌우 로밍 범위 표기
        Vector3 gizmoCenter = origin + new Vector3(0f, roamGizmoOffsetY, 0f);
        Vector3 leftBound = new Vector3(gizmoCenter.x - roamDistance, gizmoCenter.y, gizmoCenter.z);
        Vector3 rightBound = new Vector3(gizmoCenter.x + roamDistance, gizmoCenter.y, gizmoCenter.z);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(leftBound, rightBound);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftBound, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rightBound, 0.2f);

        // 2. 바라보는 방향 전방 레이저 표기
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector2 rayDirection = Vector2.right * facingDirection;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, rayDirection * detectDistance);
    }
}