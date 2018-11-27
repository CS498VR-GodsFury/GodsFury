using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = GameObject.Find("OVRCameraRig").transform.position + new Vector3(0, 2, 2);
        //this.transform.rotation = GameObject.Find("OVRCameraRig").transform.rotation;
	}
}
