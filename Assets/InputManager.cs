using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {
    public GameObject cameraParent;
    public AudioSource playerAudioSource;


    // Use this for initialization
    void Start()
    {
        Vector3 positionVec = new Vector3(PlayerPrefs.GetFloat("posX"),
                                                      PlayerPrefs.GetFloat("posY"),
                                                      PlayerPrefs.GetFloat("posZ"));
        if (Vector3.Equals(positionVec, Vector3.zero)) return;

        playerAudioSource.Play();

        cameraParent.transform.position = positionVec;
        cameraParent.transform.rotation = new Quaternion(PlayerPrefs.GetFloat("rotX"),
                                                      PlayerPrefs.GetFloat("rotY"),
                                                      PlayerPrefs.GetFloat("rotZ"),
                                                      PlayerPrefs.GetFloat("rotW"));
        Debug.Log("Position; " + cameraParent.transform.position);
    }

        // Update is called once per frame
        void Update () {
        if (OVRInput.Get(OVRInput.RawButton.Y)) {
            Debug.LogWarning("Scene is reloading");
            PlayerPrefs.SetFloat("posX", cameraParent.transform.position.x);
            PlayerPrefs.SetFloat("posY", cameraParent.transform.position.y);
            PlayerPrefs.SetFloat("posZ", cameraParent.transform.position.z);
            PlayerPrefs.SetFloat("rotX", cameraParent.transform.rotation.x);
            PlayerPrefs.SetFloat("rotY", cameraParent.transform.rotation.y);
            PlayerPrefs.SetFloat("rotZ", cameraParent.transform.rotation.z);
            PlayerPrefs.SetFloat("rotW", cameraParent.transform.rotation.w);

            SceneManager.LoadScene("Island", LoadSceneMode.Single);
            
        }
	}
}
