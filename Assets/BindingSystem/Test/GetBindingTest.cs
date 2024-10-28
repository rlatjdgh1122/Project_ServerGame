using UnityEngine;
using TMPro;

namespace Seongho.BindingSystem
{
    public class GetBindingTest : MonoBehaviour, IGetBindingTarget
    {
        public TextMeshProUGUI CoinText = null;
        public TextMeshProUGUI CoinText2 = null;
        public TextMeshProUGUI TestId = null;
        public TextMeshProUGUI TestName = null;

        [GetBinding("coin", typeof(int))]
        public void GetData(int coin)
        {
            CoinText.text = $"coin : {coin.ToString()}";
        }

        [GetBinding("name", typeof(string))]
        public void GetData(string coin)
        {
            CoinText2.text = $"coin : {coin.ToString()}";
        }

        [GetBinding("test", typeof(TestClass))]
        public void GetData(TestClass test)
        {
            TestId.text = test.Id.ToString();
            TestName.text = test.Name.ToString();
        }
    }
}
