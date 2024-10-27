using System.Reflection;
using UnityEngine.BindingSystem;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using ExtensionMethod.Object;

public interface ISetBindingTarget { }

[DefaultExecutionOrder(-100)]
public class BindingInstaller : MonoBehaviour
{
    private void Awake()
    {
        var setBindings = GetComponents<ISetBindingTarget>();
      
        foreach (var com1 in setBindings)
        {
            RegisterSetBindings(com1);
        }
    }

    private void RegisterSetBindings(ISetBindingTarget component)
    {
        // 필드에 BindingAttribute가 있는지 확인
        var fields = component.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        foreach (var field in fields)
        {
            BindingAttribute bindingAttribute = field.GetCustomAttribute<BindingAttribute>();

            if (bindingAttribute != null)
            {
                var fieldValue = field.GetValue(component);
                DataBindingManager.Instance.RegisterBinding(bindingAttribute.Name, fieldValue);

            } //end if
        } //end foreach
    }
}