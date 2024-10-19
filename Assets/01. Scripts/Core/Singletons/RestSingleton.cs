using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestSingleton<T> : Singleton<T> where T : class, new()
{

    private readonly string BaseURL;

    public struct FromData
    {
        public string Name;
        public object Value;
    }

    public RestSingleton()
    {
        Debug.Log("QWer");
    }


    public RestSingleton(string controllerName)
    {
        Debug.Log("controllerName");

        BaseURL = $"https://localhost:7012/api/{controllerName}";
    }

    public string GetURL(string method, FromData data = default)
    {
        if (!data.Equals(default))
        {
            return $"{BaseURL}/{method}?{data.Name}={data.Value.ToString()}";

        } //end return

        return $"{BaseURL}/{method}";
    }
}
