using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField]
    private EnemyList _enemyList;

    [Header("현재 적 스폰량"), SerializeField]
    private int _spawnEnemyAmount;
    public int SpawnEnemyAmount => _spawnEnemyAmount;

    private void OnEnable()
    {
        SectorSpawn.OnSectorSpawned += HandleSectorSpawned;
    }

    private void OnDisable()
    {
        SectorSpawn.OnSectorSpawned -= HandleSectorSpawned;
    }

    private void HandleSectorSpawned(SectorData sd)
    {
        EnemySpawnPoint spawnPoints = sd.GetComponent<EnemySpawnPoint>();
        if(spawnPoints == null) return;

        SpawnEnemy(sd, spawnPoints.SpawnPoint, _spawnEnemyAmount);
    }

    void Awake()
    {
        if(Instance == null)    Instance = this;
        else                    Destroy(gameObject);

        if(_enemyList == null)   _enemyList = GetComponent<EnemyList>();
    }

    /// <summary>
    /// 적 스폰 포인트 리스트 중 스폰량만큼 적을 생성함
    /// </summary>
    /// <param name="spawnPointList">적 스폰 포인트 리스트</param>
    /// <param name="amount">적 스폰량</param>
    public void SpawnEnemy(SectorData sd, Transform[] spawnPointList, int amount)
    {
        if (spawnPointList == null || spawnPointList.Length == 0) return;
        if (_enemyList == null || _enemyList.MeleeEnemy == null || _enemyList.MeleeEnemy.Length == 0) return;

        // null/미할당 요소를 제외하고 유효한 Transform만 수집
        List<Transform> validPoints = new List<Transform>();
        for (int i = 0; i < spawnPointList.Length; i++)
        {
            if (spawnPointList[i] != null)
            {
                validPoints.Add(spawnPointList[i]);
            }
        }

        if (validPoints.Count == 0) return;

        // [0, 유효 적 스폰 포인트 수] 만큼만 생성하게 함
        amount = Mathf.Clamp(amount, 0, validPoints.Count);

        int tmpLength = validPoints.Count;

        for(int i = 0; i < amount; ++i, --tmpLength)
        {
            int randIndex = Random.Range(0, tmpLength);

            // test : 우선 근접 적만 출현하게 함
            GameObject enemy = _enemyList.MeleeEnemy[0];
            if (enemy == null) continue;

            GameObject spawn = Instantiate(enemy, validPoints[randIndex].position,
                               Quaternion.identity, sd.GetComponent<Transform>());

            // 맵 크기를 (3, 3, 1) 생성해서 스폰된 적이 찌그러지는 문제 발생
            // TODO: 맵 크기를 (1, 1, 1)로 수정하거나, 카메라 크기를 줄이거나 해야 할 듯
            Vector3 parentScale = sd.transform.localScale;
            Vector3 originalScale = enemy.transform.localScale;

            spawn.transform.localScale = new Vector3(
                originalScale.x / parentScale.x,
                originalScale.y / parentScale.y,
                originalScale.z / parentScale.z
            );


            // 생성된 적은 SectorData에 저장
            sd.AddEnemy(spawn); 

            Transform last = validPoints[tmpLength - 1];
            validPoints[randIndex] = last;
        }
    }

    /// <summary>
    /// 적 스폰량 업데이트
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateEnemySpawnAmount(int amount) => _spawnEnemyAmount = amount;
}
