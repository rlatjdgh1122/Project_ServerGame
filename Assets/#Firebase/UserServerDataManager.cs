using Newtonsoft.Json;
using ShardData;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class UserServerDataManager : RestSingleton<UserServerDataManager>
{
    public UserServerDataManager() : base("UserData") { }

    /// <summary>
    /// 계정이 생성될 때 한번 실행
    /// </summary>
    public async Task CreateUserServerDataWithServerAsync()
    {
        string url = GetURL("CreateUserData", new FromData() { Name = "uid", Data = AuthManager.Instance.UId });

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                // JSON 응답을 string으로 읽기
                string json = await response.Content.ReadAsStringAsync();

                //고유 넘버인 PlayerId를 서버에서 받아옴
                //PlayerId = JsonConvert.DeserializeObject<ulong>(json);

                //Debug.Log($"UserData 생성 성공 : {PlayerId}");
                
                Debug.Log("데이터 만들기 성공");
            }

            else
            {
                Debug.LogError("UserData 생성 실패: " + response.StatusCode);

            } //end else

        } //end using
    }

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
