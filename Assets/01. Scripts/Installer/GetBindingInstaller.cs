using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.BindingSystem;

public interface IGetBindingTarget { }

[DefaultExecutionOrder(-150)]
public class GetBindingInstaller : MonoBehaviour
{
    void Awake()
    {
        var GetBindings = GetComponents<IGetBindingTarget>();
        foreach (var com2 in GetBindings)
        {
            RegisterGetBindings(com2);
        }
    }
    private void RegisterGetBindings(IGetBindingTarget com2)
    {
        var methods = com2.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            var bindingAttribute = method.GetCustomAttribute<GetBindingAttribute>();

            if (bindingAttribute != null)
            {
                DataBindingManager.Instance.AAA(bindingAttribute.Name,
                    value => method.Invoke(com2, new[] { value }));
            }
        }
    }
}
