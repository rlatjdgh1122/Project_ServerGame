using ExtensionMethod.Object;
using System.Reflection;
using UnityEngine;

namespace Seongho.BindingSystem
{
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

                //만약 해당 Atrribute를 사용했다면
                if (bindingAttribute != null)
                {
                    //데이터를 가져온 후
                    var data = field.GetValue(component);

                    //고유키를 가져온 후
                    int uniqueKey = data.Cast<IGetUniqeKey>().UniqeKey;

                    //캐싱해준다.
                    DataBindingManager.Instance.RegisterDataBinding(bindingAttribute.Name, uniqueKey, data);

                } //end if
            } //end foreach
        }
    }
}
