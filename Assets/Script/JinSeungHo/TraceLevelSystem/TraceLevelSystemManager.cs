using UnityEngine;

public class TraceLevelSystemManager : MonoBehaviour
{
    public static TraceLevelSystemManager Instance;

    public const float DAMAGE_TAKEN_MODIFIER = 0.01f;   // 받는 데미지 증가율
    public const float TOTAL_DAMAGE_MODIFIER = 0.01f;   // 주는 데미지 증가율
    public const float ENEMY_MAXHP_MODIFIER = 0.005f;   // 적 최대 체력 증가율

    [Header("적 처치 수"), SerializeField]
    private int _totalKill;

    [Header("현재 받는 데미지 증가량"), SerializeField]
    private float _currDamageTakenMod;
    public float CurrDamageTakenMod => _currDamageTakenMod;

    [Header("현재 주는 데미지 증가량"), SerializeField]
    private float _currTotalDamageMod;
    public float CurrTotalDamageMod => CurrTotalDamageMod;
    
    [Header("현재 적 최대 체력 증가량"), SerializeField]
    private float _currEnemyMaxhpMod;
    public float CurrEnemyMaxhpMod => _currEnemyMaxhpMod;

    private void Awake()
    {
        if(Instance == null)    Instance = this;
        else                    Destroy(gameObject);

        _totalKill = 0;
        _currDamageTakenMod = 0;
        _currEnemyMaxhpMod = 0;
        _currTotalDamageMod = 0;
    }

    public void IncreaseKillCount()
    {
        ++_totalKill;
        _currDamageTakenMod += DAMAGE_TAKEN_MODIFIER;
        _currTotalDamageMod += TOTAL_DAMAGE_MODIFIER;
        _currEnemyMaxhpMod += ENEMY_MAXHP_MODIFIER;
    }
}
