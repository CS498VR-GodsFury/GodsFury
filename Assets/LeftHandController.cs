﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        var controllerR = OVRInput.Controller.LTouch;
        var curPosition = OVRInput.GetLocalControllerPosition(controllerR) + GameObject.Find("OVRCameraRig").transform.position;
        var curRotation = OVRInput.GetLocalControllerRotation(controllerR);
        //print(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
        this.transform.position = curPosition;
        //var curRotation = this.transform.rotation;
        // curRotation.eulerAngles = new Vector3(0, 0, 0);
        //curRotation.eulerAngles += new Vector3(90, 0, 0);
        this.transform.rotation = curRotation;
    }
}
