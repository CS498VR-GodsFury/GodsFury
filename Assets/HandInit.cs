using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInit : MonoBehaviour {
    public bool grab;
    public bool penInHand;
    public GameObject pen;
    void Start()
    {
        grab = false;
        penInHand = false;
        var curRotation = this.transform.localRotation;
        curRotation.eulerAngles = new Vector3(-45, 180, 0);
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = curRotation;
    }

    private void checkGrab()
    {
        var tr = pen.GetComponent<Transform>().position;
        if (Vector3.Distance(this.transform.position, tr) < 0.05)
        {
            if (grab) penInHand = true;
        };
        if (!grab) penInHand = false;
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        grab = (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger));
        checkGrab();
        if(grab)
        {
            var scale = this.transform.localScale;
            scale = new Vector3(scale.x, scale.y, 0.0007f);
            this.transform.localScale = scale;

        }
        else
        {
            var scale = this.transform.localScale;
            scale = new Vector3(scale.x, scale.y, 0.001f);
            this.transform.localScale = scale;
        }

    }
}
