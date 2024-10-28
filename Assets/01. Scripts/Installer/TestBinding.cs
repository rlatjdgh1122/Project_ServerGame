using UnityEngine.BindingSystem;
using UnityEngine;
using System;

public class TestBinding : MonoBehaviour, ISetBindingTarget
{
    [Binding("coin")] private DataBinding<int> coin = new(3);
    [Binding("name")] private DataBinding<string> playerName = new("");
    [Binding("test")] private DataBinding<TestClass> testClass = new(new(id: 3, name: "¼ºÈ£"));

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            coin.Value *= 2;

        } //end if

        if (Input.GetKeyUp(KeyCode.V))
        {
            playerName.Value = $"{coin.Value}";
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            var prevData = testClass.Value;
            testClass.Value = new TestClass(prevData.Id * 2, $"Seongho");
        }
    }
}