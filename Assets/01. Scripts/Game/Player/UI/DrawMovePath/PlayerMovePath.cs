using ExtensionMethod.List;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovePath : ExpansionMonoBehaviour, IDrawMovePathHandler
{
    public DrawPathPrefab DrawObject = null;               // 그릴 프리팹
    public int DrawCount = 10;                         // 그릴 프리팹의 개수
    public float Distance = 1.0f;                      // 각 프리팹 사이의 거리

    private List<DrawPathPrefab> _list = new();

    /// <summary>
    /// 경로를 그려줍니다.
    /// </summary>
    public void OnDraw(Vector2 velocity)
    {
        Clear();

        Vector2 currentPosition = transform.position;
        Vector2 gravity = Physics2D.gravity;

        for (int i = 0; i < DrawCount; i++)
        {
            float time = i * (Distance / velocity.magnitude);
            Vector2 displacement = velocity * time + 0.5f * gravity * time * time;
            Vector2 predictedPosition = currentPosition + displacement;

            DrawPathPrefab spawnedPrefab = Instantiate(DrawObject, predictedPosition, Quaternion.identity);
            spawnedPrefab.SetAlpha(1f / i);
            _list.Add(spawnedPrefab);
        }
    }

    public void Clear()
    {
        _list.TryClear(p => Destroy(p.gameObject));
    }

}
