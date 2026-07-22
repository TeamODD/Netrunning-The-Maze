using System.Collections.Generic;
using UnityEngine;

public class SectorData : MonoBehaviour
{
    private const int MAX_SEED_VALUE = 100000000;

    [SerializeField, Tooltip("섹터 타입")]      private SectorType _sectorType;

    private Vector2Int _sectorPosition;
    public Vector2Int SectorPosition => _sectorPosition;

    private int _seed;
    public int RandomSeed => _seed;

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
        _sectorPosition = currPos;

        _seed = Random.Range(0, MAX_SEED_VALUE);

        _isVisited = false;
        _isCleared = false;
        _isDeleteProtocolOn = false;
    }


}