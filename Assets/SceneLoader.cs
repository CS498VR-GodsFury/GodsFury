using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    
        public void loadIslandLevel()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Island", LoadSceneMode.Single);
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
