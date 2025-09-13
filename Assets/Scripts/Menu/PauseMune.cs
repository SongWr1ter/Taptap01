using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMune : MonoBehaviour
{
    public GameObject panel;
    // Start is called before the first frame update
    public void ContinueButton()
    {
        panel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
