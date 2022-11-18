using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SaveState
    {
        public bool DogLevelCompleted { get; set; }

        public bool TreeLevelCompleted { get; set; }

        public bool RoadblockRemoved { get; set; }
        
        public string Level { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public float BallPositionX { get; set; }
        public float BallPositionY { get; set; }
        public float BallVelocityX { get; set; }
        public float BallVelocityY { get; set; }

        public float WorldReturnPositionX { get; set; }
        public float WorldReturnPositionY { get; set; }

        public bool BallInGame { get; set; }

        public int BallCount { get; set; }
        public List<GameObjectSaveState> SavedWorldItems { get; set; }

    }

    public abstract class GameObjectSaveState
    { 
        public string ObjectName { get; set; }

        
    }

    public class CollectibleGameObjectSaveSate : GameObjectSaveState
    {
    }

    public class CompletionGameObjectSaveSate : GameObjectSaveState
    {
    }

    public abstract class SaveableWorldObject : MonoBehaviour
    {
        public abstract GameObjectSaveState GetSaveState();

        public abstract void ApplySaveState(GameObjectSaveState saveState);

        protected void UpdateItemState()
        {
            EventHub.Instance.WordlSaveStateUpdate(GetSaveState());
        }
    }
}
