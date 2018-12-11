using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;

[ExecuteInEditMode]
public class PedestrianObject : MonoBehaviour 
{
	public  PedestrianSystem.ObjectFrequency    m_assetFrequency      = PedestrianSystem.ObjectFrequency.HIGH;

	public  PedestrianNode                      m_currentNode         = null;                 // the current node that the pedestrian object will travel to 
	public  float                               m_minSpeed            = 1000.0f;                 // the minimum speed that the pedestrian object will travel at              
	public  float                               m_maxSpeed            = 1000.0f;                 // the maximum speed that the pedestrian object will travel at 
    public  float                               runningSpeed          = 500.0f;
    [Range(0.0f, 1.0f)]
	public  float                               m_percentageOfSpeedToUse = 1.0f;              // 100% uses the entire values from min to max speed. 34% would only use from min to 35% of max speed. Set this to make objects use a certain amount of the full min / max speed value (currently up to 34% allows for walking animation only) 
	protected float                             m_speed               = 0.0f;                 // the speed that the pedestrian object will travel at            
	private float                               m_speedStoredWhileWaiting = 0.0f;             // used to record a speed when a node is telling the object to wait ( m_waitAtNode = true )            
	protected float                             m_currentSpeed        = 0.0f;                 // this is always our current speed
	public  float                               m_startMovingDelayMin = 0.0f;                 // this is the minium delay the object will wait before it starts moving
	public  float                               m_startMovingDelayMax = 0.0f;                 // this is the maxium delay the object will wait before it starts moving
	public  float                               m_rotationSpeed       = 3.5f;                 // the speed at which we will rotaion
	public  bool                                m_onlyRotYAxis        = true;                 // set to true this means the object will stand upright always
	public  float                               m_nodeThreshold       = 0.1f;                 // how close the pedestrian object needs to get to the m_currentNode before it moves on to another node

	public int                                  m_nodeVisitThreshold  = 2;                    // this is the amount of nodes to remember we have visited so we don't visit the same node again within this threshold
	private PedestrianNode[]                    m_prevPedestrianNodes;
	private int                                 m_nodeVisitIndex      = 0;

    private NavMeshAgent                        m_navMeshAgent;
    public GameObject                           disaster; 
    private float                               disaster_size_unsafe_area = 300
        ;
    private bool                                has_seen_disaster = false;

	public  PathingStatus                       m_pathingStatus       = PathingStatus.RANDOM; // this determines how the pedestrian object will traverse through the pathing nodes
	public  int                                 m_pathingIndex        = 0;                    // if the m_pathingStatus is set to INDEX, then the pedestrian object will try to use this index position for the m_nodes associated in the PedestrianNode.cs object 
	public  Vector3                             m_offsetPosVal        = Vector3.zero;         // the amount to offset the position of this object
	public  bool                                m_lookAtNode          = true;                 // set this to true if you want the objects forward direction to face the node it is currently going towards

	public  Animator                            m_animator            = null;                 // holds the animator of the object (can be null)

	protected float                             m_lanePosXVariation   = 0.0f;
	protected float                             m_lanePosZVariation   = 0.0f;
	protected float                             m_speedVariation      = 0.0f;

	private bool                                ThresholdReached     { get; set; }

	public enum PathingStatus
	{
		RANDOM = 0
	}

	void Awake () 
	{
		m_speed = UnityEngine.Random.Range(m_minSpeed, m_maxSpeed);

		m_prevPedestrianNodes = new PedestrianNode[m_nodeVisitThreshold];
		for(int nIndex = 0; nIndex < m_prevPedestrianNodes.Length; nIndex++)
			m_prevPedestrianNodes[nIndex] = null;

		if(PedestrianSystem.Instance)
			PedestrianSystem.Instance.RegisterObject( this );
	}
	
	IEnumerator Start () 
	{
        disaster = GameObject.FindGameObjectWithTag("disaster");
        m_animator = gameObject.GetComponent<Animator>();
        m_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

		if(PedestrianSystem.Instance)
		{
		    m_speedVariation    = UnityEngine.Random.Range(0.0f, PedestrianSystem.Instance.m_globalSpeedVariation);
			m_lanePosXVariation = UnityEngine.Random.Range(-PedestrianSystem.Instance.m_globalLanePosVariation, PedestrianSystem.Instance.m_globalLanePosVariation);
			m_lanePosZVariation = UnityEngine.Random.Range(-PedestrianSystem.Instance.m_globalLanePosVariation, PedestrianSystem.Instance.m_globalLanePosVariation);
		}

		DetermineSpeed( 0.0f, true ); // set idle (which is 0.0f if the object has an animator)

		yield return new WaitForSeconds(UnityEngine.Random.Range( m_startMovingDelayMin, m_startMovingDelayMax ) );

		DetermineSpeed(UnityEngine.Random.Range(m_minSpeed, m_maxSpeed) );

		yield return null;
	}

