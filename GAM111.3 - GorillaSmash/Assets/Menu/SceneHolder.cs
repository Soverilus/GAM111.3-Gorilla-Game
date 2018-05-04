using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneHolder : MonoBehaviour {
    GameObject winLoseScreen;

    public string[] loseStrings;
    public string[] winStrings;

    public Text winText;
    public Text loseText;


    void Start() {
        if (GameObject.FindGameObjectWithTag("WinLose") != null) {
            winLoseScreen = GameObject.FindGameObjectWithTag("WinLose");
            winLoseScreen.SetActive(false);
        }
    }

    public void WinLose(bool win) {
        winLoseScreen.SetActive(true);
        if (win) {
            winText.text = winStrings[(int)Random.Range(0, winStrings.Length)];
        }
        else {
            loseText.text = loseStrings[(int)Random.Range(0, winStrings.Length)];
        }
    }
    public void SceneTransition(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication() {
        Application.Quit();
    }
}
