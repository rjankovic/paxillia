using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaloriesBarScript : MonoBehaviour
{
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        EventHub.Instance.OnCalorieCounterUpdated += OnCalorieCounterUpdated;
    }

    private void OnCalorieCounterUpdated()
    {
        SetValue(GameManager.Instance.CaloriesCounter);
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
