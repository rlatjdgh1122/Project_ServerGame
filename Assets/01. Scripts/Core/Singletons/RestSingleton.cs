using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestSingleton<T> : Singleton<T> where T : class, new()
{

    private readonly string BaseURL;

    public struct FromData
    {
        public string Name;
        public object Data;
    }

    public RestSingleton() { }

    public RestSingleton(string controllerName)
    {
        BaseURL = $"https://localhost:7012/api/{controllerName}";
    }

    public string GetURL(string method, FromData? data = null)
    {
        if (data.HasValue)
        {
            return $"{BaseURL}/{method}?{data.Value.Name}={data.Value.Data.ToString()}";

        } //end return

        return $"{BaseURL}/{method}";
    }
}
