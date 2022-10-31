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
        public string Level { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
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

    public abstract class SaveableWorldObject
    {
        public abstract GameObjectSaveState GetSaveState();

        public abstract void ApplySaveState(GameObjectSaveState saveState);

        protected void UpdateItemState()
        {
            EventHub.Instance.WordlSaveStateUpdate(GetSaveState());
        }
    }
}
