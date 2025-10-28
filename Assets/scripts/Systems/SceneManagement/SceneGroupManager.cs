using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Systems.Helpers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.SceneManagement
{
    public class SceneGroupManager
    {
        public event Action<string> OnSceneLoaded = delegate { }; //durch delegate spar ich mir das invoke!!!
        public event Action<string> OnSceneUnloaded = delegate { };
        public event Action OnSceneGroupLoaded = delegate { };

        private SceneGroup ActiveSceneGroup;

        public async Task LoadScenes(SceneGroup sceneGroup, IProgress<float> progress, bool reloadDupScenes = false)
        {
            ActiveSceneGroup = sceneGroup;
            var loadedScenes = new List<string>();

            await UnloadScenes();
            
            int sceneCount = SceneManager.sceneCount;

            for (var i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }
            
            var totalScenesToLoad = ActiveSceneGroup.Scenes.Count;
            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (var i = 0; i < totalScenesToLoad; i++)
            {
                var sceneData = sceneGroup.Scenes[i];
                if(reloadDupScenes == false && loadedScenes.Contains(sceneData.Name)) continue;
                
                var operation = SceneManager.LoadSceneAsync(sceneData.Name, LoadSceneMode.Additive);
                
                operationGroup.Operations.Add(operation);
                
                OnSceneLoaded.Invoke(sceneData.Name);
            }
            //report progress until all Scenes are loaded (zb.: loading screen)
            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }

            Scene activeScene = SceneManager.GetSceneByName(ActiveSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene); //delay to avoid tight loop
            }
            
            OnSceneGroupLoaded.Invoke();
            
            //simple cleanup
            await Resources.UnloadUnusedAssets();
        }
        
        public async Task UnloadScenes()
        {
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;
            
            int sceneCount = SceneManager.sceneCount;

            for (var i = 0; i < sceneCount; i++)
            {
                var sceneData = SceneManager.GetSceneAt(i);
                if(!sceneData.isLoaded) continue;
                if(sceneData.name.Equals(activeScene) || sceneData.name == "Bootstrapper") continue;
                scenes.Add(sceneData.name);
            }

            var operationGroup = new AsyncOperationGroup(100);
            
            foreach (var scene in scenes)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);
                if(operation == null) continue;
                
                operationGroup.Operations.Add(operation);
                
                OnSceneUnloaded.Invoke(scene);
            }
            
            while (!operationGroup.IsDone)
            {
                await Task.Delay(100); //delay to avoid tight loop
            }
            
            
        }
    }
}