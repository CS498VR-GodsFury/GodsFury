using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSelector : MonoBehaviour {

    GameObject rightTutorialScreen;
    GameObject leftTutorialScreen;

    // Use this for initialization
    void Start () {
        rightTutorialScreen = GameObject.Find("RightTutorialScreen");
        leftTutorialScreen = GameObject.Find("LeftTutorialScreen");



    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
