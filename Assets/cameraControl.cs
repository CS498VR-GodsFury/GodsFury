using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {
    // Use this for initialization
    public float yAxis;
    public float xAxis;
    void Start () {
        xAxis = 0;
        yAxis = 0;
        //positionOffset = new Vector3(0, 5.5f, 0);
        //this.transform.rotation = plane.transform.rotation;
        //this.transform.position = plane.transform.position + positionOffset;
	}
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        var axis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        var curPos = this.transform.position;
        var curRot = this.transform.rotation;
        if(Mathf.Abs(axis.x)> Mathf.Abs(xAxis))
            xAxis = axis.x;

        if (Mathf.Abs(axis.y) > Mathf.Abs(yAxis))
            yAxis = axis.y;
        if ((axis.x == 0)&&(axis.y==0))
        {
            if( Mathf.Abs(xAxis) > Mathf.Abs(yAxis) )
            {
                if(Mathf.Abs(xAxis)>0.2f)
                    curRot.eulerAngles += new Vector3(0, xAxis * 45f, 0);
            }
            else
            {
                if(Mathf.Abs(yAxis)>0.2f)
                    curPos += new Vector3(0, (this.transform.position.y-40f+1.5f) * yAxis/4, 0);
            }
            xAxis = 0;
            yAxis = 0;
        }
        
        
        this.transform.rotation = curRot;
        this.transform.position = curPos;
    }
}
