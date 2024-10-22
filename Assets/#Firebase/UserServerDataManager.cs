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
    /// ������ ������ �� �ѹ� ����
    /// </summary>
    public async Task CreateUserServerDataWithServerAsync()
    {
        string url = GetURL("CreateUserData", new FromData() { Name = "uid", Data = AuthManager.Instance.UId });

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                // JSON ������ string���� �б�
                string json = await response.Content.ReadAsStringAsync();

                //���� �ѹ��� PlayerId�� �������� �޾ƿ�
                //PlayerId = JsonConvert.DeserializeObject<ulong>(json);

                //Debug.Log($"UserData ���� ���� : {PlayerId}");
                
                Debug.Log("������ ����� ����");
            }

            else
            {
                Debug.LogError("UserData ���� ����: " + response.StatusCode);

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

                Debug.Log("������ �������� ����");
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
                Debug.Log("������ ���� ����");

            } //end if

            else
            {
                Debug.LogError("Token verification failed");

            } //end else

        } //end using
    }


}