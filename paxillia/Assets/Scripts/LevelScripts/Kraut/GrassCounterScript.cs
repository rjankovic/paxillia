using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrassCounterScript : MonoBehaviour
{
    [SerializeField]
    private int _targetPercentage = 80;
    [SerializeField]
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        EventHub.Instance.OnGrassCountUpdate += OnGrassCountUpdate;
        StartCoroutine(UpdateWithDelay());
    }

    IEnumerator UpdateWithDelay()
    {
        yield return new WaitForSeconds(1f);
        UpdateText();
    }

    private void UpdateText()
    {
        if (GameManager.Instance.TotalGrassCount == 0)
        {
            _text.text = $"0/{_targetPercentage}%";
            return;
        }

        Debug.Log($"GC {GameManager.Instance.GrassCount} TGC {GameManager.Instance.TotalGrassCount}");

        var pct = (GameManager.Instance.TotalGrassCount - GameManager.Instance.GrassCount) * 100 / GameManager.Instance.TotalGrassCount;
        _text.text = $"{pct}/{_targetPercentage}%";
        if (pct >= _targetPercentage)
        {
            GameManager.Instance.GrassWiped = true;
            EventHub.Instance.EnoughGrassDropped();
        }
    }

    private void OnGrassCountUpdate()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
