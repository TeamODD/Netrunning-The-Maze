using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 프리팹 목록")]
    [SerializeField] private EnemyList _enemyList;

    [Header("소환된 적 컨테이너")]
    [SerializeField] private Transform _container;

    [Header("이 섹터 기본 적 스폰량")]
    [SerializeField] private int _spawnEnemyAmount = 6;
    public int SpawnEnemyAmount => _spawnEnemyAmount;

    [Header("스폰 시 발판 양 끝 안전 마진")]
    [SerializeField] private float _edgeMargin = 1f;

    [Header("발판 윗면 오프셋")]
    [SerializeField] private float _spawnYOffset = 0.8f;

    private SectorData _sectorData;

    private void Awake()
    {
        _sectorData = GetComponent<SectorData>();

        if(_enemyList == null)  _enemyList = GetComponent<EnemyList>();
    }

    public void SpawnEnemies(IReadOnlyList<GameObject> platforms)
    {
        if(platforms == null || platforms.Count == 0)   return;
        if(_enemyList == null || _enemyList.MeleeEnemy == null || _enemyList.MeleeEnemy.Length == 0)   return;

        // 플랫폼들을 Y 좌표 기준으로 묶고 오름차순 정렬
        List<List<GameObject>> tierGroups = GroupPlatformsByY(platforms);
        if(tierGroups.Count == 0)   return;

        int spawnedCount = 0;
        int tierIndex = 0;
        int maxAttempts = _spawnEnemyAmount * 5;
        int currentAttempt = 0;

        while (spawnedCount < _spawnEnemyAmount && currentAttempt < maxAttempts)
        {
            currentAttempt++;

            List<GameObject> currentTier = tierGroups[tierIndex % tierGroups.Count];
            tierIndex++;
            
            if (currentTier.Count == 0) continue;
            
            // 해당 층의 발판 중 랜덤 선택
            GameObject platform = currentTier[Random.Range(0, currentTier.Count)];
            if (platform == null) continue;
            
            Collider2D col = platform.GetComponent<Collider2D>();
            if (col == null) continue;
            
            Bounds bounds = col.bounds;
            
            // 발판 좌우 끝에서 떨어지지 않게 20% 마진 확보
            float margin = Mathf.Min(_edgeMargin, bounds.size.x * 0.2f);
            float spawnX = Random.Range(bounds.min.x + margin, bounds.max.x - margin);

            // 적의 스프라이트 높이
            GameObject enemyPrefab = _enemyList.MeleeEnemy[0];
            if (enemyPrefab == null) break;
            
            SpriteRenderer enemySr = enemyPrefab.GetComponent<SpriteRenderer>();
            float enemyHalfHeight = (enemySr != null && enemySr.sprite != null) 
                ? (enemySr.sprite.rect.height / enemySr.sprite.pixelsPerUnit) / 2f 
                : 0.5f;
            
            // 발판의 윗면(bounds.max.y) + 적 절반 높이 = 발판 위에 안착
            float spawnY = bounds.max.y + enemyHalfHeight;
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

            // 부모 없이 월드에 1배 정상 크기로 먼저 소환
            GameObject spawn = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            
            // worldPositionStays: true 로 컨테이너에 넣으면,
            // Unity가 부모의 RectTransform과 3배 스케일을 알아서 역산해 크기/위치를 완벽 고정
            if (_container != null)
            {
                spawn.transform.SetParent(_container, true);
            }
            else if (_sectorData != null)
            {
                spawn.transform.SetParent(_sectorData.transform, true);
            }
            if (_sectorData != null)
            {
                _sectorData.AddEnemy(spawn);
            }
            spawnedCount++;
        }
    }


    /// <summary>
    /// 플랫폼 목록을 Y 좌표 기준으로 묶음
    /// </summary>
    private List<List<GameObject>> GroupPlatformsByY(IReadOnlyList<GameObject> platforms)
    {
        var groups = new List<List<GameObject>>();
        float threshold = 1.5f;

        foreach(var p in platforms)
        {
            if(p == null) continue;
            float py = p.transform.position.y;

            bool added = false;
            foreach(var g in groups)
            {
                if(Mathf.Abs(g[0].transform.position.y - py) < threshold)
                {
                    g.Add(p);
                    added = true;
                    break;
                }
            }

            if(!added)
            {
                groups.Add(new List<GameObject> { p });
            }
        }

        // 아래층 -> 위층 순으로 정렬
        groups.Sort((a, b) => a[0].transform.position.y.CompareTo(b[0].transform.position.y));
        return groups;
    }
}
