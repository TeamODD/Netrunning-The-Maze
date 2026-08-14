using UnityEngine;

public class InfiltrationModule : MonoBehaviour
{
    /// <summary>
    /// 증폭 모듈 스택 당 공격력 증가율
    /// </summary>
    private readonly float ATTACKPOWER_INCREASE_PER_STACK = 0.1f;
    
    /// <summary>
    /// 광속 모듈 스택 당 공격 속도 증가율
    /// </summary>
    private readonly float ATTACKSPEED_INCREASE_PER_STACK = 0.1f;

    /// <summary>
    /// 안정 모듈 스택 당 최대 체력 증가율
    /// </summary>
    private readonly float MAXHPINCREASE_INCREASE_PER_STACK = 15f;

    /// <summary>
    /// 돌파 모듈 스택 당 대시 직후 첫 공격 피해 증가율
    /// </summary>
    private readonly float DASHATTACKPOWER_INCREASE_PER_STACK = 0.2f;

    /// <summary>
    /// 흡수 모듈 스택 당 적 처치 시 체력 회복량
    /// </summary>
    private readonly float LIFESTEAL_INCREASE_PER_STACK = 0.01f;

    /// <summary>
    /// 폭주 모듈 스택 당 최종 피해, 받는 피해 증가율
    /// </summary>
    private readonly float BERSERK_INCREASE_PER_STACK = 0.15f;

    [SerializeField, Header("침투 모듈 스택")]
    private int[] _moduleStack = { 0, 0, 0, 0, 0, 0, 0 };
    public int[] ModuleStack => _moduleStack;

    /// <summary>
    /// 모듈 타입에 따라 받는 효과 가중치 반환
    /// </summary>
    /// <param name="met">모듈 타입</param>
    /// <returns></returns>
    public float GetModuleStat(ModuleEffectType met)
    {
        if(met == ModuleEffectType.AttackPower)
            return ATTACKPOWER_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.AttackPower];
        
        else if(met == ModuleEffectType.AttackSpeed)
            return ATTACKSPEED_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.AttackSpeed];
        
        else if(met == ModuleEffectType.MaxHPIncrease)
            return MAXHPINCREASE_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.MaxHPIncrease];
        
        else if(met == ModuleEffectType.CriticalChance)
            return 1 - Mathf.Pow(0.9f, _moduleStack[(int)ModuleEffectType.CriticalChance]);

        else if(met == ModuleEffectType.DashAttackPower) 
            return DASHATTACKPOWER_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.DashAttackPower];
        
        else if(met == ModuleEffectType.LifeSteal)
            return LIFESTEAL_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.LifeSteal];
        
        else if(met == ModuleEffectType.Berserk)
            return BERSERK_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.Berserk];

        else return -1;
    }

    /// <summary>
    /// 특정 모듈 타입 스택 횟수를 조정함, stack이 음수일 경우 감소
    /// </summary>
    /// <param name="met">모듈 타입</param>
    /// <param name="stack">증감할 스택 횟수</param>
    /// <returns>스택 증감 성공/실패 여부 반환</returns>
    public bool UpdateModuleStack(ModuleEffectType met, int stack)
    {
        if(_moduleStack[(int)met] - stack < 0f) return false;

        _moduleStack[(int)met] += stack;

        return true;
    }
}
