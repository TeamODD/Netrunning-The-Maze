using System;
using System.Collections.Generic;
using UnityEngine;

public class SectorSpawn : MonoBehaviour
{
    private static readonly Vector2Int[] SectorDirection =
    {
        Vector2Int.up,       // 상
        Vector2Int.down,      // 하
        Vector2Int.left,      // 좌
        Vector2Int.right        // 우
    };

    /// <summary>
    /// 섹터 생성시 SectorData를 전달해 섹터 내부 적 스폰을 하도록 함
    /// </summary>
    public static event Action<SectorData> OnSectorSpawned;

    

    [Header("생성할 시작 섹터 프리팹"), SerializeField]   
    private GameObject[] _startSectorPrefab;
    
    [Header("생성할 일반 섹터 프리팹"), SerializeField]
    private GameObject[] _normalSectorPrefab;
    
    [Header("생성할 관리자 섹터 프리팹"), SerializeField]
    private GameObject[] _adminSectorPrefab;
    
    [Header("생성할 일반섹터 프리팹"), SerializeField]
    private GameObject[] _stableSectorPrefab;

    /// <summary>
    /// 시작 섹터 배치 및 활성화
    /// </summary>
    /// <param name="sectorMap"></param>
    public void SpawnStartSector(Dictionary<Vector2Int, GameObject> sectorMap)
    {
        Vector2Int startPos = Vector2Int.zero;

        if(!sectorMap.ContainsKey(startPos))
        {
            GameObject startObj = Instantiate(_startSectorPrefab[0], Vector3.zero, Quaternion.identity);
            
            SectorData data = startObj.GetComponent<SectorData>();
            data.Init(startPos);    data.ChangeState(SectorState.Active);

            sectorMap[startPos] = startObj;
        }
    }

    /// <summary>
    /// 현재 섹터 좌표 기준 생성 가능한 좌표에 섹터 생성
    /// </summary>
    /// <param name="currPos">현재 섹터 위치</param>
    public void SpawnNextSector(Vector2Int currPos, Dictionary<Vector2Int, GameObject> sectorMap)
    {
        // 중복 섹터 좌표 거르고 섹터 생성
        foreach(Vector2Int item in SectorDirection)
        {
            Vector2Int nextSectorPos = item + currPos;

            if(!sectorMap.TryGetValue(nextSectorPos, out var val))
            {
                // TODO: 여기서 조건에 따라(또는 무작위로) 3가지 섹터 유형 중 하나를 생성
                // 현재는 단순히 NormalSector 타입만을 생성하는 것으로 함

                Vector3 spawnPos = new Vector3(nextSectorPos.x * SectorData.WIDTH,
                                               nextSectorPos.y * SectorData.HEIGHT);

                int rand = UnityEngine.Random.Range(0, _normalSectorPrefab.Length);

                GameObject sectorObj = Instantiate(_normalSectorPrefab[rand],
                                        spawnPos, Quaternion.identity);
                
                SectorData data = sectorObj.GetComponent<SectorData>();
                data.Init(nextSectorPos);

                sectorMap[nextSectorPos] = sectorObj;

                OnSectorSpawned?.Invoke(data);
            }
        }
    }
}
