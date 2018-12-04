using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchScript : MonoBehaviour {


    public void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.tag == "Pullable")
        {
            GameObject pulledObj = other.gameObject;
            Rigidbody objBody = pulledObj.GetComponent<Rigidbody>();
            //float strength = (objBody.velocity.y == 0) ? 30.0f : objBody.velocity.y; 
            pulledObj.GetComponent<Rigidbody>().useGravity = false;
            System.Random random = new System.Random();
            float r = (float) random.NextDouble() * 0.2f;
            objBody.AddForce(new Vector3(0.0f + r/2, 1.0f - r, 0.0f + r/2) * 3000.0f);
            //objBody.AddForce(Vector3.up * 3000.0f);
            pulledObj.tag = "Unpullable";
            //Object.Destroy(pulledObj, 2.0f);
            //pulledObj.transform.position = Vector3.MoveTowards(pulledObj.transform.position, this.transform.position, speed * Time.deltaTime);
        }
    }

   /* public void OnTriggerExit(Collider other)
    {
        print("On trigger exit" + other.gameObject.tag);
        if (other.gameObject.tag == "Pullable") {
            other.gameObject.tag = "Unpullable";
        }

        
    }*/
}
