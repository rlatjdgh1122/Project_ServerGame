using UnityEngine.BindingSystem;
using UnityEngine;

public class TestCompo : MonoBehaviour, ISetBindingTarget
{
    [Binding("coin")] private DataBinding<int> coin;
    [Binding("name")] private DataBinding<string> playerName;

    private void Awake()
    {
        coin.Value = 1; // ó���� coin ���� 1�� ����
    }

    private void Start()
    {
        coin.Value = 3; // coin ���� 3���� ����
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
              coin.Value += 2;
        }
    }
}