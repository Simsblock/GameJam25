using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Networking;
using System.Linq;

public static class JsonReader
{
    private static CoroutineHelper CoroutineHelper;
    private static string SPCJsonPath;
    private static string TransverJson;

    static JsonReader()
    {
        SPCJsonPath = Path.Combine(Application.streamingAssetsPath, "data.json");

        // Find or create CoroutineHelper instance
        GameObject helperObj = new GameObject("CoroutineHelper");
        CoroutineHelper = helperObj.AddComponent<CoroutineHelper>();
        UnityEngine.Object.DontDestroyOnLoad(helperObj);
    }

    // Coroutine method using a callback
    public static IEnumerable GetSPCEffect(string name, Action<EffectDto> callback)
    {
        yield return CoroutineHelper.StartCoroutine(ReadFileFromStreamingAssets(name));
        List<EffectDto> effects = JsonConvert.DeserializeObject<List<EffectDto>>(TransverJson);

        // Find the effect by name
        EffectDto result = effects.FirstOrDefault(e => e.name == name);

        // Call the callback with the result
        callback?.Invoke(result);
    }

    private static IEnumerator ReadFileFromStreamingAssets(string name)
    {
        UnityWebRequest www = UnityWebRequest.Get(SPCJsonPath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            TransverJson = www.downloadHandler.text;
            
        }
        else
        {
            Debug.LogError("Failed to read file: " + www.error);
        }
    }
}

//only used to start a coroutine
public class CoroutineHelper : MonoBehaviour
{
}
