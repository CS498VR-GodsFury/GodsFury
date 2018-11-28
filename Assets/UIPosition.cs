using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = GameObject.Find("OVRCameraRig").transform.position + new Vector3(0, 0.5f, 2.5f);
        //this.transform.rotation = GameObject.Find("OVRCameraRig").transform.rotation;
	}
}
