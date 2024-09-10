using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrawMovePathHandler
{
    public void OnDraw(Vector2 velocity);
    public void Clear();
}
