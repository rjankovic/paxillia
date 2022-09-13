using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLook(InputValue inputValue)
    {
        //var deltaVector = inputValue.Get<Vector2>();
        var absoluteVector = Mouse.current.position.ReadValue();
        var screenV3 = new Vector3(absoluteVector.x, absoluteVector.y, 0);
        var worldPosition = Camera.main.ScreenToWorldPoint(screenV3);

        var deltaPosition = new Vector2(worldPosition.x - transform.position.x, 0);

        RaycastHit2D[] result = new RaycastHit2D[10];

        var cf = new ContactFilter2D();
        cf.layerMask = new LayerMask();
        cf.layerMask.value = 6;
        var cast = Physics2D.BoxCast(new Vector2((worldPosition.x + transform.position.x) / 2, transform.position.y), new Vector2(Mathf.Abs(worldPosition.x - transform.position.x) + transform.localScale.x, transform.localScale.y), 0, new Vector2(0,0) /*deltaPosition*/, 
            cf, result);
        if (cast > 0)
        {

            for (int i = 0; i < cast; i++)
            {
                // wall on the right
                if (result[i].point.x >= transform.position.x)
                {
                    Debug.Log("Block right");
                    deltaPosition.x = (result[i].collider.gameObject.transform.position.x - result[i].collider.gameObject.transform.localScale.x / 2 - transform.localScale.x / 2) - transform.position.x - 0.01f;
                }
                Debug.Log("Collision: " + result[i].collider.gameObject.name);
            }
            //return;
            
        }
        transform.Translate(deltaPosition);

        
            /*
             *  if (Physics.BoxCast(new Vector3(0, 5, -1), new Vector3(0.5f, 0.5f, 0.5f), Vector3.forward, out hitInfo, Quaternion.identity, 1.0f
 {
       print(hitInfo.collider.name);
 }
 else
 {
       print("Nothing hit");
 }
             */

        //Debug.Log("Mouse position: " + worldPosition);

        //var targetPositionX = transform.position.x + deltaVector.x / 30.0f;
        //if (targetPositionX > 8 - transform.lossyScale.x / 2)
        //{
        //    targetPositionX = 8f - transform.lossyScale.x / 2;
        //}
        //else if (targetPositionX < -8f + transform.lossyScale.x / 2)
        //{
        //    targetPositionX = -8f + transform.lossyScale.x / 2;
        //}

        ////Debug.Log($"OnLook [{deltaVector.x},{deltaVector.y}]");
        //transform.Translate(new Vector2(targetPositionX - transform.position.x, 0));
    }
}
