using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {
	// Use this for initialization
	void Start () {
        //positionOffset = new Vector3(0, 5.5f, 0);
        //this.transform.rotation = plane.transform.rotation;
        //this.transform.position = plane.transform.position + positionOffset;
	}
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        var axis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        var curPos = this.transform.position;
        var curRot = this.transform.rotation;
        curPos += new Vector3(0, axis.y*0.3f, 0);
        curRot.eulerAngles += new Vector3(0, axis.x*0.1f, 0);
        this.transform.rotation = curRot;
        this.transform.position = curPos;
    }
}
