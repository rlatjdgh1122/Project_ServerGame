using UnityEngine.BindingSystem;
using UnityEngine;

public class TestCompo : MonoBehaviour, ISetBindingTarget
{
    [Binding("coin")] private DataBinding<int> coin = new(3);
    [Binding("name")] private DataBinding<string> playerName = new("null");

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            coin.Value *= 2;

        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            playerName.Value = $"{coin.Value}";

        }
    }
}