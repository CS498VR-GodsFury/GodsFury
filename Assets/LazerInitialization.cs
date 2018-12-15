using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerInitialization : MonoBehaviour {

    // Use this for initialization
    public GameObject rayTrack;
    public Vector3 hitPoint;
    private GameObject target;
    private GameObject cameraRig; 
    Vector3 startControllerPosition;
    Vector3 startCameraPosition;
    public Vector3[] LRpoints;
    private Ray lazerRay;
    private bool buttonStatus;
    public bool isHit;
	void Start () {
        hitPoint = new Vector3(0, 0, 0);
        var curRotation = this.transform.localRotation;
        curRotation.eulerAngles = new Vector3(0, 0, 0);
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = curRotation;
        buttonStatus = false;
        LRpoints = new Vector3[2];
        target = GameObject.Find("target");
        cameraRig = GameObject.Find("OVRCameraRig");
    }
	
    void doRayCasting()
    {
        /* from unity website */
        lazerRay.origin = this.transform.position;
        lazerRay.direction = this.transform.up;
        RaycastHit hitInfo = new RaycastHit();
        var controllerR = OVRInput.Controller.RTouch;
        var curPosition = OVRInput.GetLocalControllerPosition(controllerR);
        
        if (Physics.Raycast(lazerRay, out hitInfo))
        {
            LRpoints[1] = hitInfo.point;
            isHit = true;
            target.SetActive(true);
            GameObject.Find("targetContainer").transform.position = hitInfo.point - lazerRay.direction * 0.2f;

            //Scale target impact point based of ray distance
            float scale = hitInfo.distance * 0.01f;
            target.transform.localScale = new Vector3(scale, scale, scale);

            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
            {
                var offset = (curPosition - startControllerPosition)*(startCameraPosition.y/2);
                //print(offset);
                offset.y = 0;
                cameraRig.transform.position = startCameraPosition;
                cameraRig.transform.Translate(- offset, Space.Self);
            }
            else
            {
                startControllerPosition = curPosition;
                startCameraPosition = cameraRig.transform.position;
            }
            hitPoint = hitInfo.point;
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                if (!buttonStatus)
                {
                    var curY = cameraRig.transform.position.y;
                    cameraRig.transform.position = new Vector3(hitPoint.x, curY, hitPoint.z);
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
            isHit = false;
            target.SetActive(false);
        }
        
    }

    private void LateUpdate()
    {
        LineRenderer lr = rayTrack.GetComponent<LineRenderer>();
        LRpoints[0] = this.transform.position;
        if (!isHit) LRpoints[1] = this.transform.position + this.transform.up * 50f;
        lr.SetPositions(LRpoints);
    }
    // Update is called once per frame
    void Update () {
        //print(1 / Time.deltaTime);
        OVRInput.Update();

        
        doRayCasting();
	}
}
