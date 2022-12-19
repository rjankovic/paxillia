using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrandmaHealthBarScript : MonoBehaviour
{
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GrandmaHealth = 1000;
        }

        EventHub.Instance.OnGrandmaHealthUpdated += OnGrandmaHealthUpdated;
    }

    private void OnGrandmaHealthUpdated()
    {
        SetValue(GameManager.Instance.GrandmaHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(int value)
    { 
        slider.value = value;
    }
}
