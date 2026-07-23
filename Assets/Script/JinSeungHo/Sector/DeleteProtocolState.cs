public enum DeleteProtocolState
{
    Normal = 0,         // 정상 상태
    Alert = 150,        // 경고: 시스템 감지
    Ready = 180,        // 삭제 프로토콜 준비, 배경 노이즈 증가
    FinalAlert = 200,   // 강한 경고음, 출구 방향 표시
    Active = 210,       // 삭제 프로토콜 활성화
}