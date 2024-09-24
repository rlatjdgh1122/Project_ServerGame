using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private TurnType _myType = TurnType.None;

    public void SetMyType(TurnType type)
    {
        _myType = type;
    }

    public TurnType GetMyType()
    {
        return _myType;
    }

}
