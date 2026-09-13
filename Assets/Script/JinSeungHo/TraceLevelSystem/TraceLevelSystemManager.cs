using System;
using UnityEngine;

public class TraceLevelSystemManager : MonoBehaviour
{
    public static TraceLevelSystemManager Instance;

    /// <summary>
    /// Trace 수치가 변경되었을 때 발행 (HUD, 플레이어, 적 구독용)
    /// </summary>
    public static event Action<int> OnTraceChanged;

    /// <summary>
    /// 20킬 배수에 도달 시 발행 
    /// </summary>
    public static event Action<int> OnKillMilestoneReached;

    [Header("추적 레벨 (누적 적 처치 수)"), SerializeField]
    private int _trace;
    public int Trace => _trace;

    /// <summary>
    /// 플레이어 공격력 반환 = floor(40 + 60 * (Trace / 120)^1.6)
    /// </summary>
    public int PlayerATK
    {
        get
        {
            float ratio = (float)_trace / 120f;
            return Mathf.FloorToInt(40f + 60f * Mathf.Pow(ratio, 1.6f));
        }
    }

    /// <summary>
    /// 적 공격력 반환 = floor(20 + 80 * (Trace / 180)^1.6)
    /// </summary>
    public int EnemyATK
    {
        get
        {
            float ratio = (float)_trace / 180f;
            return Mathf.FloorToInt(20f + 80f * Mathf.Pow(ratio, 1.6f));
        }
    }

    /// <summary>
    /// 근접 적 공격력 (EnemyATK * 1.0)
    /// </summary>
    public int MeleeEnemyATK => Mathf.FloorToInt(EnemyATK * 1.0f);

    /// <summary>
    /// 드론 탄환 공격력 (EnemyATK * 0.5)
    /// </summary>
    public int DroneEnemyATK => Mathf.FloorToInt(EnemyATK * 0.5f);

    private void Awake()
    {
        if(Instance == null)    Instance = this;
        else                    Destroy(gameObject);

        _trace = 0;
    }

    /// <summary>
    /// 적 처치 시 호출되어 Trace 수치를 1 증가시키고 이벤트를 발행
    /// </summary>
    public void IncreaseKillCount()
    {
        _trace++;

        // Trace 변경 이벤트 전달
        OnTraceChanged?.Invoke(_trace);

        // 정확히 20 배수로 1회씩 이벤트 발행
        if (_trace > 0 && _trace % 20 == 0)
        {
            OnKillMilestoneReached?.Invoke(_trace);
        }
    }

    /// <summary>
    /// 런 재시작 등을 위한 초기화
    /// </summary>
    public void ResetTrace()
    {
        _trace = 0;
        OnTraceChanged?.Invoke(_trace);
    }
}
