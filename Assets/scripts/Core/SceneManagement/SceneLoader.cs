using System.Threading.Tasks;
using Reflex.Attributes;
using Reflex.Core;
using Systems.Helpers;
using UnityEngine;

namespace Systems.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] SceneGroup[] sceneGroups;
        [Inject] public readonly ISceneGroupManager manager;

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