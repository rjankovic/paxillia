using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CrackableWallScript : MonoBehaviour
{
    private SpriteShapeController controller;
    private Spline s;
    private Vector2 position = new Vector2 (-8f, -5.5f);

    private  int topHeight = 6;
    private  int bottomHeight = 2;
    private  int sideWidth = 7;
    private  int midWidth = 2;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = position;
        controller = GetComponent<SpriteShapeController>();
        s = controller.spline;

        // base
        s.InsertPointAt(s.GetPointCount(), new Vector3(0, 0f, 0));
        s.InsertPointAt(s.GetPointCount(), new Vector3(sideWidth * 2 + midWidth, 0f, 0));

        // right to left
        for (int i = sideWidth * 2 + midWidth; i > sideWidth + midWidth; i--)
        {
            s.InsertPointAt(s.GetPointCount(), new Vector3(i, topHeight, 0));
            s.InsertPointAt(s.GetPointCount(), new Vector3((float)i - (i > sideWidth + midWidth + 1 ? 0.5f : 1f), topHeight, 0));
        }

        for (int i = sideWidth + midWidth; i > sideWidth; i--)
        {
            s.InsertPointAt(s.GetPointCount(), new Vector3(i, bottomHeight, 0));
            s.InsertPointAt(s.GetPointCount(), new Vector3((float)i - (i > sideWidth + 1 ? 0.5f : 1f), bottomHeight, 0));
        }

        for (int i = sideWidth; i > 0; i--)
        {
            s.InsertPointAt(s.GetPointCount(), new Vector3(i, topHeight, 0));
            s.InsertPointAt(s.GetPointCount(), new Vector3((float)i - (i > 1 ? 0.5f : 1f), topHeight, 0));
        }

        for (int i = 0; i < 3; i++)
        {
            s.RemovePointAt(0);
        }

        //s.InsertPointAt(s.GetPointCount(), new Vector3(3f, 3f, 0));


        //for (int i = 0; i < 3; i++)
        //{
        //    s.RemovePointAt(0);
        //}

        //s.InsertPointAt(s.GetPointCount(), new Vector3(2f, 3f, 0));
        //s.InsertPointAt(s.GetPointCount(), new Vector3(2f, 2f, 0));
        //s.InsertPointAt(s.GetPointCount(), new Vector3(1f, 2f, 0));
        //s.InsertPointAt(s.GetPointCount(), new Vector3(1f, 3f, 0));
        //s.InsertPointAt(s.GetPointCount(), new Vector3(0f, 3f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
