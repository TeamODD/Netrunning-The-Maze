using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SectorData : MonoBehaviour
{
    private const int MAX_SEED_VALUE = 100000000;
    public const float WIDTH = 18;
    public const float HEIGHT = 10;
    
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

    [Header("테스트 전용 변수"), SerializeField]
    private TextMeshProUGUI tmp;

    private Vector2Int _sectorPosition;
    public Vector2Int SectorPosition => _sectorPosition;

    private int _seed;
    public int SectorSeed => _seed;

    private bool _isVisited;
    public bool IsVisited => _isVisited;

    private bool _isCleared;
    public bool IsCleared => _isCleared;

    private bool _isDeleteProtocolOn;
    private bool IsDeleteProtocolOn => _isDeleteProtocolOn;

    private List<GameObject> _aliveEnemyList = new List<GameObject>();
    private List<GameObject> AliveEnemyList => _aliveEnemyList;

    public void Init(Vector2Int currPos)
    {
        _state = SectorState.Stanby;

        _sectorPosition = currPos;

        _seed = Random.Range(0, MAX_SEED_VALUE);

        _isVisited = false;
        _isCleared = false;
        _isDeleteProtocolOn = false;

        Test_SectorName();
    }

    /// <summary>
    /// 현재 섹터 상태를 인자 state로 바꿈
    /// </summary>
    /// <param name="state">변환할 섹터 상태</param>
    public void ChangeState(SectorState state) => _state = state;

    private void Test_SectorName()
    {
        if(tmp == null) return;

        tmp.text = _seed.ToString() + "\n" + $"({_sectorPosition.x.ToString()}, {_sectorPosition.y.ToString()})";
    }
}