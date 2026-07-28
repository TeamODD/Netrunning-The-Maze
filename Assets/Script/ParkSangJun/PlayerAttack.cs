using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    [SerializeField] private float attackDamage = 10f;    // 공격력
    [SerializeField] private float attackCooldown = 0.5f; // 공격 쿨타임 (초)

    [Header("공격 판정(사각형)")]
    [SerializeField] private Vector2 boxSize = new Vector2(1.5f, 1f); // 타격 박스 가로/세로 크기
    [SerializeField] private float xOffset = 1f;                      // 플레이어 중심에서 앞쪽으로 얼마나 떨어져 있는지
    [SerializeField] private LayerMask enemyLayer;                    // 타격할 적 레이어

    private float nextAttackTime = 0f; // 다음 공격이 가능한 시간

    private void Update()
    {
        // 쿨타임이 지났고, 마우스 좌클릭을 눌렀을 때 공격 실행
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0))
        {
            Attack();
            // 현재 시간에 쿨타임을 더해서 다음 공격 가능 시간을 설정
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack()
    {
        // 1. 플레이어가 바라보는 방향 계산 (캐릭터의 scale X값 부호를 활용)
        float facingDirection = Mathf.Sign(transform.localScale.x);

        // 2. 타격 박스의 중심 위치 계산 (플레이어 위치 + X축 오프셋 * 방향)
        Vector2 boxCenter = (Vector2)transform.position + new Vector2(xOffset * facingDirection, 0f);

        // 3. 해당 사각형 범위 안의 'enemyLayer'에 속한 모든 콜라이더 감지
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, enemyLayer);

        // 4. 감지된 적들에게 데미지 전달 (태그 검사 제거)
        foreach (Collider2D hit in hitEnemies)
        {
            // 레이어로 이미 걸러졌기 때문에, 바로 스크립트를 가져와서 실행
            EnemyStatus enemyStatus = hit.GetComponent<EnemyStatus>();
            if (enemyStatus != null)
            {
                enemyStatus.TakeDamage(attackDamage);
            }
        }
    }

    // 씬 뷰에서 공격 범위를 빨간색 사각형으로 확인하기 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        float facingDirection = 1f;

        // 게임 실행 중일 때는 실시간 방향을 반영
        if (Application.isPlaying)
        {
            facingDirection = Mathf.Sign(transform.localScale.x);
        }

        Vector2 boxCenter = (Vector2)transform.position + new Vector2(xOffset * facingDirection, 0f);

        Gizmos.color = Color.red;
        // 씬 뷰에 사각형(WireCube) 그려주기
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}