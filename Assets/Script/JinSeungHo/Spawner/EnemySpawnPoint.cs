using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("적 스폰 포인트 리스트"), SerializeField]
    private Transform[] _spawnPoint;
    public Transform[] SpawnPoint => _spawnPoint;
}
