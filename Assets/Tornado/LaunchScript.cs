using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchScript : MonoBehaviour {


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            GameObject pulledObj = other.gameObject;
            Rigidbody objBody = pulledObj.GetComponent<Rigidbody>();

            pulledObj.GetComponent<Rigidbody>().useGravity = false;
            System.Random random = new System.Random();
            float r = (float) random.NextDouble() * 0.2f;
            objBody.AddForce(new Vector3(0.0f + r/2, 1.0f - r, 0.0f + r/2) * 30000.0f);

            pulledObj.tag = "Unpullable";
        }
    }

}
