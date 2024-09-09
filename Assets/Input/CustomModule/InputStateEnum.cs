namespace Seongho.InputSystem
{
  

    // UI 입력 키 해시값 (메모리 절약을 위해 byte로 정의)
    public enum INPUT_TYPE : byte
    {
        Player = 0,       // Player Action으로 변경
        UI,               // UI Action으로 변경
    }

}