using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [Header("근접 적 프리팹 리스트"), SerializeField]
    private GameObject[] _meleeEnemy;

    [Header("드론 적 프리팹 리스트"), SerializeField]
    private GameObject[] _droneEnemy;
}
