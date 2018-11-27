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
        //this.transform.rotation = new Quaternion(1, 0, 0, 0);
        this.transform.position = GameObject.Find("CenterEyeAnchor").transform.position + new Vector3(0,0,2);
    }
}
