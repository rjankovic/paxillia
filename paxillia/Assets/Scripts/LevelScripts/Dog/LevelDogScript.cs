using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelDogScript : MonoBehaviour
{
    public SpriteShapeController crackableWallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //var wall = GameObject.Instantiate<SpriteShapeController>(crackableWallPrefab, transform);
        //wall.splineDetail = 32;

        //var s = wall.spline;
        //var pointCount = s.GetPointCount();
        //for (int i = 0; i < pointCount; i++)
        //{
        //    Debug.Log("-----------------");
        //    Debug.Log($"Point {i}");
        //    var pointLeft = s.GetLeftTangent(i);
        //    var pointRight = s.GetRightTangent(i);
        //    Debug.Log($"L: {pointLeft}");
        //    Debug.Log($"R: {pointRight}");

        //}

        //s.InsertPointAt(3, new Vector3(3f, 3f, 0));



        //s.InsertPointAt(s.GetPointCount(), new Vector3(0f, 0f, 0));
        //s.InsertPointAt(s.GetPointCount(), new Vector3(3f, 0f, 0));
        //s.InsertPointAt(s.GetPointCount(), new Vector3(3f, 3f, 0));


        //for (int i = 0; i < 4; i++)
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
