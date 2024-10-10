using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class MapConfiner : ExpansionMonoBehaviour, IMapConfiner
{
	[SerializeField] private PolygonCollider2D _coll = null;

	public Collider2D GetConfiner()
	{
		return _coll;
	}
}
