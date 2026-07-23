using System.Collections.Generic;
using UnityEngine;

public class SectorManager : MonoBehaviour
{
    public static SectorManager Instance;

    [Header("섹터 스포너"), SerializeField]
    private SectorSpawn _sectorSpawner;

    [Header("현재 플레이어가 위치한 섹터 좌표"), SerializeField]
    private Vector2Int _currPlayerPos;
    public Vector2Int CurrPlayerPos => _currPlayerPos;

    private Dictionary<Vector2Int, GameObject> _sectorMap = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<Vector2Int, GameObject> SectorMap => _sectorMap;

    private float _stayDuration;

    private void Awake()
    {
        if(Instance == null)    Instance = this;
        else                    Destroy(gameObject);

        _currPlayerPos = new Vector2Int(0, 0);

        if(_sectorSpawner == null)  _sectorSpawner = GetComponent<SectorSpawn>();

        _sectorSpawner.SpawnStartSector(_sectorMap);
        _sectorSpawner.SpawnNextSector(_currPlayerPos, _sectorMap);

        _stayDuration = 0;
    }

    [ContextMenu("테스트/섹터 생성")]
    private void Test_SpawnSector() => _sectorSpawner.SpawnNextSector(_currPlayerPos, _sectorMap);
}