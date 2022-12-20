using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToMain());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GoToMain()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.GoToMainMenu();
    }
}
