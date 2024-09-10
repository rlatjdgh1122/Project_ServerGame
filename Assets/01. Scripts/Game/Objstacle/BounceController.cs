using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : ExpansionMonoBehaviour
{
    public float Friction = 1f;
    public float Bounce = 1f;

    private Collider2D _cols = null;

    private void Awake()
    {
        _cols = GetComponent<Collider2D>();
        var mat = Instantiate(new PhysicsMaterial2D());

        mat.friction = Friction;
        mat.bounciness = Bounce;

        _cols.sharedMaterial = mat;
    }
}
