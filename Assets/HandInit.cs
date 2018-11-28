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


    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
    }
}
