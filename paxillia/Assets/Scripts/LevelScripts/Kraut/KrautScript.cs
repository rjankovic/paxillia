using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrautScript : MonoBehaviour
{
    [SerializeField]
    private Sprite damageSprite1;
    [SerializeField]
    private Sprite damageSprite2;
    [SerializeField]
    private Sprite damageSprite3;
    private new SpriteRenderer renderer;

    private int lives = 3;
    

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Ball")
        {
            return;
        }
        if (lives < 1)
        {
            return;
        }
        if (lives == 3)
        {
            renderer.sprite = damageSprite1;
        }
        if (lives == 2)
        {
            renderer.sprite = damageSprite2;
        }
        if (lives == 1)
        {
            renderer.sprite = damageSprite3;
        }
        lives--;

        if (lives == 0)
        {
            EventHub.Instance.KrautDestroyed();
        }
    }
}
