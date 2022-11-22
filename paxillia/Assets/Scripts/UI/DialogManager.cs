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

    [SerializeField]
    private GameObject _pausePanel;

    [SerializeField]
    private GameObject _yesButton;
    [SerializeField]
    private GameObject _noButton;
    [SerializeField]
    private GameObject _okButton;

    private Action _yesAction;
    private Action _noAction;


    private bool _gamePaused = false;

    

    //[SerializeField]
    //private Animator _animator;


    //public event DialogEventHandler OnDialogStart;
    //public event DialogEventHandler OnDialogEnd;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            UnityEngine.Object.Destroy(gameObject);
            return;
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    //private void Start()
    //{
    //    DontDestroyOnLoad(gameObject);

    //    if (Instance != null && Instance != this)
    //    {
    //        UnityEngine.Object.Destroy(gameObject);
    //        return;
    //    }

    //}


    private void SetButtonsForDialog()
    {
        _yesButton.SetActive(false);
        _noButton.SetActive(false);
        _okButton.SetActive(true);
    }

    private void SetButtonsForYesNo()
    {
        _yesButton.SetActive(true);
        _noButton.SetActive(true);
        _okButton.SetActive(false);
    }

    public void StartDialog(Dialog dialog)
    {
        StartCoroutine(StartDialogInner(dialog));
        //SetButtonsForDialog();
        //_dialog = dialog;
        //_messages = new Queue<Message>();
        //foreach (var message in dialog.Messages)
        //{
        //    _messages.Enqueue(message);
        //}

        ////_animator.SetBool("DialogOpen", true);

        //EventHub.Instance.DialogOpen(_dialog);
        ////if (OnDialogStart != null)
        ////{
        ////    OnDialogStart(this, new DialogEventArgs() { Dialog = _dialog });
        ////}

        //DisplayNextMessage();
    }


    private IEnumerator StartDialogInner(Dialog dialog)
    {
        yield return new WaitForSeconds(0.2f);

        SetButtonsForDialog();
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

    public void StartYesNoDialog(Dialog dialog, Action yesAction, Action noAction)
    {
        SetButtonsForYesNo();
        _yesAction = yesAction;
        _noAction = noAction;

        _dialog = dialog;
        _messages = new Queue<Message>();
        foreach (var message in dialog.Messages)
        {
            _messages.Enqueue(message);
        }

        EventHub.Instance.DialogOpen(_dialog);

        DisplayNextMessage();
    }

    public void DisplayNextMessage()
    {
        //Debug.Log("Display next message, MC " + _messages.Count);
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

    public void YesClick()
    {
        EndDialog();
        if (_yesAction != null)
        {
            _yesAction();
        }
    }

    public void NoClick()
    {
        EndDialog();
        if (_noAction != null)
        {
            _noAction();
        }
    }

    //private IEnumerator DisplayIngameDialog()
    //{
    //    while (_ingameMessages.Count > 0)
    //    { 
    //        _ingameMessage = _ingameMessages.Dequeue();
    //        _currentIngameText.text = _ingameMessage.Text;
    //        _currentIngameDialogCharacter.text = _ingameMessage.Character ?? string.Empty;
    //        yield return new WaitForSeconds(_ingameMessage.Duration);
    //    }
    //    EndIngameDialog();
    //}

    private IEnumerator DisplayNextIngameMessage(int remainingCount)
    {
        if (_ingameMessages.Count == 0)
        {
            EndIngameDialog();
            yield return null;
        }

        if (_ingameMessages.Count < remainingCount)
        {
            yield return null;
        }
        if (_ingameMessages.Count > 0)
        {

            _ingameMessage = _ingameMessages.Dequeue();
            _currentIngameText.text = _ingameMessage.Text;
            _currentIngameDialogCharacter.text = _ingameMessage.Character ?? string.Empty;
            yield return new WaitForSeconds(_ingameMessage.Duration);

            StartCoroutine(DisplayNextIngameMessage(_ingameMessages.Count));
        }
    }

    public void SkipIngameMessage()
    {
        StartCoroutine(DisplayNextIngameMessage(_ingameMessages.Count));
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

    public void EndIngameDialog()
    {
        EventHub.Instance.IngameDialogClose(_ingameDialog);
        _ingameMessage = null;
        _ingameDialog = null;
    }

    public void OnSubmit(InputValue value)
    {
        //Debug.Log("Dialog manager submit");
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

        //Debug.Log(EventHub.Instance);
        EventHub.Instance.IngameDialogOpen(_ingameDialog);

        StartCoroutine(DisplayNextIngameMessage(_ingameMessages.Count));
        //StartCoroutine(DisplayIngameDialog());
    }


    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);

        //if (Instance != null && Instance != this)
        //{
        //    UnityEngine.Object.Destroy(gameObject);
        //    return;
        //}


        _gamePaused = false;
        _pausePanel.SetActive(false);
        EventHub.Instance.OnPaused += Pause;
        EventHub.Instance.OnUnpaused += Resume;
    }

    public void OnEscape(InputValue inputValue)
    {
        //Debug.Log("OnExcape!");

        if (!_gamePaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
        //Debug.Log(inputValue.Get().GetType());
        //Debug.Log(inputValue.Get());
    }

    private bool _inputWasEnabled = false;

    private void Pause()
    {
        _gamePaused = true;
        _pausePanel.SetActive(true);
        _inputWasEnabled = GameManager.Instance.InputEnabled;
        Time.timeScale = 0;
        EventHub.Instance.InputEnabled(false);
    }

    public void Resume()
    {
        _gamePaused = false;
        _pausePanel.SetActive(false);
        if (_inputWasEnabled)
        {
            EventHub.Instance.InputEnabled(true);
        }
        Time.timeScale = 1;
    }

    public void ExitGameFromPause()
    {
        GameManager.Instance.GoToMainMenu();
        Resume();
    }

    public void LoadGameFromPause()
    {
        GameManager.Instance.LoadGame();
        Resume();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
