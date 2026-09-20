using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    private PlayerMovement playerMovement;

    protected override void Awake()
    {
        base.Awake(); // �θ�(CharacterStatus)�� Awake() ���� ���� (currentHp = maxHp �ʱ�ȭ)
        playerMovement = GetComponent<PlayerMovement>();
    }

    // �θ��� TakeDamage�� �����ͼ� ���� üũ ������ �߰�
    public override void TakeDamage(float damage)
    {
        // ��� ��(���� ����)�̶�� �������� �����ϰ� ����
        if (playerMovement != null && playerMovement.isInvincible)
        {
            Debug.Log("�÷��̾ ��� ���� �����̹Ƿ� �������� ���� �ʽ��ϴ�.");
            return;
        }

        // ������ �ƴ϶�� �θ� Ŭ������ �⺻ TakeDamage(������ ���� �� HP ����) ����
        base.TakeDamage(damage);
    }

    // �θ��� Die�� �����ͼ� �÷��̾� ���� ��� ó�� �ۼ�
    protected override void Die()
    {
        base.Die();
    }
}