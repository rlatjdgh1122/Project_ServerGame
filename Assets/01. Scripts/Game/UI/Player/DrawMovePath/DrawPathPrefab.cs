using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathPrefab : ExpansionMonoBehaviour, ISetupHandler
{
    private ISpriteRenderer2DHandler _sr = null;

    public void Setup(ComponentList list)
    {
        _sr = GetComponent<ISpriteRenderer2DHandler>();
    }

    public void SetAlpha(float alpha)
    {
        _sr.SetAlpha(alpha);
    }

}
