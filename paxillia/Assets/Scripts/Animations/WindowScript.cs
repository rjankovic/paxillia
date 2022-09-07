using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowScript : MonoBehaviour
{
    [SerializeField]
    private Sprite brokenSprite;

    private new SpriteRenderer renderer;

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
        //Collider2D[] colliders = new Collider2D[10];
        if (collision.gameObject.tag != "Ball")
        {
            return;
        }
        Debug.Log("Window broken by " + collision.gameObject.tag);
        renderer.sprite = brokenSprite;
        EventHub.Instance.WindowBroken();
    }
}
