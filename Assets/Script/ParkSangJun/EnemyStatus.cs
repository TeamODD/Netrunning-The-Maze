using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}