using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadblockSign : MonoBehaviour
{
    private List<string> names = new List<string>()
    {
        "Josephine",
        "Géraldine",
        "Célestine",
        "Cosette",
        "Béatrice",
        "Mélanie",
        "Cléméntine",
        "Dominique",
        "Karine",
        "Jeanne",
        "Chloé"
    };

    private int nameIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        nameIndex = ((int)(Mathf.Floor(Random.value * names.Count))) % names.Count;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool _active = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_active)
        {
            return;
        }

        if (collision.gameObject.tag != Constants.TAG_BALL)
        {
            return;
        }

        EventHub.Instance.DialogPause();

        EventHub.Instance.OnDialogClose += Unpause;

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
            {
                new Message() { Text = $"I went to give {names[nameIndex]} her regular beating, wird be back soon.\n\n-Gertruda" }
            }
        });

        StartCoroutine(ActivateDelay());
    }

    void Unpause(Dialog d)
    {
        EventHub.Instance.DialogUnpause();
        EventHub.Instance.OnDialogClose -= Unpause;
    }

    void OnBecameInvisible()
    {
        nameIndex = (nameIndex+1) % names.Count;
    }

    private IEnumerator ActivateDelay()
    {
        _active = false;
        yield return new WaitForSeconds(6f);
        _active = true;
    }

}
