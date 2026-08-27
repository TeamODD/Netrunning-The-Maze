using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [Header("기본 스탯")]
    [SerializeField] protected float maxHp = 100f; // 최대 체력
    protected float currentHp;                     // 현재 체력

    public float CurrentHp => currentHp;           // 외부 읽기용 프로퍼티
    public float MaxHp => maxHp;

    protected virtual void Awake()
    {
        currentHp = maxHp; // 게임 시작 시 현재 체력을 최대 체력으로 초기화
    }

    // 데미지를 받는 공통 피격 함수
    public virtual void TakeDamage(float damage)
    {
        // 이미 죽어있는 상태라면 추가 피격 무시
        if (currentHp <= 0f) return;

        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp); // 체력이 0 미만으로 떨어지지 않게 제한

        Debug.Log($"{gameObject.name}에게 {damage}데미지 (남은 체력: {currentHp}/{maxHp})");

        // 체력이 0 이하가 되면 사망 처리
        if (currentHp <= 0f)
        {
            Die();
        }
    }

    // 사망 처리 함수 (자식 클래스에서 각각 오버라이드하여 구체적인 로직 작성)
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} 사망!");
    }
}