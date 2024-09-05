using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    public enum INPUT_KEY_STATE : sbyte
    {
        NOT_PRESSED = -128,
        DOWN,
        PRESSING,
        UP,
    }

    public enum HASH_INPUT_PLAYER : sbyte
    {
        LeftClick = -128,
        Space
    }

    public enum HASH_INPUT_UI : sbyte
    {
        LeftClick = -128,
    }

    public static class Core
    {

    }
}
