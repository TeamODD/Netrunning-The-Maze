using UnityEngine;

public class RunScoreManager : MonoBehaviour
{
    private const int SCORE_PER_SECTOR = 100;
    private const int SCORE_PER_KILL = 20;
    private const int SCORE_PER_BOSS = 400;
    private const int SCORE_PER_MODULE = 80;
    private const int SURVIVAL_TIME_INTERVAL = 10; 
    private const int SCORE_PER_SURVIVAL_UNIT = 10;
    private const int SCORE_PER_MAX_HIT = 1;

    public static RunScoreManager Instance;

    private float _survivalTime;
    public uint SurvivalTime => (uint)Mathf.FloorToInt(_survivalTime);

    [SerializeField, Header("최종 점수")]
    private int _finalScore;
    public int FinalScore => _finalScore;

    [SerializeField, Tooltip("이번 회차 최대 단일 딜량")]
    private float _maxSingleHit = 0;

    public void UpdateMaxSingleHit(float damage) => _maxSingleHit = Mathf.Max(_maxSingleHit, damage);

    private void Awake()
    {
        if(Instance == null)    Instance = this;
        else                    Destroy(gameObject);
    }

    private void Start()
    {
        _survivalTime = 0;
    }

    private void Update()
    {
        if(GameStateManager.Instance != null && GameStateManager.Instance.CurrentState == GameState.Playing)
        {
            _survivalTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// 점수 계산 및 반환
    /// </summary>
    public int CalculateScore()
    {
         int sectors = (SectorManager.Instance != null) ? SectorManager.Instance.DiscoveredSectorCount : 0;
        int kills = (TraceLevelSystemManager.Instance != null) ? TraceLevelSystemManager.Instance.KillCount : 0;
        int modules = (InfiltrationModule.Instance != null) ? InfiltrationModule.Instance.TotalModuleCount() : 0;

        int sectorScore   = sectors * SCORE_PER_SECTOR;
        int killScore     = kills * SCORE_PER_KILL;
        int bossScore     = 0 * SCORE_PER_BOSS;                                     // TODO: 보스 처치 수 추후 연동
        int moduleScore   = modules * SCORE_PER_MODULE;
        int survivalScore = (int)(SurvivalTime / SURVIVAL_TIME_INTERVAL) * SCORE_PER_SURVIVAL_UNIT;
        int maxHitScore   = Mathf.FloorToInt(_maxSingleHit) * SCORE_PER_MAX_HIT;
        
        _finalScore = sectorScore + killScore + bossScore + moduleScore + survivalScore + maxHitScore;

        SaveHighScore(sectors, kills, _finalScore);

        return _finalScore;
    }

    /// <summary>
    /// 최고 점수 저장
    /// </summary>
    private void SaveHighScore(int sectors, int kills, int score)
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        
        if(score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            PlayerPrefs.SetInt("BestKills", kills);
            PlayerPrefs.SetInt("BestSectors", sectors);
            PlayerPrefs.Save();
        }
    }
}
