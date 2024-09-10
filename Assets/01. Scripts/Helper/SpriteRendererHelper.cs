using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererHelper : ExpansionMonoBehaviour, ISpriteRendererHandler
{
    private SpriteRenderer _sr = null;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void SetAlpha(float value)
    {
        Color color = new Color(_sr.color.a, _sr.color.g, _sr.color.b, value);
        _sr.color = color;
    }

}
