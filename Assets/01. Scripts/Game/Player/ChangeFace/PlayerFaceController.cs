using ExtensionMethod.Dictionary;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerFaceController : ExpansionMonoBehaviour, ISetupHandler, IPlayerStopHandler, IGameFlowHandler, ILocalActionHandler<FaceType>
{
    public event Action<FaceType> OnActionEvent = null;

    public string SkinName = "";

    private Dictionary<FaceType, Sprite> _typeToSpriteDic = new();
    private ISpriteRenderer2DHandler _sr = null;

    private IPlayerStopController _stopController = null;

    public void Setup(ComponentList list)
    {
        _sr = list.Find<ISpriteRenderer2DHandler>();
        _stopController = list.Find<IPlayerStopController>();
    }

    void IGameFlowHandler.OnGameInit()
    {

        List<Sprite> spriteList = ResourceManager.Instance.GetAssetsByLabelName<Sprite>(SkinName);

        foreach (Sprite sprite in spriteList)
        {
            if (Enum.TryParse(sprite.name, out FaceType type))
            {
                _typeToSpriteDic.TryAdd(type, sprite);
            }

        }//end foreach

        _stopController.OnStop();
    }

    public void OnPlayerStart()
    {
        OnActionEvent?.Invoke(FaceType.Default);
    }

    public void OnPlayerStop()
    {
        OnActionEvent?.Invoke(FaceType.Stop);
    }

    public void DoAction(FaceType type)
    {
        _sr.SetSprite(_typeToSpriteDic.GetValue(type));
    }
}
