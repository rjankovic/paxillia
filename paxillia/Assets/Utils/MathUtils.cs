using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Utils
{
    internal static class MathUtils
    {
        public static Vector2 RotateVector(Vector2 vector, float radians)
        {
            var newX = vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians);
            var newY = vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians);
            return new Vector2(newX, newY);
        }

        /// <summary>
        /// trying to move from position to targetPosition, returns the delta position if there is no obstacle, else returns the delta to move to position ajacent to the closest obstacle where we can move
        /// </summary>
        /// <param name="position">starting position</param>
        /// <param name="targetPosition">moving target</param>
        /// <param name="size">object size</param>
        /// <param name="wallLayer">wall object layer mask</param>
        /// <param name="wallTag">the wall object tag</param>
        /// <returns></returns>
        public static Vector2 TryMove(Vector3 position, Vector3 targetPosition, Vector3 size, float movementSpeed = 0f, int wallLayer = 6, string wallTag = "Wall")
        {
            Physics2D.defaultContactOffset = 0;
            /**/
            //var worldPosition = Camera.main.ScreenToWorldPoint(screenV3);

            var deltaPosition = new Vector2(targetPosition.x - position.x, targetPosition.y - position.y);
            if (movementSpeed > 0 && MathF.Abs(deltaPosition.x) > movementSpeed)
            {
                deltaPosition.x = Math.Sign(deltaPosition.x) * movementSpeed;
            }
            if (movementSpeed > 0 && MathF.Abs(deltaPosition.y) > movementSpeed)
            {
                deltaPosition.y = Math.Sign(deltaPosition.y) * movementSpeed;
            }
            targetPosition.x = position.x + deltaPosition.x;
            targetPosition.y = position.y + deltaPosition.y;

            //Debug.Log($"DX {deltaPosition.x}");

            RaycastHit2D[] result = new RaycastHit2D[10];

            // find any collisions that can occur when moving from transform.position to worldPosition on the X axis - check the rectangle between the original and new position (including)
            var collisionRectCenter = new Vector2((targetPosition.x + position.x) / 2, (targetPosition.y + position.y) / 2);
            // the size is [deltaX + paddle width; paddle height]
            var collisionRectangleSize = new Vector2(Mathf.Abs(targetPosition.x - position.x) + size.x, Mathf.Abs(targetPosition.y - position.y) + size.y);

            // filter the collisions to layer 6 - Walls
            var cf = new ContactFilter2D();
            cf.layerMask = new LayerMask();
            cf.layerMask.value = wallLayer;

            // returns the number of boxcast hits
            var cast = Physics2D.BoxCast(collisionRectCenter, collisionRectangleSize, 0, new Vector2(0, 0), cf, result);
            if (cast > 0)
            {

                for (int i = 0; i < cast; i++)
                {
                    if (result[i].collider.tag != wallTag)
                    {
                        continue;
                    }

                    // wall on the right and we're moving to the right
                    if (result[i].point.x >= position.x && deltaPosition.x > 0)
                    {
                        //Debug.Log($"Block right - tX {targetPosition.x} dX {deltaPosition.x} CPX {result[i].point.x} COL {result[i].collider.gameObject.name}");

                        // the target position is left from the obstackle by half of obstckle's width + half of paddle's width

                        //Debug.Log($"CPX: {result[i].point.x}");
                        deltaPosition.x = 0;
                        
                        //var targetPositionX =
                        //    result[i].collider.gameObject.transform.position.x - result[i].collider.gameObject.transform.localScale.x / 2
                        //    //result[i].point.x 
                        //    - size.x / 2;
                        //var candidatePosition = targetPositionX - position.x;
                        //if (candidatePosition < deltaPosition.x)
                        //{
                        //    deltaPosition.x = candidatePosition;
                        //}
                    }


                    // wall on the left and moving to the left
                    else if (result[i].point.x <= position.x && deltaPosition.x < 0)
                    {
                        //Debug.Log($"Block left - tX {targetPosition.x} dX {deltaPosition.x} CPX {result[i].point.x} COL {result[i].collider.gameObject.name}");

                        deltaPosition.x = 0;


                        //Debug.Log($"Left coll: P [{result[i].point.x}], OBJ [{result[i].collider.gameObject.transform.position.x}]");
                        //var targetPositionX =
                        //    //result[i].collider.gameObject.transform.position.x + result[i].collider.gameObject.transform.localScale.x / 2
                        //    result[i].point.x
                        //    + size.x / 2;
                        //var candidatePosition = targetPositionX - position.x;
                        //if (candidatePosition > deltaPosition.x)
                        //{
                        //    Debug.Log($"Constrain to {candidatePosition}");
                        //    deltaPosition.x = candidatePosition;
                        //}
                        //else
                        //{
                        //    Debug.Log($"Free to {deltaPosition.x}");
                        //}
                    }


                    // wall down and moving down
                    if (result[i].point.y >= position.y && deltaPosition.y > 0)
                    {
                        deltaPosition.y = 0;
                    }
                    // wall up and moving up
                    else if (result[i].point.y <= position.y && deltaPosition.y < 0)
                    {
                        deltaPosition.y = 0;
                    }

                    //Debug.Log("Collision: " + result[i].collider.gameObject.name);
                }


            }
            return deltaPosition;
            //transform.Translate(deltaPosition);
            /**/
        }

        public static Vector2 RestrictToMovementSpeed(Vector2 delta, float movementSpeed)
        {
            if (delta.magnitude < movementSpeed)
            {
                return delta;
            }
            else
            {
                return delta.normalized * movementSpeed;
            }

            //var deltaPosition = delta.normalized * movementSpeed;

            ////var deltaPosition = new Vector2(delta.x, delta.y);

            //if (movementSpeed > 0 && MathF.Abs(deltaPosition.x) > movementSpeed)
            //{
            //    deltaPosition.x = Math.Sign(deltaPosition.x) * movementSpeed;
            //}
            //if (movementSpeed > 0 && MathF.Abs(deltaPosition.y) > movementSpeed)
            //{
            //    deltaPosition.y = Math.Sign(deltaPosition.y) * movementSpeed;
            //}

            //return deltaPosition;
        }

        //public static Vector2 TryMoveHorizontally(Vector3 position, Vector3 targetPosition, Vector3 size, float movementSpeed = 0f, int wallLayer = 6, string wallTag = "Wall")
        //{
        //    targetPosition.y = position.y;
        //    return TryMove(position, targetPosition, size, movementSpeed, wallLayer, wallTag);
        //}
    }

}
