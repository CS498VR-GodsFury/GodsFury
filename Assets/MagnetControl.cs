using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetControl : MonoBehaviour {

    // Use this for initialization
    public float radius;
    public float distance;
    Vector3 offset;
	void Start () {
        offset = new Vector3(0, 0, 0);
        radius = 0f;
        distance = 20f;
        var hand = GameObject.Find("leftHand");
        var handPosition = hand.transform.position;
        this.transform.position = handPosition + hand.transform.up*distance;
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
                rBody.AddForce(towardsVector.normalized*500* rBody.mass);
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
        checkOverlap();
    }

    void controlMagnet()
    {
        if( OVRInput.GetDown(OVRInput.RawButton.Y) )
        {
            if (this.radius > 0)
            {
                this.radius = 0;
                this.distance = 20f;
            }
            else this.radius = 5f;
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
        controlMagnet();
        GameObject.Find("MagnetField").transform.localScale = new Vector3(2 * radius, 2 * radius, 2 * radius);
	}
}
