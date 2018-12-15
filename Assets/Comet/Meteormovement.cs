using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteormovement : MonoBehaviour {
    public GameObject comet;
    public ParticleSystem expObject;
    private ParticleSystem cometTrail;

    //private Vector3 currentPosition;
    private Vector3 targetPosition;
    private float velocity = 10.0f;
    private Vector3 directionVector;

    private const float MAX_POWER = 60000000.0f;
    private const float IMPACT_RADIUS = 75.0f;

    private bool endOfLifeCycle;
    public bool shoot;
    public GameObject laser;
	// Use this for initialization
	void Start () {
        print("Did we manage to spawn the thing?");
        transform.localScale = new Vector3(10, 10, 10);
        endOfLifeCycle = false;
        laser = GameObject.Find("Laser Pointer");
        LazerInitialization lazerInfo = laser.GetComponent<LazerInitialization>();
        Vector3 targetPosition = lazerInfo.hitPoint;
       
        directionVector = targetPosition - this.transform.position;
        this.GetComponent<Rigidbody>().AddForce(directionVector * velocity);
        cometTrail = transform.Find("CometTrail").GetComponent<ParticleSystem>();
        //reset();
    }

    /*private void reset()
    {
        shoot = false;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        //transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.position = new Vector3(-97, 10240, -840);
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        ParticleSystem exp = expObject.GetComponent<ParticleSystem>();
        //exp.Stop();
        
    }*/

    /*private void Update()
    {
        OVRInput.Update();
        LazerInitialization lazerInfo = laser.GetComponent<LazerInitialization>();
        if (GameObject.Find("leftHand").GetComponent<LeftHandSelector>().selectedWeapon != "Comet") return;
        if (OVRInput.Get(OVRInput.RawButton.Y)&&(!shoot))
        {
            if(lazerInfo.isHit)
            {
                transform.localScale = new Vector3(20, 20, 20);
                shoot = true;
                init(lazerInfo.hitPoint);
            }
        }
    }*/

    /*private void init(Vector3 targetPosition)
    {    
        var temp = GameObject.Find("OVRCameraRig").transform.position;
        transform.position = new Vector3(temp.x + Random.Range(-850, 850), temp.y + 1024, temp.z + Random.Range(-850, 850));
        endOfLifeCycle = false;
        currentPosition = transform.position;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        comet.GetComponent<Rigidbody>().isKinematic = false;

        directionVector = targetPosition - currentPosition;
        this.GetComponent<Rigidbody>().AddForce(directionVector * velocity);
        cometTrail = transform.Find("CometTrail").GetComponent<ParticleSystem>();
        cometTrail.Play();
        ParticleSystem.MainModule ps = cometTrail.main;
        ps.loop = true;
    }*/

    private void Explode(Vector3 impactPos) {
        expObject.transform.position = impactPos;
        ParticleSystem exp = expObject.GetComponent<ParticleSystem>();
        exp.Play();
    }

    private void repelAllBuildings(Vector3 impactPos) {

        Collider[] hitColliders = Physics.OverlapSphere(impactPos, IMPACT_RADIUS);
        Debug.Log(hitColliders.Length);
        foreach (Collider col in hitColliders) {
            Rigidbody objectBody = col.gameObject.GetComponent<Rigidbody>();

            float power = MAX_POWER;
            try
            {
                objectBody.AddExplosionForce(power, impactPos, IMPACT_RADIUS, -5.0f, ForceMode.Impulse);
            }
            catch
            { }
        }
    }

    private void despawnComet() {
        comet.GetComponent<MeshRenderer>().enabled = false;
        comet.GetComponent<SphereCollider>().enabled = false;
        comet.GetComponent<Rigidbody>().isKinematic = true;
        ParticleSystem.MainModule ps = cometTrail.main;
        ps.loop = false;
        Destroy(comet, 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Can we kill the thing off?");
        if (endOfLifeCycle)
            return;

        endOfLifeCycle = true;
        Vector3 impactPosition = transform.position;
        Explode(impactPosition);

        repelAllBuildings(impactPosition);

        despawnComet();
        //Invoke("reset", 5);
        //Stop trail animation
        /*ParticleSystem.MainModule ps = cometTrail.main;
        ps.loop = false;*/
    }

}
