using UnityEngine.BindingSystem;
using UnityEngine;

public class TestCompo : MonoBehaviour, ISetBindingTarget
{
    [Binding("coin")] private DataBinding<int> coin;
    [Binding("name")] private DataBinding<string> playerName;

    private void Awake()
    {
        coin.Value = 1; // 처음에 coin 값을 1로 설정
    }

    private void Start()
    {
        coin.Value = 3; // coin 값을 3으로 변경
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
              coin.Value += 2;
        }
    }
}