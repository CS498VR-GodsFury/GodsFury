using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDisasterScript : MonoBehaviour {
    public GameObject tornadoPrefab;
    public GameObject cometPrefab;

    private GameObject laser;
    private bool tornadoActive;
    private GameObject tornadoInstance;
	// Use this for initialization
	void Start () {
        tornadoActive = false;
        laser = GameObject.Find("Laser Pointer");
    }

    // Update is called once per frame
    void Update() {
        string selectedWeapon = GameObject.Find("leftHand").GetComponent<LeftHandSelector>().selectedWeapon;
        OVRInput.Update();
        bool isButtonNotDown = !OVRInput.GetDown(OVRInput.RawButton.A);
        if (isButtonNotDown) return;

        if (string.Equals(selectedWeapon, "Tornado"))
        {
            print("Pressed Y");
            if (!tornadoActive)
            {
                print("We are spawning a tornado!");
                tornadoActive = true;
                LazerInitialization lazerInfo = laser.GetComponent<LazerInitialization>();
                if (lazerInfo.isHit)
                {
                    Vector3 lazerPos = lazerInfo.hitPoint;
                    tornadoInstance = Instantiate(tornadoPrefab, lazerPos, tornadoPrefab.transform.rotation);
                }

            }
            else if (tornadoActive)
            {
                tornadoActive = false;
                Destroy(tornadoInstance, 0.1f);

            }
        }
        else if (string.Equals(selectedWeapon, "Comet"))
        {
            LazerInitialization lazerInfo = laser.GetComponent<LazerInitialization>();
            if (lazerInfo.isHit)
            {
                Vector3 cameraPos = this.transform.position;
                Vector3 cometPos = new Vector3(cameraPos.x + Random.Range(-850, 850), cameraPos.y + 1024, cameraPos.z + Random.Range(-850, 850));
                Instantiate(cometPrefab, cometPos, cometPrefab.transform.rotation);
            }
        }
    }
}