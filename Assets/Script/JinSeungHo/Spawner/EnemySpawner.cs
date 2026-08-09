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

    void Start()
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
        // [0, 적 스폰 포인트 리스트 수] 만큼만 생성하게 함
        amount = Mathf.Clamp(amount, 0, spawnPointList.Length);

        List<Transform> tmpPoints = new List<Transform>(spawnPointList);
        int tmpLength = tmpPoints.Count;

        for(int i = 0; i < amount; ++i, --tmpLength)
        {
            int randIndex = Random.Range(0, tmpLength);

            // test : 우선 근접 적만 출현하게 함
            GameObject enemy = _enemyList.MeleeEnemy[0];
            GameObject spawn = Instantiate(enemy, tmpPoints[randIndex].position,
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

            Transform last = tmpPoints[tmpLength - 1];
            tmpPoints[randIndex] = last;
        }
    }

    /// <summary>
    /// 적 스폰량 업데이트
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateEnemySpawnAmount(int amount) => _spawnEnemyAmount = amount;
}
