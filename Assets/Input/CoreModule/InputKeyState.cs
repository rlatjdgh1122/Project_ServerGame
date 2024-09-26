namespace Seongho.InputSystem
{
    // 키 입력 상태를 나타내는 열거형 (메모리 절약을 위해 byte로 정의)
    public enum INPUT_KEY_STATE : byte
    {
        NOT_PRESSED = 0,  // 키가 눌리지 않음
        DOWN,             // 키가 눌림
        PRESSING,         // 키가 계속 눌려있는 상태
        UP,               // 키가 떼어짐
    }
}