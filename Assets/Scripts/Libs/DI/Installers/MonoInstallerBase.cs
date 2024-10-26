using DevLab.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DevLab.DI
{
    [DefaultExecutionOrder(-100)]
    public abstract class MonoInstallerBase : MonoBehaviour
    {

        protected DIContainer _container;

        private void Awake()
        {
            _container = new();
            InstallBinding();
            Install();
        }

        protected abstract void InstallBinding();

        private void Install()
        {

            Stack<GameObject> objs = new Stack<GameObject>();
            objs.Push(gameObject);

            while(objs.Count > 0)
            {

                var obj = objs.Pop();

                if (obj != gameObject && obj.TryGetComponent<MonoInstallerBase>(out var _))
                    continue;

                var compos = GetComponents<Component>();

                foreach (var compo in compos)
                {
                    SetFields(compo);
                    SetMethods(compo);
                    SetPropertys(compo);
                }

                var childs = obj.GetChilds();
                foreach (var o in childs)
                    objs.Push(o.gameObject);

            }
        }

        private void SetPropertys(Component compo)
        {
            var propertys = compo.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<Inject>() != null);

            foreach (var property in propertys)
                property.SetValue(compo, FindInstnace(property.PropertyType));
        }

        private void SetMethods(Component compo)
        {
            var methods = compo.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<Inject>() != null);

            foreach(var method in methods)
            {
                var @params = method.GetParameters();
                object[] objs = new object[@params.Length];

                for(int i = 0; i < @params.Length; i++)
                    objs[i] = FindInstnace(@params[i].ParameterType);

                method.Invoke(compo, objs);
            }

        }

        private void SetFields(Component compo)
        {
            var fields = compo.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<Inject>() != null);
            foreach (var field in fields)
            {
                field.SetValue(compo, FindInstnace(field.FieldType));
            }
        }

        private object FindInstnace(Type t)
        {
            var o = _container.Get(t);
            if(o == null)
                o = GetComponent(t);
            return o;
        }

    }
}