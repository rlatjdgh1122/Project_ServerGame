using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : ExpansionMonoBehaviour
{

    [SerializeField] private Transform _detectBox = null;

    public void OnDrawGizmosSelected()
    {
        if (_detectBox)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, (Vector2)_detectBox.localScale);
        }
 
    }
}
