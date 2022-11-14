using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class CrackableWallScript : MonoBehaviour
{
    private SpriteShapeController controller;
    private Spline s;
    //private Vector2 position = new Vector2 (-8f, -5.5f);
    private Vector2 position = new Vector2(-7f, -5.5f);

    private  int topHeight = 6;
    private  int bottomHeight = 2;
    //private  int sideWidth = 7;
    private int sideWidth = 6;
    private  int midWidth = 2;

    public class WallColumn
    {
        public float LeftX;
        public float RightX;
        public float Height;
        public int LeftPointIndex;
        public int RightPointIndex;
        public int ColumnNumber;
    }

    public static List<WallColumn> Columns = new List<WallColumn>();

    // Start is called before the first frame update
    void Start()
    {
        transform.position = position;
        controller = GetComponent<SpriteShapeController>();
        s = controller.spline;

        // base
        s.InsertPointAt(s.GetPointCount(), new Vector3(0, 0f, 0));
        s.InsertPointAt(s.GetPointCount(), new Vector3(sideWidth * 2 + midWidth, 0f, 0));

        int pointCount = 2;
        int columnsLeft = sideWidth * 2 + midWidth;

        // right to left
        for (int i = sideWidth * 2 + midWidth; i > sideWidth + midWidth; i--)
        {
            s.InsertPointAt(s.GetPointCount(), new Vector3(i, topHeight, 0));
            s.InsertPointAt(s.GetPointCount(), new Vector3((float)i - (i > sideWidth + midWidth + 1 ? 0.5f : 1f), topHeight, 0));

            Columns.Add(new WallColumn()
            {
                LeftX = position.x + i - 1,
                RightX = position.x + i,
                Height = topHeight,
                LeftPointIndex = pointCount + 1,
                RightPointIndex = pointCount,
                ColumnNumber = columnsLeft--
            });

            var c = Columns[Columns.Count - 1];
            Debug.Log($"{c.ColumnNumber} L {c.LeftX} R {c.RightX}");

            pointCount += 2;
        }

        for (int i = sideWidth + midWidth; i > sideWidth; i--)
        {
            s.InsertPointAt(s.GetPointCount(), new Vector3(i, bottomHeight, 0));
            s.InsertPointAt(s.GetPointCount(), new Vector3((float)i - (i > sideWidth + 1 ? 0.5f : 1f), bottomHeight, 0));

            Columns.Add(new WallColumn()
            {
                LeftX = position.x + i - 1,
                RightX = position.x + i,
                Height = bottomHeight,
                LeftPointIndex = pointCount + 1,
                RightPointIndex = pointCount,
                ColumnNumber = columnsLeft--
            });

            var c = Columns[Columns.Count - 1];
            Debug.Log($"{c.ColumnNumber} L {c.LeftX} R {c.RightX}");

            pointCount += 2;
        }

        for (int i = sideWidth; i > 0; i--)
        {
            s.InsertPointAt(s.GetPointCount(), new Vector3(i, topHeight, 0));
            s.InsertPointAt(s.GetPointCount(), new Vector3((float)i - (i > 1 ? 0.5f : 1f), topHeight, 0));

            Columns.Add(new WallColumn()
            {
                LeftX = position.x + i - 1,
                RightX = position.x + i,
                Height = topHeight,
                LeftPointIndex = pointCount + 1,
                RightPointIndex = pointCount,
                ColumnNumber = columnsLeft--
            });

            var c = Columns[Columns.Count - 1];
            Debug.Log($"{c.ColumnNumber} L {c.LeftX} R {c.RightX}");

            pointCount += 2;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != Constants.TAG_BALL)
        {
            return;
        }


        var contactPoint = collision.contacts[0].point;
        Debug.Log($"C {contactPoint}");
        var column = Columns.First(x => x.LeftX <= contactPoint.x && x.RightX >= contactPoint.x);
        Debug.Log($"Column {column.ColumnNumber} H {column.Height} L {column.LeftX} R {column.RightX}");

        // do not crack if hitting from the side
        if (column.Height + position.y - contactPoint.y > 0.1)
        {
            return;
        }

        WallColumn columnToLeft = Columns.FirstOrDefault(x => x.ColumnNumber == column.ColumnNumber - 1);
        bool midWay = false;
        if (columnToLeft != null)
        {
            // columns that weren't at the same height will now be - the the left point needs to be mid way
            // (left column was lower)
            if (System.Math.Abs(columnToLeft.Height + 1 - column.Height) < 0.1)
            {
                midWay = true;
            }
        }

        WallColumn columnToRight = Columns.FirstOrDefault(x => x.ColumnNumber == column.ColumnNumber + 1);
        bool shiftRightColumn = false;
        bool unshiftRightColumn = false;
        if (columnToRight != null)
        {
            // columns that were at the same height will now not be - the the left point to the right needs to be all the way
            if (System.Math.Abs(columnToRight.Height - column.Height) < 0.1)
            {
                shiftRightColumn = true;
            }
            // columns that were not at the same height will now be - the the left point to the right needs to be half way
            if (System.Math.Abs(columnToRight.Height + 1 - column.Height) < 0.1)
            {
                unshiftRightColumn = true;
            }
        }

        if (column.Height > bottomHeight)
        {
            column.Height--;
            s.RemovePointAt(column.LeftPointIndex);
            s.RemovePointAt(column.RightPointIndex);
            if (shiftRightColumn)
            {
                s.RemovePointAt(columnToRight.LeftPointIndex);
                s.InsertPointAt(columnToRight.LeftPointIndex, new Vector3(columnToRight.RightX - 1 - position.x, columnToRight.Height, 0));
            }
            if (unshiftRightColumn)
            {
                s.RemovePointAt(columnToRight.LeftPointIndex);
                s.InsertPointAt(columnToRight.LeftPointIndex, new Vector3(columnToRight.RightX - 0.5f - position.x, columnToRight.Height, 0));
            }
            s.InsertPointAt(column.RightPointIndex, new Vector3(column.RightX - position.x, column.Height, 0));
            s.InsertPointAt(column.LeftPointIndex, new Vector3((midWay ? (column.LeftX + 0.5f) : column.LeftX) - position.x, column.Height, 0));
            //if (shiftRightColumn)
            //{
            //    s.InsertPointAt(columnToRight.LeftPointIndex, new Vector3(columnToRight.RightX - 1 - position.x, columnToRight.Height, 0));
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
