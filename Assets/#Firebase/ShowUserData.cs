using ShardData;
using TMPro;
using UnityEngine;
public class ShowUserData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText = null;
    [SerializeField] private TextMeshProUGUI _userName = null;
    [SerializeField] private TextMeshProUGUI _id = null;

    public async void OnGetUserData()
    {
        UserServerData data = await AuthManager.Instance.GetUserServerDataWithServerAsync();

        _coinText.text = data.Coin.ToString();
        _userName.text = data.UserName.ToString();
        _id.text = data.PlayerId.ToString();
    }

}
