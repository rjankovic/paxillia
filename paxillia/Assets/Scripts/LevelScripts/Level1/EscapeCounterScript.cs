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
    }

    private void BallPassed()
    {
        _passCounter++;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"{_passCounter}/{_targetCount}";
    }
}
