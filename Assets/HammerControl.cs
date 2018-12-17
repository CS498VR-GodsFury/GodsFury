using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerControl : MonoBehaviour {

    // Use this for initialization
    Rigidbody RB;
    Vector3 lastLocation;
    Vector3 currentLocation;
    public bool offHand;
    public GameObject lightening;
    public GameObject Fire;
    bool hit;
    bool APressed;
    bool BPressed;
    public bool HammerEnabled;
    public bool lighteningEnabled;
    float updateTimer;
	void Start () {
        RB = this.GetComponent<Rigidbody>();
        lastLocation = this.transform.position;
        currentLocation = this.transform.position;
        offHand = true;
        updateTimer = 0;
        HammerEnabled = false;
        lighteningEnabled = false;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (!lighteningEnabled) return;
        if(!hit)
        {
            Fire.active = true;
            
            //print(collision.contacts[0].point);
            lightening.transform.position = collision.contacts[0].point;
            lightening.transform.parent = collision.gameObject.transform;
            //Fire.GetComponent<ParticleSystem>().ex

        }
        hit = true;
    }
    // Update is called once per frame
    void Update () {
        if(lighteningEnabled)
            lightening.transform.rotation = Quaternion.identity;
        updateTimer += Time.deltaTime;
        OVRInput.Update();

        if((!APressed)&&(OVRInput.Get(OVRInput.RawButton.A)))
        {
            HammerEnabled = !HammerEnabled;
        }
        HammerEnabled = HammerEnabled && (GameObject.Find("leftHand").GetComponent<LeftHandSelector>().selectedWeapon == "Hammer");
        
        this.GetComponent<MeshRenderer>().enabled = HammerEnabled;
        this.GetComponent<BoxCollider>().enabled = HammerEnabled;

        APressed = OVRInput.Get(OVRInput.RawButton.A);

        if (HammerEnabled)
        {
            if((!BPressed)&&(OVRInput.Get(OVRInput.RawButton.B)))
            {
                lighteningEnabled = !lighteningEnabled;
                lightening.SetActive(lighteningEnabled);
            }
            BPressed = OVRInput.Get(OVRInput.RawButton.B);
        }
        lighteningEnabled = lighteningEnabled && HammerEnabled;
        if ( (!OVRInput.Get(OVRInput.RawButton.LHandTrigger))&&(HammerEnabled) )
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

            if (lighteningEnabled)
            {
                Fire.active = false;
                lightening.transform.parent = this.transform;
                lightening.transform.localPosition = new Vector3(0, 5.49f, 0);
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
