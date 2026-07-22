using UnityEngine;

public enum SectorType
{
    StartSector,        // 시작 섹터
    NormalSector,       // 일반 섹터
    AdminSector,        // 관리자 섹터
    StableSector        // 안정화 섹터
}

public class SectorData : ScriptableObject
{
    [SerializeField, Tooltip("섹터 타입")]      private SectorType _sectorType;

    [SerializeField, Tooltip("섹터 프리팹")]    private GameObject _sectorPrefab;
}