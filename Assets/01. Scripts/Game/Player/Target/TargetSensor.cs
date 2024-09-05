using Define;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : ExpansionMonoBehaviour, ITargetSensor
{
    [SerializeField] private Transform _detectBox = null;
    [SerializeField] private Collider2D _myCollder = null;
    [SerializeField] private LayerMask _targetLayerMask;

    private float saveDistance = 1000f;

    public IGrabTargetHandler FindTarget()
    {
        saveDistance = 1000f;

        IGrabTargetHandler target = null;
        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, (Vector2)_detectBox.localScale, 0, _targetLayerMask);

        foreach (Collider2D coll in colls)
        {
            if (coll.Equals(_myCollder)) continue;

            if (coll.TryGetComponent(out IGrabTargetHandler result))
            {
                float dictance = Vector3.Distance(transform.position, coll.transform.position);
                if (saveDistance > dictance)
                {
                    saveDistance = dictance;

                    target = result;
                }

            } //end if

        } //end foreach


        return target;
    }

    public float GetDistance()
    {
        return saveDistance;
    }

    public void OnDrawGizmosSelected()
    {
        if (_detectBox)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, (Vector2)_detectBox.localScale);
        }

    }


}
