using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevLab.DI
{
    public class MonoInstaller : MonoInstallerBase
    {
        protected override void InstallBinding()
        {
            var compos = GetComponents<Component>();

            foreach(var compo in compos)
            {
                _container.Bind(compo.GetType(), compo);
            }
        }
    }
}
