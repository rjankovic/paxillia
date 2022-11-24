using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnterScript : MonoBehaviour
{
    [SerializeField]
    private Sprite completedSprite;
    
    private new SpriteRenderer renderer;

    private bool levelCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        
        if (GameManager.Instance.TreeLevelCompleted)
        {
            OnTreeLevelCompleted();
        }
    }

    private void OnTreeLevelCompleted()
    {
        Debug.Log("Tree level completed");
        renderer.sprite = completedSprite;
        //transform.localScale = new Vector3(0.75f, 0.75f, 1);
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

        Debug.Log("Entering tree level");
        GameManager.Instance.BallCount++;

        GameManager.Instance.WorldReturnPosition = GameManager.Instance.Ball.GetComponent<Rigidbody2D>().position;

        GameManager.Instance.GotoLevel(GameManager.LevelEnum.AppleTree);
    }
}
