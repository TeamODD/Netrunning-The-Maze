using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SectorManager : MonoBehaviour
{
    public static SectorManager Instance;

    [Header("섹터 스포너"), SerializeField]
    private SectorSpawn _sectorSpawner;

    [Header("현재 플레이어가 위치한 섹터 좌표"), SerializeField]
    private Vector2Int _currPlayerPos;
    public Vector2Int CurrPlayerPos => _currPlayerPos;

    [Header("플레이어 좌표 체크 주기"), SerializeField]
    private float _checkingInterval;

    [Header("플레이어")]    private GameObject _player;

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

        if(_player == null) _player = GameObject.FindGameObjectWithTag("Player");
        _currPlayerPos = new Vector2Int(0, 0);

        if(_sectorSpawner == null)  _sectorSpawner = GetComponent<SectorSpawn>();

        _sectorSpawner.SpawnStartSector(_sectorMap);
        _sectorSpawner.SpawnNextSector(_currPlayerPos, _sectorMap);

        _currSectorDeleteProtocol = _sectorMap[_currPlayerPos].GetComponent<DeleteProtocol>();

        _currStayDuration = 0;
        
        // 좌표 확인 주기는 0.1f초 마다 수행
        if(_checkingInterval < 0.1f)  _checkingInterval = 0.1f;

        StartCoroutine(Co_CheckPlayerSectorPosition());
    }

    private void Update()
    {
        // 프로토콜 시간 갱신
        _currStayDuration += Time.deltaTime;
        _currSectorDeleteProtocol.ProtocolUpdate(_currStayDuration);

        // TODO: 여기서 나중에 GetPurgeDamagePercentage() 를 통해
        // 플레이어에게 입힐 퍼센트 데미지 비례 양을 계산 후 플레이어 HP에 반영
    }

    private IEnumerator Co_CheckPlayerSectorPosition()
    {
        WaitForSeconds delay = new WaitForSeconds(_checkingInterval);
        while(true)
        {
            if(_player == null) break;

            int px = Mathf.RoundToInt(_player.transform.position.x / SectorData.WIDTH);
            int py = Mathf.RoundToInt(_player.transform.position.y / SectorData.HEIGHT);

            _currPlayerPos = new Vector2Int(px, py);

            _currSectorDeleteProtocol = _sectorMap[_currPlayerPos].GetComponent<DeleteProtocol>();

            _currStayDuration = _currSectorDeleteProtocol.StayDuration;

            yield return delay;
        }
    }

    [ContextMenu("테스트/섹터 생성")]
    private void Test_SpawnSector() => _sectorSpawner.SpawnNextSector(_currPlayerPos, _sectorMap);
}