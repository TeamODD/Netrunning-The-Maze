using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    /// <summary>
    /// 현재 변경된 게임 상태(State)를 알림
    /// </summary>
    public static event Action<GameState> OnGameStateChanged;

    [SerializeField, Header("현재 게임 상태")]
    private GameState _gameState = GameState.RunStart;
    public GameState CurrentState => _gameState;

    public void Awake()
    {
        if(Instance == null)    Instance = this;
        else                    Destroy(gameObject);
    }

    public void ChangeState(GameState state)
    {
        if(_gameState == state) return;
        
        _gameState = state;

        HandleStateTransition();

        OnGameStateChanged?.Invoke(_gameState);
    }

    private void HandleStateTransition()
    {
        if(_gameState == GameState.Paused)          Time.timeScale = 0f;        // 게임 정지
        else                                        Time.timeScale = 1f;        // 게임 제개
        
        if(_gameState == GameState.PlayerDead)
        {
            // TODO: 사망처리?
        }
    }
}