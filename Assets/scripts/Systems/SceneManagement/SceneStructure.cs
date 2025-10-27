using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Eflatun.SceneReference;

namespace Systems.SceneManagement
{
    [Serializable]
    public class SceneData
    {
        public SceneReference Reference;
        public string Name => Reference.Name;
        public SceneType SceneType;
    }

    [Serializable]
    public class SceneGroup
    {
        public string GroupName = "New Scene Group";
        public List<SceneData> Scenes;

        public string FindSceneNameByType(SceneType sceneType)
        {
            return Scenes.FirstOrDefault(scene => scene.SceneType == sceneType)?.Name;
        }
    }

    public enum SceneType
    {
        ActiveScene = 0,
        HUD = 1,
        Menu = 2
    }
}