using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("감지 거리")]
    [SerializeField] private float detectDistance = 5f;

    // OnDrawGizmos를 사용하면 오브젝트를 선택하지 않아도 씬 뷰에 레이저가 항상 표시됩니다.
    private void OnDrawGizmos()
    {
        // 현재 바라보는 방향 판정 (localScale.x 부호 기준)
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector2 rayDirection = Vector2.right * facingDirection;

        // 레이저 그리시 선 색상 설정
        Gizmos.color = Color.yellow;

        // 적 위치에서 전방으로 레이저 그리기
        Gizmos.DrawRay(transform.position, rayDirection * detectDistance);
    }
}