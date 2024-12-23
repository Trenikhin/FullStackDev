using System.Collections.Generic;
using Modules.Entities;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ProductionOrder : MonoBehaviour
    {
        [SerializeField]
        private List<EntityConfig> _queue;
     
        [Variable]
        public IReadOnlyList<EntityConfig> Queue
        {
            get { return _queue; }
            set { _queue = new List<EntityConfig>(value); }
        }
    }
}