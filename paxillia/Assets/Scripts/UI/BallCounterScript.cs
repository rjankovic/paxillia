using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallCounterScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        //_text = GetComponent<TextMeshPro>();
        //Debug.Log(_text);
        //textMesh.text
        _text.text = GameManager.Instance.BallCount.ToString();

        EventHub.Instance.OnBallCountUpdate += OnBallCountUpdate;
    }

    private void OnBallCountUpdate(int obj)
    {
        _text.text = GameManager.Instance.BallCount.ToString();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}


}
