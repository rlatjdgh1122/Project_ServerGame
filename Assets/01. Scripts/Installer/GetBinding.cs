using UnityEngine.BindingSystem;
using UnityEngine;

public class GetBinding : MonoBehaviour, IGetBindingTarget
{
    [GetBinding("coin", typeof(int))]
    public void GetData(int coin)
    {
        Debug.Log("GetData : " + coin);
    }

    [GetBinding("name", typeof(string))]
    public void GetName(string name)
    {
        Debug.Log("GetName : " + name);
    }
}