using System.Collections.Generic;
using UnityEngine;

public enum SectorType
{
    Start,        // 시작 섹터
    Normal,       // 일반 섹터
    Admin,        // 관리자 섹터
    Stable        // 안정화 섹터
}

public class SectorManager : MonoBehaviour
{
    private static readonly Vector2Int[] SectorDirection =
    {
        new Vector2Int(0, 1),       // 상
        new Vector2Int(0, -1),      // 하
        new Vector2Int(-1, 0),      // 좌
        new Vector2Int(1, 0)        // 우
    };

    [Header("생성할 시작 섹터 프리팹"), SerializeField]   
    private GameObject[] _startSectorPrefab;
    
    [Header("생성할 일반 섹터 프리팹"), SerializeField]
    private GameObject[] _normalSectorPrefab;
    
    [Header("생성할 관리자 섹터 프리팹"), SerializeField]
    private GameObject[] _adminSectorPrefab;
    
    [Header("생성할 일반섹터 프리팹"), SerializeField]
    private GameObject[] _stableSectorPrefab;
    
    private Dictionary<Vector2Int, GameObject> _sectorMap = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<Vector2Int, GameObject> SectorMap => _sectorMap;

    [Header("현재 플레이어가 위치한 섹터 좌표"), SerializeField]
    private Vector2Int _currSectorPos;
    public Vector2Int CurrSectorPos => _currSectorPos;

    private float _sectorWidth;
    private float _sectorHeight;

    private void Awake()
    {
        _currSectorPos = new Vector2Int(0, 0);

        _sectorMap[new Vector2Int(0, 0)]
            = Instantiate(_startSectorPrefab[0], new Vector3(0, 0, 0), Quaternion.identity);
        

        // 나중에 수정해야 할 가능성 높음
        _sectorWidth = _startSectorPrefab[0].transform.localScale.x;
        _sectorHeight = _startSectorPrefab[0].transform.localScale.y;
    }

    /// <summary>
    /// 현재 섹터 좌표 기준 생성 가능한 좌표에 섹터 생성
    /// </summary>
    /// <param name="currPos">현재 섹터 위치</param>
    [ContextMenu("다음 섹터 생성")]
    private void SpawnNextSector()
    {
        
        // 중복 섹터 삭제
        foreach(Vector2Int item in SectorDirection)
        {
            Vector2Int nextSectorPos = item + _currSectorPos;
            if(!_sectorMap.TryGetValue(nextSectorPos, out var val))
            {
                // TODO: 여기서 조건에 따라(또는 무작위로) 3가지 섹터 유형 중 하나를 생성
                // 현재는 단순히 NormalSector를 생성하는 것으로 함

                Vector3 spawnPos = new Vector3(nextSectorPos.x * _sectorWidth,
                                               nextSectorPos.y * _sectorHeight);

                GameObject sectorObj = Instantiate(_normalSectorPrefab[0],
                                        spawnPos, Quaternion.identity);
                
                SectorData data = sectorObj.GetComponent<SectorData>();
                data.Init(nextSectorPos);

                _sectorMap[nextSectorPos] = sectorObj;
            }
        }
    }


}
