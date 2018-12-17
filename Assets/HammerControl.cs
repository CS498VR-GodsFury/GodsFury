using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerControl : MonoBehaviour {

    // Use this for initialization
    Rigidbody RB;
    Vector3 lastLocation;
    public bool offHand;
    float updateTimer;
	void Start () {
        RB = this.GetComponent<Rigidbody>();
        lastLocation = this.transform.position;
        offHand = true;
        updateTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        updateTimer += Time.deltaTime;
        OVRInput.Update();
        if(!OVRInput.Get(OVRInput.RawButton.LHandTrigger))
        {
            
            RB.useGravity = true;
            //RB.freezeRotation = false;
            RB.constraints = RigidbodyConstraints.None;
            this.transform.parent = null;
            if (!offHand)
            {
                Vector3 F = (this.transform.position - lastLocation) * RB.mass;
                //print(F);
                RB.AddForceAtPosition(F*27f, this.transform.position + this.transform.up*.9f, ForceMode.Impulse);
                offHand = true;
            }
            updateTimer = 0;
        }
        else
        {
            offHand = false;
            RB.constraints = RigidbodyConstraints.FreezeAll;
            RB.useGravity = false;
            
            this.transform.parent = GameObject.Find("leftHand").transform;
            this.transform.localPosition = new Vector3(-1.9f, 205f, 18f);
            this.transform.localRotation = Quaternion.identity;
        }
        if (updateTimer > 0.16f)
        {
            lastLocation = this.transform.position;
            updateTimer = 0;
        }
    }
}
