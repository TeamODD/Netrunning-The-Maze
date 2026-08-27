using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    private PlayerMovement playerMovement;

    protected override void Awake()
    {
        base.Awake(); // 부모(CharacterStatus)의 Awake() 먼저 실행 (currentHp = maxHp 초기화)
        playerMovement = GetComponent<PlayerMovement>();
    }

    // 부모의 TakeDamage를 가져와서 무적 체크 로직을 추가
    public override void TakeDamage(float damage)
    {
        // 대시 중(무적 상태)이라면 데미지를 무시하고 리턴
        if (playerMovement != null && playerMovement.isInvincible)
        {
            Debug.Log("플레이어가 대시 무적 상태이므로 데미지를 받지 않습니다.");
            return;
        }

        // 무적이 아니라면 부모 클래스의 기본 TakeDamage(데미지 차감 및 HP 제한) 실행
        base.TakeDamage(damage);
    }

    // 부모의 Die를 가져와서 플레이어 전용 사망 처리 작성
    protected override void Die()
    {
        base.Die();
    }
}