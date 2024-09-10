using ExtensionMethod.List;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovePath : ExpansionMonoBehaviour, IDrawMovePathHandler
{
    public DrawPathPrefab DrawObject = null;               // �׸� ������
    public int DrawCount = 10;                         // �׸� �������� ����
    public float Distance = 1.0f;                      // �� ������ ������ �Ÿ�

    private List<DrawPathPrefab> _list = new();

    /// <summary>
    /// ��θ� �׷��ݴϴ�.
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
