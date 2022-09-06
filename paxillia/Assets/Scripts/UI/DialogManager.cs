using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogEventArgs : EventArgs
{ 
    public Dialog Dialog { get; set; }
}

//public delegate void DialogEventHandler(object sender, DialogEventArgs args);

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _currentText;

    private Dialog _dialog;
    private Queue<Message> _messages;
    private Message _message;

    //[SerializeField]
    //private Animator _animator;
    
    
    //public event DialogEventHandler OnDialogStart;
    //public event DialogEventHandler OnDialogEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialog(Dialog dialog)
    {
        _dialog = dialog;
        _messages = new Queue<Message>();
        foreach (var message in dialog.Messages)
        {
            _messages.Enqueue(message);
        }

        //_animator.SetBool("DialogOpen", true);

        EventHub.Instance.DialogOpen(_dialog);
        //if (OnDialogStart != null)
        //{
        //    OnDialogStart(this, new DialogEventArgs() { Dialog = _dialog });
        //}

        DisplayNextMessage();
    }

    public void DisplayNextMessage()
    {
        if (_messages.Count == 0)
        {
            EndDialog();
        }
        else
        {
            _message = _messages.Dequeue();
            _currentText.text = _message.Text;
        }
    }

    private void EndDialog()
    {
        //_animator.SetBool("DialogOpen", false);


        EventHub.Instance.DialogClose(_dialog);
        //if (OnDialogEnd != null)
        //{
        //    OnDialogEnd(this, new DialogEventArgs() { Dialog = _dialog });
        //}
        _dialog = null;
    }

    public void OnSubmit(InputValue value)
    {
        Debug.Log("Dialog manager submit");
        if (_dialog != null)
        {
            DisplayNextMessage();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
