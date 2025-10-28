using System.Threading.Tasks;
using Systems.Helpers;
using UnityEngine;

namespace Systems.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] SceneGroup[] sceneGroups;
        public readonly SceneGroupManager manager = new SceneGroupManager();

        async void Start()
        {
            await LoadSceneGroup(0);
        }

        public async Task LoadSceneGroup(int index)
        {
            if (index < 0 || index >= sceneGroups.Length)
            {
                Debug.LogError($"Invalid scene group index {index}");
                return;
            }
            LoadingProgress progress = new LoadingProgress();
            await manager.LoadScenes(sceneGroups[index], progress);
        }
    }
}