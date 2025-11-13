using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Systems.Helpers
{
    public readonly struct AsyncOperationGroup
    {
        public readonly List<UnityEngine.AsyncOperation> Operations;
        
        public float Progress => Operations.Count == 0 ? 0 : Operations.Average(operation => operation.progress);
        public bool IsDone => Operations.All(operation => operation.isDone);
        
        public AsyncOperationGroup(int initialCapacity)
        {
            Operations = new List<UnityEngine.AsyncOperation>(initialCapacity);
        }
    }
}