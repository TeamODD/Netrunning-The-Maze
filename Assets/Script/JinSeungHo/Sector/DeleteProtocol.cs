using UnityEngine;

public class DeleteProtocol : MonoBehaviour
{
    [Header("삭제 프로토콜 단계"), SerializeField]
    private DeleteProtocolState _state;
    public DeleteProtocolState State => _state;

    [Header("섹터 삭제 프로토콜 타이머"), SerializeField]
    private float _stayDuration;
    public float StayDuration => _stayDuration;

    void Awake()
    {
        _state = DeleteProtocolState.Normal;
        _stayDuration = 0;
    }

    /// <summary>
    /// 삭제 프로토콜에 따른 데미지 증가율 반환
    /// </summary>
    /// <returns>데미지 증가율(1% => 0.01)</returns>
    public float GetPurgeDamagePercentage()
    {
        if(_state != DeleteProtocolState.Active)                        return 0;
        else if(_stayDuration < (float)DeleteProtocolState.Active + 30) return 0.02f;
        else if(_stayDuration < (float)DeleteProtocolState.Active + 60) return 0.04f;
        else                                                            return 0.08f;
    }

    /// <summary>
    /// 인자만큼 _stayDuration 갱신 후 State 변경
    /// </summary>
    /// <param name="duration"></param>
    public void ProtocolUpdate(float duration)
    {
        _stayDuration = duration;

        if(_stayDuration < (float)DeleteProtocolState.Alert)
            _state = DeleteProtocolState.Normal;
        else if(_stayDuration < (float)DeleteProtocolState.Ready)
            _state = DeleteProtocolState.Alert;
        else if(_stayDuration < (float)DeleteProtocolState.FinalAlert)
            _state = DeleteProtocolState.Ready;
        else if(_stayDuration < (float)DeleteProtocolState.Active)
            _state = DeleteProtocolState.FinalAlert;
        else
            _state = DeleteProtocolState.Active;
    }
}