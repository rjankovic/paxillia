using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCollectible : SaveableWorldObject //: MonoBehaviour
{
    public override void ApplySaveState(GameObjectSaveState saveState)
    {
        if (saveState.ObjectName == gameObject.name)
        {
            Destroy(gameObject);
        }
    }

    public override GameObjectSaveState GetSaveState()
    {
        return new CollectibleGameObjectSaveSate() { ObjectName = gameObject.name };
    }

    public void SetCollected()
    {
        UpdateItemState();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

