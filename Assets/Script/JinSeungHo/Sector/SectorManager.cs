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

    /// <summary>
    /// 현재 플레이어가 위치한 섹터의 삭제 프로토콜
    /// </summary>
    private DeleteProtocol _currSectorDeleteProtocol;

    private Dictionary<Vector2Int, GameObject> _sectorMap = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<Vector2Int, GameObject> SectorMap => _sectorMap;

    /// <summary>
    /// 현재 섹터에서 머문 시간
    /// </summary>
    private float _currStayDuration;

    private void Awake()
    {
        if(Instance == null)    Instance = this;
        else                    Destroy(gameObject);

        _currPlayerPos = new Vector2Int(0, 0);

        if(_sectorSpawner == null)  _sectorSpawner = GetComponent<SectorSpawn>();

        _sectorSpawner.SpawnStartSector(_sectorMap);
        _sectorSpawner.SpawnNextSector(_currPlayerPos, _sectorMap);

        _currSectorDeleteProtocol = _sectorMap[_currPlayerPos].GetComponent<DeleteProtocol>();

        _currStayDuration = 0;
    }

    private void Update()
    {
        // 프로토콜 시간 갱신
        _currStayDuration += Time.deltaTime;
        _currSectorDeleteProtocol.ProtocolUpdate(_currStayDuration);

        // TODO: 여기서 나중에 GetPurgeDamagePercentage() 를 통해
        // 플레이어에게 입힐 퍼센트 데미지 비례 양을 계산 후 플레이어 HP에 반영
    }

    /// <summary>
    /// 플레이어 섹터 위치를 인자 currPos의 위치로 업데이트
    /// </summary>
    /// <param name="currPos">현재 위치</param>
    private void UpdateCurrPlayerPosition(Vector2Int currPos)
    {
        _currPlayerPos = currPos;

        _currSectorDeleteProtocol = _sectorMap[currPos].GetComponent<DeleteProtocol>();

        _currStayDuration = _currSectorDeleteProtocol.StayDuration;
    }

    [ContextMenu("테스트/섹터 생성")]
    private void Test_SpawnSector() => _sectorSpawner.SpawnNextSector(_currPlayerPos, _sectorMap);
}