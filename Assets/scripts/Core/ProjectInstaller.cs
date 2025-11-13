using UnityEngine;
using Reflex.Core;
using Systems.SceneManagement;

namespace Core
{
    // Dependency Injection
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(new SceneGroupManager(), typeof(ISceneGroupManager));
        }
    }
}