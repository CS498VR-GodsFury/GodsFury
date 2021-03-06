﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLaserPointer : MonoBehaviour {

    public GameObject laserPointer;
    private GameObject target;

	// Use this for initialization
	void Start () {
        target = GameObject.Find("target");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(laserPointer.transform.position, laserPointer.transform.up);
        if (Physics.Raycast(ray, out hit))
        {
            target.SetActive(true);
            GameObject.Find("targetContainer").transform.position = hit.point;

            //Scale target impact point based of ray distance
            float scale = hit.distance * 0.01f;
            target.transform.localScale = new Vector3(scale, scale, scale);

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                if (hit.collider.GetComponent<Button>() != null)
                {
                    Button button = hit.collider.GetComponent<Button>();
                    button.onClick.Invoke();
                }
                else if (hit.collider.GetComponent<Toggle>()) {
                    
                    Toggle toggle = hit.collider.GetComponent<Toggle>();
                    Debug.Log("Hit checkbox " + toggle.isOn);
                    toggle.isOn = !toggle.isOn;  
                }
            }

        }
        else
        {
            target.SetActive(false);
        }

	}
}



