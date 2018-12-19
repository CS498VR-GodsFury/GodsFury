using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
    public Toggle toggle;
    
        public void loadIslandLevel()
    {
        PlayerPrefs.DeleteAll();
        checkPedestrians();
        SceneManager.LoadScene("Island", LoadSceneMode.Single);
    }

    public void loadIslandLevelWithTutorial()
    {
        PlayerPrefs.DeleteAll();
        checkPedestrians();
        PlayerPrefs.SetString("TutorialEnabled", "Activated");
        SceneManager.LoadScene("Island", LoadSceneMode.Single);
    }

    public void checkPedestrians() {
        if (toggle.isOn)
            PlayerPrefs.SetInt("Pedestrians", 1);
        else
            PlayerPrefs.SetInt("Pedestrians", 0);
    }
    public void quitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
