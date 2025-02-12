using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Networking;

public static class JsonReader
{
    public static IEnumerator ReadJsonTo<T>(string path, Action<T> callback)
    {
        string fullPath= Path.Combine(Application.streamingAssetsPath, path);
        UnityWebRequest www = UnityWebRequest.Get(fullPath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string s = www.downloadHandler.text;
            T result = JsonConvert.DeserializeObject<T>(s);
            callback?.Invoke(result);
        }
        else
        {
            Debug.LogError("Failed to read file: " + www.error);
        }
    }
}

