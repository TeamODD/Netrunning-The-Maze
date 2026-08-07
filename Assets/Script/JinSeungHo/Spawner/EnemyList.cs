using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [Header("근접 적 프리팹 리스트"), SerializeField]
    private GameObject[] _meleeEnemy;
    public GameObject[] MeleeEnemy;

    [Header("드론 적 프리팹 리스트"), SerializeField]
    private GameObject[] _droneEnemy;
    public GameObject[] DroneEnemy;
}
