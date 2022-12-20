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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CaloriesCounter = 0;
        }

        EventHub.Instance.OnCalorieCounterUpdated += OnCalorieCounterUpdated;
    }

    private void OnCalorieCounterUpdated()
    {
        SetValue(GameManager.Instance.CaloriesCounter);

        if (slider.value < 1000)
        {
            return;
        }

        EventHub.Instance.InputEnabled(false);

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
                    {
                        new Message() { Text = $"Om, nom, nom...brwrrr...ble..bloh...ufff...\nWhen grandma told you to sit down and eat, you should have listened..."},
                    }
        });

        EventHub.Instance.OnDialogClose += (x) =>
        {
                //Time.timeScale = 1;
            EventHub.Instance.InputEnabled(true);
            GameManager.Instance.LoadGame();
        };


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
