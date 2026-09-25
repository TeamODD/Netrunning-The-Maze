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

    public static InfiltrationModule Instance;

    // 침투 모듈 스택
    private int[] _moduleStack = { 0, 0, 0, 0, 0, 0, 0 };
    public int[] ModuleStack => _moduleStack;

    private void Start()
    {
        if(Instance == null)    Instance = this;
        else                    Destroy(gameObject);
    }

    private void OnEnable()
    {
        TraceLevelSystemManager.OnKillMilestoneReached += HandleKillMilestone;
    }

    private void OnDisable()
    {
        TraceLevelSystemManager.OnKillMilestoneReached -= HandleKillMilestone;
    }

    /// <summary>
    /// 획득 모듈 수 반환
    /// </summary>
    /// <returns>획득 모듈 수</returns>
    public int TotalModuleCount()
    {
        int total = 0;
        foreach(int count in _moduleStack)  total += count;
        return total;
    }

    /// <summary>
    /// 모듈 타입에 따라 받는 효과 가중치 반환
    /// </summary>
    /// <param name="met">모듈 타입</param>
    /// <returns></returns>
    public float GetModuleStat(ModuleEffectType met)
    {
        if(met == ModuleEffectType.Amplification)
            return ATTACKPOWER_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.Amplification];
        
        else if(met == ModuleEffectType.LightSpeed)
            return ATTACKSPEED_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.LightSpeed];
        
        else if(met == ModuleEffectType.Stability)
            return MAXHPINCREASE_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.Stability];
        
        else if(met == ModuleEffectType.Critical)
            return 1 - Mathf.Pow(0.9f, _moduleStack[(int)ModuleEffectType.Critical]);

        else if(met == ModuleEffectType.Breakthrough) 
            return DASHATTACKPOWER_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.Breakthrough];
        
        else if(met == ModuleEffectType.Absorption)
            return LIFESTEAL_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.Absorption];
        
        else if(met == ModuleEffectType.Rampage)
            return BERSERK_INCREASE_PER_STACK * _moduleStack[(int)ModuleEffectType.Rampage];

        else return -1;
    }

    /// <summary>
    /// 특정 모듈 타입 스택 횟수를 조정함, stack이 음수일 경우 감소
    /// </summary>
    /// <param name="met">모듈 타입</param>
    /// <param name="stack">증감할 스택 횟수</param>
    /// <returns>스택 증감 성공/실패 여부 반환</returns>
    private bool UpdateModuleStack(ModuleEffectType met, int stack)
    {
        if(_moduleStack[(int)met] + stack < 0f) return false;

        _moduleStack[(int)met] += stack;

        return true;
    }

    private void HandleKillMilestone(int trace)
    {
        ModuleEffectType randModule = (ModuleEffectType)Random.Range(0, 7);
        UpdateModuleStack(randModule, 1);
        
        Debug.Log($"모듈 지급 | {randModule} : {_moduleStack[(int)randModule]}");
    }
}
