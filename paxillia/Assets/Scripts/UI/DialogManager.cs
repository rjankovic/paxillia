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

    [SerializeField]
    private TextMeshProUGUI _currentIngameDialogCharacter;
    [SerializeField]
    private TextMeshProUGUI _currentIngameText;
    private Dialog _ingameDialog;
    private Queue<Message> _ingameMessages;
    private Message _ingameMessage;

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
        Debug.Log("Display next message");
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

    private IEnumerator DisplayIngameDialog()
    {
        while (_ingameMessages.Count > 0)
        { 
            _ingameMessage = _ingameMessages.Dequeue();
            _currentIngameText.text = _ingameMessage.Text;
            _currentIngameDialogCharacter.text = _ingameMessage.Character ?? string.Empty;
            yield return new WaitForSeconds(_ingameMessage.Duration);
        }
        EndIngameDialog();
    }

    private void EndDialog()
    {
        //_animator.SetBool("DialogOpen", false);

        EventHub.Instance.DialogClose(_dialog);
        //if (OnDialogEnd != null)
        //{
        //    OnDialogEnd(this, new DialogEventArgs() { Dialog = _dialog });
        //}
        _message = null;
        _dialog = null;
    }

    private void EndIngameDialog()
    {
        EventHub.Instance.IngameDialogClose(_ingameDialog);
        _ingameMessage = null;
        _ingameDialog = null;
    }

    public void OnSubmit(InputValue value)
    {
        Debug.Log("Dialog manager submit");
        if (_dialog != null)
        {
            DisplayNextMessage();
        }
    }

    public void StartIngameDialog(Dialog dialog)
    {
        _ingameDialog = dialog;

        _ingameMessages = new Queue<Message>();
        foreach (var message in dialog.Messages)
        {
            _ingameMessages.Enqueue(message);
        }

        Debug.Log(EventHub.Instance);
        EventHub.Instance.IngameDialogOpen(_ingameDialog);

        StartCoroutine(DisplayIngameDialog());
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
