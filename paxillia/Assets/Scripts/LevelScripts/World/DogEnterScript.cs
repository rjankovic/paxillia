using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnterScript : MonoBehaviour //: SaveableWorldObject
{
    [SerializeField]
    private Sprite completedSprite;
    // Start is called before the first frame update

    private new SpriteRenderer renderer;

    private bool levelCompleted = false;
    
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        //EventHub.Instance.OnDogLevelCompleted += OnDogLevelCompleted;

        if (GameManager.Instance.DogLevelCompleted)
        {
            OnDogLevelCompleted();
        }
    }

    private void OnDogLevelCompleted()
    {
        Debug.Log("Dog level completed");
        renderer.sprite = completedSprite;
        transform.localScale = new Vector3(0.75f, 0.75f, 1);
        levelCompleted = true;
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

        if (levelCompleted)
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
