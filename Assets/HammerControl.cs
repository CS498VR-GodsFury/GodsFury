using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerControl : MonoBehaviour {

    // Use this for initialization
    Rigidbody RB;
    Vector3 lastLocation;
    Vector3 currentLocation;
    public bool offHand;
    public GameObject lightning;
    public GameObject Fire;
    bool hit;
    //bool APressed;
    bool BPressed;
    //public bool HammerEnabled;
    public bool lightningEnabled;
    float updateTimer;
	void Start () {
        RB = this.GetComponent<Rigidbody>();
        lastLocation = this.transform.position;
        currentLocation = this.transform.position;
        offHand = true;
        updateTimer = 0;
        //HammerEnabled = false;
        lightningEnabled = false;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (!lightningEnabled) return;
        if(!hit)
        {
            Fire.SetActive(true);
            
            lightning.transform.position = collision.contacts[0].point;
            lightning.transform.parent = collision.gameObject.transform;

        }
        hit = true;
    }

    // Update is called once per frame
    void Update () {
        if(lightningEnabled)
            lightning.transform.rotation = Quaternion.identity;
        updateTimer += Time.deltaTime;
        OVRInput.Update();

        bool hammerSelected = GameObject.Find("leftHand").GetComponent<LeftHandSelector>().selectedWeapon == "Hammer";
        this.GetComponent<MeshRenderer>().enabled = hammerSelected;
        this.GetComponent<BoxCollider>().enabled = hammerSelected;

        if (hammerSelected)
        {
            if ((!BPressed) && (OVRInput.Get(OVRInput.RawButton.B)))
            {
                lightningEnabled = !lightningEnabled;
                lightning.SetActive(lightningEnabled);
            }
            BPressed = OVRInput.Get(OVRInput.RawButton.B);
        }
        else {
            lightningEnabled = false;
        }
        
        lightning.SetActive(lightningEnabled) ;
        if ( (!OVRInput.Get(OVRInput.RawButton.LHandTrigger))&&(hammerSelected) )
        {
            
            RB.useGravity = true;
            //RB.freezeRotation = false;
            RB.constraints = RigidbodyConstraints.None;
            this.transform.parent = null;
            if (!offHand)
            {
                Vector3 F = (currentLocation - lastLocation) * RB.mass;
                //print(F);
                RB.AddForceAtPosition(F*40f, this.transform.position + this.transform.up*.9f, ForceMode.Impulse);
                offHand = true;
            }
            updateTimer = 0;
        }
        else
        {
            offHand = false;

            if (lightningEnabled)
            {
                Fire.SetActive(false);
                lightning.transform.parent = this.transform;
                lightning.transform.localPosition = new Vector3(0, 5.49f, 0);
            }

            hit = false;
            RB.constraints = RigidbodyConstraints.FreezeAll;
            RB.useGravity = false;
            
            this.transform.parent = GameObject.Find("leftHand").transform;
            this.transform.localPosition = new Vector3(-66.8f, 97.8f, 18f);
            var rot = Quaternion.Euler(0, 0, 45);

            this.transform.localRotation = rot;
        }
        if (updateTimer > 0.10f)
        {
            lastLocation = currentLocation;
            currentLocation = this.transform.position;
            updateTimer = 0;
        }
    }
}
