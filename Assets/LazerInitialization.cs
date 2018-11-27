using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerInitialization : MonoBehaviour {

    // Use this for initialization
    public GameObject rayTrack;
    public Vector3 hitPoint;
    Vector3 startControllerPosition;
    Vector3 startCameraPosition;
    private Ray lazerRay;
    private bool buttonStatus;
    private string[] buttonNames = { "Button1", "Button2", "Button3" };
	void Start () {
        hitPoint = new Vector3(0, 0, 0);
        var curRotation = this.transform.localRotation;
        curRotation.eulerAngles = new Vector3(0, 0, 0);
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = curRotation;
        buttonStatus = false;
    }
	
    void doRayCasting()
    {
        /* from unity website */
        //Ray lazerRay = new Ray(this.transform.position, this.transform.up);
        lazerRay.origin = this.transform.position;
        lazerRay.direction = this.transform.up;
        RaycastHit hitInfo = new RaycastHit();
        var controllerR = OVRInput.Controller.RTouch;
        var curPosition = OVRInput.GetLocalControllerPosition(controllerR);
        LineRenderer lr = rayTrack.GetComponent<LineRenderer>();
        if (Physics.Raycast(lazerRay, out hitInfo))
        {
            GameObject.Find("targetContainer").transform.position = hitInfo.point - lazerRay.direction * 0.2f;
            GameObject.Find("target").transform.forward = lazerRay.direction;
            
            Vector3[] points = new Vector3[2] { this.transform.position, hitInfo.point };
            lr.SetPositions(points);
           
            //print(hitPoint);

            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
            {
                var offset = (curPosition - startControllerPosition)*(startCameraPosition.y/2);
                print(offset);
                offset.y = 0;
                GameObject.Find("OVRCameraRig").transform.position = startCameraPosition - offset;
            }
            else
            {
                startControllerPosition = curPosition;
                startCameraPosition = GameObject.Find("OVRCameraRig").transform.position;
            }
            hitPoint = hitInfo.point;
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                if (!buttonStatus)
                {
                    var curY = GameObject.Find("OVRCameraRig").transform.position.y;
                    GameObject.Find("OVRCameraRig").transform.position = new Vector3(hitPoint.x, curY, hitPoint.z);
                }
                buttonStatus = true;
                
            }
            else
            {
                buttonStatus = false;
            }
        }
        else
        {
            Vector3[] points = new Vector3[2] { this.transform.position, this.transform.position + this.transform.up*100f };
            lr.SetPositions(points);
        }
        
    }
	// Update is called once per frame
	void Update () {
        //print(1 / Time.deltaTime);
        OVRInput.Update();

        
        doRayCasting();
	}
}
