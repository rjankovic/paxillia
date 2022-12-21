using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    [Serializable]
    public class SaveState
    {
        public bool DogLevelCompleted ;

        public bool TreeLevelCompleted ;

        public bool RoadblockRemoved ;
        
        public string Level ;
        public float PositionX ;
        public float PositionY ;

        public float BallPositionX ;
        public float BallPositionY ;
        public float BallVelocityX ;
        public float BallVelocityY ;

        public float WorldReturnPositionX ;
        public float WorldReturnPositionY ;

        public bool BallInGame ;

        public int BallCount ;
        public string SavedWorldItemsString = string.Empty;
        public List<GameObjectSaveState> SavedWorldItems = new List<GameObjectSaveState>() ;
        public void ItemsFromString()
        {
            var pts = SavedWorldItemsString.Split("|");
            SavedWorldItems = new List<GameObjectSaveState>();
            foreach (var pt in pts)
            {
                var spl = pt.Split(";");
                if (spl[0] == "COL")
                {
                    SavedWorldItems.Add(new CollectibleGameObjectSaveSate() { ObjectName = spl[1] });
                }
                else {
                    SavedWorldItems.Add(new CompletionGameObjectSaveSate() { ObjectName = spl[1] });
                }
            }
        }

        public void ItemsToString()
        {
            List<string> pts = new List<string>();
            foreach (var it in SavedWorldItems)
            {
                pts.Add((it is CollectibleGameObjectSaveSate ? "COL" : "COM") + ";" + it.ObjectName);
            }
            SavedWorldItemsString = String.Join("|", pts);
        }

    }

    [Serializable]
    public abstract class GameObjectSaveState
    {
        public string ObjectName;

        
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
