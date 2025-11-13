using System;
using System.Threading.Tasks;

namespace Systems.SceneManagement;

public interface ISceneGroupManager
{
    public event Action<string> OnSceneLoaded;
    public event Action<string> OnSceneUnloaded;
    public event Action OnSceneGroupLoaded;
    
    public Task LoadScenes(SceneGroup sceneGroup, IProgress<float> progress, bool reloadDupScenes = false);
    public Task UnloadScenes();
}