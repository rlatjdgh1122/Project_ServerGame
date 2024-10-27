using UnityEngine.BindingSystem;
using UnityEngine;

public class GetBinding : MonoBehaviour, IGetBindingTarget
{
    [GetBinding("coin")]
    public void GetData(int coin)
    {
        Debug.Log("GetData : " + coin); //ó�� : 1, ���� : 3
    }
}