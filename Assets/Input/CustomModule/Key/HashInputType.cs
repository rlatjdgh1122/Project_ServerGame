namespace Seongho.InputSystem
{
    // 플레이어 입력 키 해시값 (메모리 절약을 위해 byte로 정의)
    public enum HASH_INPUT_PLAYER : byte
    {
        LeftClick = 0,    // 왼쪽 클릭
        Space             // 스페이스바
    }

    // UI 입력 키 해시값 (메모리 절약을 위해 byte로 정의)
    public enum HASH_INPUT_UI : byte
    {
        LeftClick = 0,    // 왼쪽 클릭
    }
}