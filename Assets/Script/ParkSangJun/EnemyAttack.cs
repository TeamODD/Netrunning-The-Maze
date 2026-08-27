using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("공격 범위 (네모) 설정")]
    [SerializeField] private Vector2 attackBoxSize = new Vector2(1.5f, 1f);   // 네모 공격 범위 크기 (가로, 세로)
    [SerializeField] private Vector2 attackBoxOffset = new Vector2(0.8f, 0f); // 적 중심 기준 공격 박스 위치
    [SerializeField] private float attackDamage = 10f;                         // 공격력
    [SerializeField] private float attackCooldown = 1.5f;                      // 공격 쿨타임
    [SerializeField] private LayerMask playerLayer;                            // 플레이어 레이어

    private float lastAttackTime;
    private Animator anim; // 1. 애니메이터 변수 추가

    // 공격 가능 여부 프로퍼티
    public bool CanAttack => Time.time >= lastAttackTime + attackCooldown;

    private void Awake()
    {
        // 2. 애니메이터 가져오기 (스프라이트가 자식 오브젝트에 있다면 GetComponentInChildren 사용)
        anim = GetComponentInChildren<Animator>();
    }

    public void ExecuteAttack()
    {
        if (!CanAttack) return;

        lastAttackTime = Time.time;

        // 3. 공격 애니메이션 트리거 실행
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }

        // 적이 바라보는 방향 계산 (scale.x 부호 기준)
        float facingDirection = Mathf.Sign(transform.localScale.x);

        // 바라보는 방향에 맞춰 사각형 중심점 계산
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(attackBoxOffset.x * facingDirection, attackBoxOffset.y);

        // 사각형 범위(OverlapBox) 내 플레이어 감지
        Collider2D hitPlayer = Physics2D.OverlapBox(boxCenter, attackBoxSize, 0f, playerLayer);

        if (hitPlayer != null && hitPlayer.CompareTag("Player"))
        {
            PlayerStatus playerStatus = hitPlayer.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 씬 뷰 시각화: 빨간색 사각형 (공격 히트박스)
        float facingDirection = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(attackBoxOffset.x * facingDirection, attackBoxOffset.y);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, attackBoxSize);
    }
}