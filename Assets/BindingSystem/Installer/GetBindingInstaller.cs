using System.Reflection;
using UnityEngine;

namespace Seongho.BindingSystem
{
    public interface IGetBindingTarget { }

    [DefaultExecutionOrder(-100)]
    public class GetBindingInstaller : MonoBehaviour
    {
        void Awake()
        {
            //1. IGetBindingTarget를 상속받은 컴포넌트를 찾는다.
            var GetBindings = GetComponents<IGetBindingTarget>();

            foreach (var compo in GetBindings)
            {
                RegisterGetBindings(compo);

            } //end foreach
        }

        private void RegisterGetBindings(IGetBindingTarget compo)
        {
            var methods = compo.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            //1. 모든 메서드를 확인해본다.
            foreach (var method in methods)
            {
                var bindingAttribute = method.GetCustomAttribute<GetBindingAttribute>();

                //2. GetBinding을 사용한 메서드가 있는지 체크한다.
                if (bindingAttribute != null)
                {
                    //3. 메서드를 캐싱한다.
                    DataBindingManager.Instance.RegisterMethodBinding
                        (bindingAttribute.Name,
                        bindingAttribute.DataType,
                        value => method.Invoke(compo, new[] { value }));

                } //end if
            } //end foreach

        }
    }

}