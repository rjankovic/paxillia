using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnterScript : MonoBehaviour //: SaveableWorldObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != Constants.TAG_BALL)
        {
            return;
        }

        Debug.Log("Entering dog level");

        GameManager.Instance.WorldReturnPosition = GameManager.Instance.Ball.GetComponent<Rigidbody2D>().position;

        GameManager.Instance.GotoLevel(GameManager.LevelEnum.Dog);

        //UpdateItemState();
    }

    //public override GameObjectSaveState GetSaveState()
    //{
    //    return new CompletionGameObjectSaveSate() { ObjectName = gameObject.name };
    //}

    //public override void ApplySaveState(GameObjectSaveState saveState)
    //{
    //    throw new System.NotImplementedException();
    //}
}
