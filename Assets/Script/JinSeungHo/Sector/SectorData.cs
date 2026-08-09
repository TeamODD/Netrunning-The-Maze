using System.Collections.Generic;
using UnityEngine;

public class SectorData : MonoBehaviour
{
    private const int MAX_SEED_VALUE = 100000000;
    public const float WIDTH = 50;
    public const float HEIGHT = 28;
    
    /// <summary>
    /// 월드 상에서 현재 섹터의 상하좌우 출입구의 상대 좌표
    /// </summary>
    public static readonly Vector2[] ENTRY_POSITION =
    {
        new Vector2(0, HEIGHT / 2),
        new Vector2(0, -HEIGHT / 2),
        new Vector2(-WIDTH / 2, 0),
        new Vector2(WIDTH / 2, 0)
    };

    [SerializeField, Tooltip("섹터 타입")]      private SectorType _type;

    [SerializeField, Tooltip("섹터 상태")]
    private SectorState _state;
    public SectorState State => _state;

    private Vector2Int _sectorPosition;
    public Vector2Int SectorPosition => _sectorPosition;

    private int _seed;
    public int SectorSeed => _seed;

    private List<GameObject> _aliveEnemyList = new List<GameObject>();
    private List<GameObject> AliveEnemyList => _aliveEnemyList;

    public void Init(Vector2Int currPos)
    {
        _state = SectorState.Active;

        _sectorPosition = currPos;

        _seed = Random.Range(0, MAX_SEED_VALUE);
    }

    /// <summary>
    /// 현재 섹터 상태를 인자 state로 바꿈
    /// </summary>
    /// <param name="state">변환할 섹터 상태</param>
    public void ChangeState(SectorState state) => _state = state;

    public void AddEnemy(GameObject enemy)
    {
        if(enemy != null && !_aliveEnemyList.Contains(enemy))
            _aliveEnemyList.Add(enemy);
    }
}