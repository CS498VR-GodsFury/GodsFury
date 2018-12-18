using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGManipulation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print(this.name + "BG");
        GameObject.Find(this.name + "BG").GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
	}

    private void OnCollisionStay(Collision collision)
    {
        //GameObject.Find(this.name + "BG").GetComponent<RawImage>().color = new Color(255, 167, 0, 0.5f);
    }

    private void OnCollisionExit(Collision collision)
    {
        //GameObject.Find(this.name + "BG").GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