	void Update ()
	{
        Vector3 disaster_to_pedestrian_vector = (gameObject.transform.position - disaster.transform.position) * runningSpeed;
        float disaster_size = disaster.GetComponent<Renderer>().bounds.size.magnitude;
        if (disaster_to_pedestrian_vector.magnitude < disaster_size + disaster_size_unsafe_area)
            has_seen_disaster = true;



        if (!has_seen_disaster) {
            followPedestrianPaths();
        }
        else
        {
            if (disaster_to_pedestrian_vector.magnitude < disaster_size + disaster_size_unsafe_area)
            {
                has_seen_disaster = true;
                m_animator.SetInteger("Mode", 2);
                m_navMeshAgent.SetDestination(gameObject.transform.position + disaster_to_pedestrian_vector);
                m_navMeshAgent.speed = 10.0f;
            }
            else
            {
                m_animator.SetInteger("Mode", 1);
                m_navMeshAgent.SetDestination(gameObject.transform.position);
            }
        }
            

        
        
       
	}

    private void followPedestrianPaths()
    {

        if (!m_currentNode)
        {
            DetermineSpeed(0.0f, true); // idle animation as we are not walking anywhere
            return;
        }
        

        Vector3 dir = m_currentNode.transform.position;
        dir.x += m_lanePosXVariation;
        dir.z += m_lanePosZVariation;
        dir = dir - (transform.position + m_offsetPosVal); // find the direction to the next node

        Vector3 speed = dir.normalized * m_currentSpeed; // work out how fast we should travel in the desired directoin

        if (ThresholdReached)
        {
            if (m_currentNode.m_waitAtNode)
            {
                if (m_speed != 0.0f)
                    m_speedStoredWhileWaiting = m_speed;

                DetermineSpeed(0.0f, true);
            }
            else
            {
                if (m_speedStoredWhileWaiting != 0.0f)
                {
                    DetermineSpeed(m_speedStoredWhileWaiting);
                    m_speedStoredWhileWaiting = 0.0f;
                }

                m_prevPedestrianNodes[m_nodeVisitIndex] = m_currentNode;
                m_nodeVisitIndex++;
                if (m_nodeVisitIndex >= m_prevPedestrianNodes.Length)
                    m_nodeVisitIndex = 0;

                m_currentNode = m_currentNode.NextNode(this);  // find another node or do something else
                ThresholdReached = false;
            }
        }
        else if (dir.magnitude > m_nodeThreshold)
        {
            if (GetComponent<Rigidbody>())                                                                                                       // if we have a rigidbody, use the following code to move us
            {
                GetComponent<Rigidbody>().velocity = speed;  // set our rigidbody to this speed to move us by the determined speed 

                if (m_lookAtNode)
                    transform.forward = Vector3.Slerp(transform.forward, dir.normalized, m_rotationSpeed * Time.deltaTime);   // rotate our forward directoin over time to face the node we are moving towards
            }
            else                                                                                                                // no rigidbody then use the following code to move us
            {
                if (GetComponent<Collider>())                                                                                                    // it generally is a bad idea to move something with a collider, so we should tell someone about it if this is happening. See Unity Docs for more info: http://docs.unity3d.com/ScriptReference/Collider.html
                    Debug.LogWarning("Pedestrian System Warning -> Object has a collider. You should think about moving the object with a rigidbody instead.");

                transform.position += speed * Time.deltaTime;                                                                   // move us by the determined speed

                if (m_lookAtNode)
                    transform.forward = Vector3.Slerp(transform.forward, dir.normalized, m_rotationSpeed * Time.deltaTime);   // rotate our forward directoin over time to face the node we are moving towards
            }

            if (m_onlyRotYAxis)
                transform.rotation = Quaternion.Euler(new Vector3(0.0f, transform.eulerAngles.y, 0.0f)); // only rotate around the Y axis.
        }
        else
            ThresholdReached = true;
        
    }

    void FixedUpdate()
	{
		//DetermineAnimation();
	}

	void Destroy()
	{
		if(PedestrianSystem.Instance)
			PedestrianSystem.Instance.UnRegisterObject( this );
	}

	public void Spawn( Vector3 a_pos, PedestrianNode a_startNode )
	{
		transform.position = a_pos - m_offsetPosVal;
		     m_currentNode = a_startNode;
	}

	public void DetermineSpeed( float a_speed, bool a_overrideCurrentSpeed = false )
	{
        m_speed = a_speed;
		m_currentSpeed = m_speed + m_speedVariation;

		m_currentSpeed = m_currentSpeed * m_percentageOfSpeedToUse;

		if(a_overrideCurrentSpeed)
			m_currentSpeed = m_speed;
	}

	void DetermineAnimation()
	{
		if(m_animator)
		{
            //m_animator.SetInteger("Mode", 2);
            /*
            if (m_currentSpeed <= 0.0f)
				m_animator.SetInteger("Mode", 0);
			else
                m_animator.SetInteger("Mode", 1);
                */
		}
	}

	public bool HasVisitedNode( PedestrianNode a_node )
	{
		for(int nIndex = 0; nIndex < m_prevPedestrianNodes.Length; nIndex++)
		{
			if(m_prevPedestrianNodes[nIndex] == a_node)
				return true;
		}

		return false;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube((transform.position + m_offsetPosVal), Vector3.one * 0.25f);
	}
}
