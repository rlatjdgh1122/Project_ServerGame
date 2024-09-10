using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathPrefab : ExpansionMonoBehaviour, ISetupHandler
{
    private ISpriteRendererHandler _sr = null;

    public void Setup(ComponentList list)
    {
        _sr = GetComponent<ISpriteRendererHandler>();
    }

    public void SetAlpha(float alpha)
    {
        _sr.SetAlpha(alpha);
    }

}
