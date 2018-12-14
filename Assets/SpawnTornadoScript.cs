using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTornadoScript : MonoBehaviour {
    public GameObject tornadoPrefab;

    private GameObject laser;
    private bool tornadoActive;
    private GameObject tornadoInstance;
	// Use this for initialization
	void Start () {
        tornadoActive = false;
        laser = GameObject.Find("Laser Pointer");
    }
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("leftHand").GetComponent<LeftHandSelector>().selectedWeapon != "Tornado") return;
        OVRInput.Update();
        if (OVRInput.GetDown(OVRInput.RawButton.Y)) {
            if (!tornadoActive)
            {
                tornadoActive = true;
                LazerInitialization lazerInfo = laser.GetComponent<LazerInitialization>();
                if (lazerInfo.isHit) {
                    Vector3 lazerPos = lazerInfo.hitPoint;
                    tornadoInstance = Instantiate(tornadoPrefab, lazerPos, tornadoPrefab.transform.rotation);
                }
                
            }
            else if (tornadoActive) {
                tornadoActive = false;
                Destroy(tornadoInstance, 0.1f);

            }
        }
    }
}
//0.8041326