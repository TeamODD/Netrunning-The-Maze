using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [Serializable]
    public struct TierData
    {
        public string tierName;     // 현재 플랫폼이 배치된 층 수
        public float yOffset;       // 섹터 중심(0) 기준 y 좌표
        public int minPlatforms;    // 이 층에 배치될 최소 발판 수
        public int maxPlatforms;    // 이 증에 배치될 최대 발판 수
    }

    [Header("플랫폼 프리팹")]
    [SerializeField] private GameObject _platformPrefab;

    [Header("Tier 1 ~ Tier 6 층별 설정")]
    [SerializeField] private TierData[] _tiers;

    [Header("플랫폼 가로 크기 종류")]
    [SerializeField] private float[] _platformWidths = { 4f, 8f, 12f };

    [Header("벽과 플랫폼 간격")]
    [SerializeField] private float _platformPadding = 4.0f;

    [Header("벽과 플랫폼 간격")]
    [SerializeField] private float _platformGap = 2.5f;

    /// <summary>
    /// 생성된 플랫폼 목록
    /// </summary>
    private List<GameObject> _spawnedPlatforms = new List<GameObject>();
    public IReadOnlyList<GameObject> SpawnedPlatforms => _spawnedPlatforms;

    private void Start()
    {
        GeneratePlatforms();
    }

    [ContextMenu("플랫폼 생성")]
    public void GeneratePlatforms()
    {
        // 발판 배치 전 기존 발판 삭제
        ClearPlatforms();

        // 섹터 너비에 비례하는 x 가용 범위
        float halfWidth = SectorData.WIDTH / 2f;
        float xMin = -halfWidth + _platformPadding;
        float xMax = halfWidth - _platformPadding;

        foreach(TierData td in _tiers)
        {
            // 생성할 플랫폼 수
            int count = UnityEngine.Random.
                                Range(td.minPlatforms, td.maxPlatforms + 1);
            if(count <= 0)  continue;

            // 겹치지 않고 + 편향되지 않게 (x, width) 세트 생성
            List<(float x, float  width)> platformPlacements = 
                                          CalcTierPlacment(count, xMin, xMax);

            // 플랫폼 배치
            foreach(var placement in platformPlacements)
            {
                SpawnPlatform(placement.x, td.yOffset, placement.width);
            }
        }
    }

    /// <summary>
    /// 플랫폼이 배치될 좌표 계산
    /// </summary>
    /// <param name="count">배치할 플랫폼 수</param>
    private List<(float x, float y)> CalcTierPlacment(int count, float xMin, float xMax)
    {
        var result =new List<(float x, float y)>();
        float totalWidth = xMax - xMin;
        float zoneWidth = totalWidth / count;

        for(int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.
                        Range(0, _platformWidths.Length);
            float width = _platformWidths[index];
            float halfPlaform = width / 2f;

            float zoneStart = xMin  + (i * zoneWidth);
            float zoneEnd = zoneStart + zoneWidth;

            float safeMin = zoneStart + halfPlaform + (_platformGap / 2f);
            float safeMax = zoneEnd - halfPlaform - (_platformGap / 2f);

            float posX;
            if(safeMin < safeMax)
            {
                // 구역 내에서 랜덤 배치
                posX = UnityEngine.Random.Range(safeMin, safeMax);
            }
            else
            {
                // 발판이 너무 커서 구역 여유가 없는 경우
                // 구역 중앙에 고정
                posX = (zoneStart + zoneEnd) / 2f;
                posX = Mathf.Clamp(posX, xMin + halfPlaform, xMax - halfPlaform);
            }

            result.Add((posX, width));
        }

        return result;
    }

    private void SpawnPlatform(float x, float y, float width)
    {
        Vector3 spawnPos = transform.position + new Vector3(x, y, 0);
        GameObject platform = Instantiate(_platformPrefab, spawnPos, Quaternion.identity, transform);

        // width에 맞게 플랫폼 크기 조절
        SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();
        if(sr != null)  sr.size = new Vector2(width, sr.size.y);

        // BoxCollider2D 크기 동기화
        BoxCollider2D col = platform.GetComponent<BoxCollider2D>();
        if(col != null) col.size = new Vector2(width, col.size.y);

        _spawnedPlatforms.Add(platform);
    }

    private void ClearPlatforms()
    {
        foreach (var p in _spawnedPlatforms)
        {
            if (p != null) Destroy(p);
        }
        _spawnedPlatforms.Clear();
    }
}