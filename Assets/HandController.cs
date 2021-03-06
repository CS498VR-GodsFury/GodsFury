﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {
        OVRInput.Update();
        var controllerR = OVRInput.Controller.RTouch;
        var curPosition = OVRInput.GetLocalControllerPosition(controllerR);// + GameObject.Find("OVRCameraRig").transform.position;
        //print(curPosition);
        var curRotation = OVRInput.GetLocalControllerRotation(controllerR);
        //print(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
        this.transform.localPosition = curPosition;
        //var curRotation = this.transform.rotation;
        // curRotation.eulerAngles = new Vector3(0, 0, 0);
        //curRotation.eulerAngles += new Vector3(90, 0, 0);
        this.transform.localRotation = curRotation;
    }
}
