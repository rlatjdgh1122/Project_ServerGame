using Newtonsoft.Json;
using ShardData;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class UserServerDataManager : RestSingleton<UserServerDataManager>
{
    public UserServerDataManager() : base("UserData") { }

    public async Task<UserServerData> GetUserServerDataWithServerAsync()
    {
        string url = GetURL("GetUserDataByPlayerId", new FromData { Name = "uid", Data = AuthManager.Instance.UId });

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                UserServerData data = JsonConvert.DeserializeObject<UserServerData>(json);

                Debug.Log("데이터 가져오기 성공");
                return data;

            } //end if

            else
            {
                Debug.LogError("Token verification failed");
                return default;

            } //end else

        } //end using
    }

    public async Task UpdateUserServerDataWithServerAsync(UserServerData data)
    {
        string url = GetURL("UpdateUserData", new FromData { Name = "uid", Data = AuthManager.Instance.UId });

        using (HttpClient client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                Debug.Log("데이터 수정 성공");

            } //end if

            else
            {
                Debug.LogError("Token verification failed");

            } //end else

        } //end using
    }


}
