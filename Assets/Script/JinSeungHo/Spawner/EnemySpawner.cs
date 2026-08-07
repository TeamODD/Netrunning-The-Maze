using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField]
    private EnemyList _enemyList;

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
    public void SpawnEnemy(Transform[] spawnPointList, int amount)
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
            Instantiate(enemy, tmpPoints[randIndex].position, Quaternion.identity);

            Transform last = tmpPoints[tmpLength - 1];
            tmpPoints[randIndex] = last;
        }
    }
}
