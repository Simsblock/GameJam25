using System;

namespace Systems.SceneManagement
{
    public class SceneGroupManager
    {
        public event Action<string> OnSceneLoaded = delegate { }; //durch delegate spar ich mir das invoke!!!
        public event Action<string> OnSceneUnloaded = delegate { };
        public event Action OnSceneGroupLoaded = delegate { };

        private SceneGroup ActiveSceneGroup;
    }
}