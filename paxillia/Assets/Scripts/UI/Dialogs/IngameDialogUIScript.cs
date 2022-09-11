using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameDialogUIScript : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        EventHub.Instance.OnIngameDialogOpen += OnDialogStart;
        EventHub.Instance.OnIngameDialogClose += OnDialogEnd;
    }

    private void OnDialogStart(Dialog dialog)
    {
        Debug.Log("Dialog UI - dialog start");
        animator.SetBool("DialogOpen", true);
    }
    private void OnDialogEnd(Dialog dialog)
    {
        Debug.Log("Dialog UI - dialog end");
        animator.SetBool("DialogOpen", false);
    }


    //// Update is called once per frame
    //void Update()
    //{

    //}
}
