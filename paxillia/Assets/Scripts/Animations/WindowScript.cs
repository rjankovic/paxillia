using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowScript : MonoBehaviour
{
    [SerializeField]
    private Sprite brokenSprite;

    [SerializeField]
    private AudioSource brokenSound;

    private new SpriteRenderer renderer;

    private bool _broken = false;

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
        if (_broken)
        {
            return;
        }
        _broken = true;
        Debug.Log("Window broken by " + collision.gameObject.tag);
        renderer.sprite = brokenSprite;
        brokenSound.Play();
        EventHub.Instance.WindowBroken();
    }
}
