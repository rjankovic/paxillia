using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EscapeCounterScript : MonoBehaviour
{
    private int _passCounter = 0;
    [SerializeField]
    private int _targetCount = 2;
    [SerializeField]
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
        EventHub.Instance.OnBallPassed += BallPassed;
        GameManager.Instance.BallEscapeTarget = _targetCount;
    }

    private void BallPassed()
    {
        _passCounter++;
        GameManager.Instance.BallEscapeCount = _passCounter;
        UpdateText();

        //if (GameManager.Instance.BallCount == 0 && _passCounter >= _targetCount)
        //{
        //    EventHub.Instance.Pause();
        //}
    }

    private void UpdateText()
    {
        _text.text = $"{_passCounter}/{_targetCount}";
    }
}
