using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererHelper : ExpansionMonoBehaviour, ILineRenderer2DHandler
{
    [SerializeField] private Transform _startTrm = null;
    private LineRenderer _lineRenderer = null;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        InitializeLineRenderer();
    }

    private void InitializeLineRenderer()
    {
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, _startTrm.position);
    }


    public void SetTargetPosition(Vector3 position)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, _startTrm.position);
        _lineRenderer.SetPosition(1, position);
    }

    public void Clear()
    {
        _lineRenderer.positionCount = 1;
    }

    private void OnDestroy()
    {
        _lineRenderer.positionCount = 0;
    }
}
