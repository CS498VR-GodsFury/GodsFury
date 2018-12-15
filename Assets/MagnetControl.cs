using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetControl : MonoBehaviour {

    // Use this for initialization
    public float radius;
    public float distance;
    Vector3 offset;
    bool APressed;
	void Start () {
        offset = new Vector3(0, 0, 0);
        radius = 0f;
        distance = 20f;
        var hand = GameObject.Find("leftHand");
        var handPosition = hand.transform.position;
        this.transform.position = handPosition + hand.transform.up*distance;
        APressed = false;
        //GameObject.Find("MagnetField").position = handPosition + hand.transform.up * 20f
    }
	
    void checkOverlap()
    {
        if (radius <= 0) return;
        var overLapInfo = Physics.OverlapSphere(this.transform.position, radius*1.25f);
        foreach(var collider in overLapInfo)
        {
            var towardsVector = this.transform.position - collider.transform.position;
            try
            {
                var rBody = collider.gameObject.GetComponent<Rigidbody>();
                var distCof = towardsVector.magnitude;
                if (distCof < radius*1.1f) rBody.velocity = new Vector3(0, 0, 0);
                rBody.AddForce(towardsVector.normalized*900f* rBody.mass);
                //rBody.AddExplosionForce(10000f * rBody.mass, this.transform.position, radius * 2.25f, 1);
                //print(distCof);
            }
            catch
            {

            }
        }
    }

    void explode()
    {
        var overLapInfo = Physics.OverlapSphere(this.transform.position, radius * 1.25f);
        foreach (var collider in overLapInfo)
        {
            //var towardsVector = this.transform.position - collider.transform.position;
            try
            {
                var rBody = collider.gameObject.GetComponent<Rigidbody>();
                
                rBody.AddExplosionForce(4005f*rBody.mass, this.transform.position, radius * 1.55f, 1, ForceMode.Impulse);
                //print(distCof);
            }
            catch
            {

            }
        }
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        if (GameObject.Find("leftHand").GetComponent<LeftHandSelector>().selectedWeapon != "Magnet")
        {
            return;
        }
        if (this.radius <= 0) return;
        var dTime = Time.deltaTime;
        var hand = GameObject.Find("leftHand");
        var handPosition = hand.transform.position;
        var newOffset = handPosition + hand.transform.up * distance - this.transform.position;
        /*
        print((newOffset - offset).magnitude);
        
        //print(offset.magnitude);
        */
        if ((newOffset - offset).magnitude > 1f)
        {
            offset = newOffset;
        }
        /*
        print("---------");
        print(offset);
        print(newOffset);
        print("---------");
        */
        //offset = newOffset;
        if( (newOffset.magnitude > 0.01f)&&(offset.magnitude>1f) )
            this.GetComponent<Rigidbody>().MovePosition(this.transform.position + offset*4f*dTime);
        if (this.radius > 0)
        {
            //print("woof");
            checkOverlap();
        }

    }

    void controlMagnet()
    {
        if( OVRInput.Get(OVRInput.RawButton.A) )
        {
            if (!APressed)
            {
                if (this.radius > 0)
                {
                    explode();
                    this.radius = 0;
                    this.distance = 20f;
                }
                else this.radius = 5f;
            }
            APressed = true;
        }
        else
        {
            APressed = false;
        }

        if(this.radius > 0)
        {
            var axis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
            if (Mathf.Abs(axis.x) > Mathf.Abs(axis.y))
                axis.y = 0;
            else
                axis.x = 0;
            this.distance = Mathf.Max(this.radius + 5f, this.distance + axis.y);
            this.radius = Mathf.Min(this.distance - 5f, this.radius + axis.x*0.03f*this.radius);
            this.radius = Mathf.Max(this.radius, 1f);
        }
    }
    void Update () {
        OVRInput.Update();
        if( GameObject.Find("leftHand").GetComponent<LeftHandSelector>().selectedWeapon != "Magnet" )
        {
            this.radius = 0f;
            this.distance = 20f;
            GameObject.Find("MagnetField").transform.localScale = new Vector3(2 * radius, 2 * radius, 2 * radius);
            return;
        }
        controlMagnet();
        GameObject.Find("MagnetField").transform.localScale = new Vector3(2 * radius, 2 * radius, 2 * radius);
	}
}
