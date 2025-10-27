using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Networking;

public static class AssetsLoader
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
            Debug.Log(fullPath);
            Debug.LogError("Failed to read file: " + www.error);
        }
    }
        public static IEnumerator LoadTexture(string relativePath, Action<Texture2D> callback)
        {
            
            string fullPath= Path.Combine(Application.streamingAssetsPath, relativePath);
            UnityWebRequest www = UnityWebRequest.Get(fullPath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                byte[] bytes = File.ReadAllBytes(fullPath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                callback?.Invoke(texture);
            }
            else
            {
                Debug.Log(fullPath);
                Debug.LogError("Failed to read file: " + www.error);
            }
        }
        public static List<Sprite> SliceTexture(Texture2D texture, int columns, int rows)
        {
            List<Sprite> sprites = new List<Sprite>();
            int spriteWidth = texture.width / columns;
            int spriteHeight = texture.height / rows;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Rect rect = new Rect(x * spriteWidth, y * spriteHeight, spriteWidth, spriteHeight);
                    Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100);
                    sprites.Add(sprite);
                }
            }
            return sprites;
        }
    
}

