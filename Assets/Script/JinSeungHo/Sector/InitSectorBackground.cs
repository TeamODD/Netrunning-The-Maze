using UnityEngine;

public class InitSectorBackground : MonoBehaviour
{
    [Header("초기화할 캔버스"), SerializeField]
    private Canvas _canvas;

    void Start()
    {
        if(_canvas == null) _canvas = GetComponent<Canvas>();

        if(_canvas != null)
        {
            _canvas.sortingOrder = -10;
        }
    }

}
