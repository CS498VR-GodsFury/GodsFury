using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerController : MonoBehaviour {

    // Use this for initialization
    private GameObject hand;
	void Start () {
        hand = GameObject.Find("hand");
	}
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        var PenInHand = true;// hand.GetComponent<HandInit>().penInHand;
        if(PenInHand)
        {
            this.transform.position = hand.transform.position;
            this.transform.rotation = hand.transform.rotation;
        }
	}
}
