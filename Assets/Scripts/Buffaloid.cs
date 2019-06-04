using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class Buffaloid : MonoBehaviour
{

    //public float maxSpeed;
    public float acceleration;
    public float rotationSpeed;
    public float separation_radius;
    public float edge_separation;
    public float friend_radius;
    public float forwardVelocity;

    //boid behavior weights
    public float avoidObstacleWeight;
    public float avoidFriendsWeight;
    public float alignRiderWeight;
    public float alignBoidWeight;
    public float cohesionRiderWeight;
    public float cohesionBoidWeight;
    public float chargeSpeed;

    public float matchSpeedDotAngle;

    //state stuff
    //public bool switchState = false;
    public StateMachine<Buffaloid> stateMachine { get; set; }
    public Vector2 currentMove;
    public Vector2 friendDir;

    private Rigidbody2D rb;
    private Vector2 move;
    private List<string> obstacleTags;
    private List<string> friendTags;
    private List<string> boidTags;
    private List<string> riderTags;


    private float friendSpeed;
    private BuffaloDebugger debugger;
    //private List<Collider2D> current_colliders;
    private List<Vector2> closestColliderPoints;
    private List<Collider2D> myColliders;
    private Rect avoidBounds;
    private Vector2 rightSide;
    private Vector2 leftSide;
    private Vector2 frontLeft;
    private Vector2 frontRight;

    private Vector2 cohesionDir;
    private Vector2 alignmentDir;
    private Vector2 avoidObstacleDir;
    private Vector2 avoidFriendsDir;


    private List<GameObject> players;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      
        obstacleTags = new List<string>();
        obstacleTags.Add("Obstacle");

        friendTags = new List<string>();
        friendTags.Add("Boids");
        friendTags.Add("Rider1");
        friendTags.Add("Rider2");


        boidTags = new List<string>();
        boidTags.Add("Boids");

        riderTags = new List<string>();
        riderTags.Add("Rider1");
        riderTags.Add("Rider2");

        players = new List<GameObject>();
        players.Add(GameObject.Find("Player1"));
        players.Add(GameObject.Find("Player2"));

        myColliders = new List<Collider2D>();
        myColliders.Add(GetComponent<PolygonCollider2D>());
        myColliders.Add(GetComponent<BoxCollider2D>());
        myColliders.Add(GetComponent<CircleCollider2D>());

        //forwardVelocity = 0f;
        GameObject debuggerPanel = GameObject.Find("AvoidDebuggerPanel");
        if (debuggerPanel)
        {
            debugger = debuggerPanel.GetComponent<BuffaloDebugger>();
        }
        //state initialization
        stateMachine = new StateMachine<Buffaloid>(this);
        stateMachine.ChangeState(new IdleState());

        InvokeRepeating("GetCurrentMove", 1f, 0.2f);
    }

    List<Vector2> getClosestPoints(Collider2D col, List<string> avoidTags)
    { 
        Collider2D[] hitColliders = new Collider2D[20];
        Physics2D.OverlapCollider(col, new ContactFilter2D(), hitColliders);

        //Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, separation_radius);

        //List<Collider2D> avoidColliders = new List<Collider2D>();
        //current_colliders = avoidColliders;
        List<Vector2> closest = new List<Vector2>();

        //list of my colliders


        //creates new list of colliders that contain avoid tags
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] == null)
            {
                break;
            }

            if (avoidTags.Contains(hitColliders[i].tag) && !myColliders.Contains(hitColliders[i]))
            {
                Collider2D currentCol = hitColliders[i];
                //avoidColliders.Add(currentCol);
                closest.Add(currentCol.ClosestPoint(transform.position + transform.up * 0.15f));
            }
        }
        closestColliderPoints = closest;


        return closest;
    }

    //gets points nearest to head of buffalo that are also within box collider
    Vector2 getNearestPoint(List<Vector2> closestPoints)
    {

        Vector2 closestVec = closestPoints[0];
        float shortestDist = Vector2.Distance(transform.position + transform.up * 0.15f, closestVec);

        foreach (Vector2 vec in closestPoints)
        {
            if(gameObject.GetComponent<BoxCollider2D>().bounds.Contains(vec))
            {
                float newDist = Vector2.Distance(transform.position + transform.up * 0.15f, vec);
                if (newDist < shortestDist)
                {
                    closestVec = vec;
                    shortestDist = newDist;
                }
            }
        }

        return closestVec;
    }
    //returns vector pointing away from average concentration of nearby objects,
    //or a zero vector if no nearby objects
    Vector2 getAvoid(Collider2D col, List<string> avoidTags)
    {

        List<Vector2> closestPoints = getClosestPoints(col, avoidTags);
        Vector2 anchorPoint = transform.position + (transform.up * 0.15f);


        if (debugger && col == gameObject.GetComponent<CircleCollider2D>())
        {
            debugger.SetColliderText(closestPoints.Count);
        }
        //if more nearby objects than self
        if (closestPoints.Count > 0)
        {
            Vector2 nearestPoint = getNearestPoint(closestPoints);
            Debug.DrawRay(anchorPoint, nearestPoint - anchorPoint, Color.cyan);

            float dot = Vector2.Dot(transform.right, nearestPoint - anchorPoint );

            //get inverted vector relative to self
            if(dot > 0)
            {
                return transform.right * -1;
            }
            else
            {
                return transform.right;
            }
            //Vector2 averageInverted = ((Vector2)transform.position - average) + (Vector2)transform.position;
            //Debug.DrawRay(ugtransform.position, averageInverted, Color.red);
        }
        else
        {
            //return transform.position;
            return Vector2.zero;
        }
    }

    //get nearby buffaloids in pack
    List<GameObject> getFriends() 
    {
        Collider2D[] friendColliders = Physics2D.OverlapCircleAll(transform.position, friend_radius);
        List<GameObject> friends = new List<GameObject>();

        for (int i = 0; i < friendColliders.Length; i++)
        {
            string friendTag = friendColliders[i].gameObject.tag;
            if ((friendTag == "Boids" || friendTag == "Rider1" || friendTag == "Rider2") && !myColliders.Contains(friendColliders[i]))
            //if( friendColliders[i].gameObject.transform.root != transform)
            {
                friends.Add(friendColliders[i].gameObject);
            }
        }
        return friends;
    }

    //returns average speed of list of friend objects. 
    float getFriendSpeed(List<GameObject> friends) 
    {

        float totalSpeed = 0f;
        int numFriends = friends.Count;

        if(numFriends == 0)
        {
            return 0f;
        }

        foreach (GameObject friend in friends)
        {
            totalSpeed += friend.GetComponent<Rigidbody2D>().velocity.magnitude;
        }

        return totalSpeed / numFriends;
    }

    private Vector2 getAlignment(List<GameObject> friends, List<string> tags )
    {
        if(friends.Count == 0)
        {
            return Vector2.zero;
        }

        List<Transform> positions = new List<Transform>();
        for (int i = 0; i < friends.Count; i++)
        {
            if(tags.Contains(friends[i].tag))
            {
                positions.Add(friends[i].gameObject.transform);
            }
        }

        Vector2 avg_dir = GetMeanDir(positions);
        return avg_dir;
    }

    private Vector2 GetMeanDir(List<Transform> transforms)
    {
        if (transforms.Count == 0)
            return Vector2.zero;

        Vector2 avg_vec = Vector2.zero;
        foreach (Transform pos in transforms)
        {
            avg_vec += (Vector2)pos.up;
        }
        return avg_vec.normalized;
    }

    //gets average position of a list of transforms
    private Vector2 GetMeanPos(List<Transform> transforms)
    {
        if (transforms.Count == 0)
            return Vector2.zero;

        float x = 0f;
        float y = 0f;

        foreach (Transform pos in transforms)
        {
            x += pos.position.x;
            y += pos.position.y;
 
        }
        return new Vector2(x / transforms.Count, y / transforms.Count);
    }

    //returns vector pointing towards center of pack of friends
    Vector2 getCohesion(List<GameObject> friends, List<string> tags)
    {
        if (friends.Count > 0)
        {
            //Debug.Log("in pack");
            List<Transform> positions = new List<Transform>();
            for(int i = 0; i < friends.Count; i++ )
            {
                if(tags.Contains(friends[i].tag))
                {
                    positions.Add(friends[i].gameObject.transform);
                }
            }

           //Debug.Log("num positions: " + positions.Count);
            //Debug.Log("first pos: " + positions[0]);

            Vector2 avg = GetMeanPos(positions);
            //Debug.Log("calc avg: " + avg);

            Vector2 relativeAvg = avg - (Vector2)transform.position;
            //Debug.DrawRay(transform.position, avg - (Vector2)transform.position, Color.green);

            return relativeAvg.normalized;
        }
        else
        {
            return Vector2.zero;
        }
    }

    //moves object towards vector, speed multiplier depends on state of buffaloid
    public void moveObject(Vector2 mv, float targetSpeed)
    {
        //float step = maxSpeed * Time.deltaTime;
        //Debug.Log("accel: " + acceleration);
        //Debug.Log("forwardVelocity: " + forwardVelocity);
        //Debug.DrawRay(transform.position, mv.normalized*2, Color.blue);

        //if buffaloid is moving to a point
        if (mv != Vector2.zero)
        {
            float rotSpeedRatio = forceRotate(mv);
            //Debug.Log("rotspeedratio: " + rotSpeedRatio);
            //if( rotSpeedRatio < .6)
            //{
            //    rotSpeedRatio = 0.6f;
            //}
            ForceAccelerate(rotSpeedRatio, targetSpeed);
        }
        //decelerate
    }

    float forceRotate(Vector2 mv)
    {
        float currRot = transform.eulerAngles.z;
        //Debug.Log("currRot: " + currRot);

        if (currRot > 180f)
        {
            currRot -= 360.0f;
        }
        float targetDegree = -1 * Vector2.SignedAngle(mv, transform.up);

        //Debug.Log("currRot: " + currRot);
        //Debug.Log("targetDegree: " + targetDegree);
        float horizontalAxis = 0f;
        if (targetDegree < 0)
        {
            horizontalAxis = -1;
        }
        else if (targetDegree > 0)
        {
            horizontalAxis = 1;
        }

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        float mvDegree = -1 * Vector2.SignedAngle(mv, Vector2.up);
        //Debug.Log("direction: " + direction);
        //Debug.Log("targetDegree: " + targetDegree);
        //Debug.Log("currRot: " + currRot);
        //Debug.Log("mvDegree: " + mvDegree);


        float rotationUnit = horizontalAxis * rotationSpeed * (rb.velocity.magnitude / 2.0f);
        //Debug.Log("rotationUnit: " + rotationUnit);
        //Debug.Log("diffRot: " + Mathf.Abs(currRot - targetDegree));

        if (direction >= 0.0f)
        {
            if (Mathf.Abs(currRot - mvDegree) < rotationUnit)
            {
                //Debug.Log("within turn distance");
                rb.rotation = mvDegree;
            }
            else
            {
                rb.rotation += rotationUnit;
            }
            //rb.AddTorque((horizontalAxis * rotationSpeed) * (rb.velocity.magnitude / 10.0f));
        }
        else
        {


            if (Mathf.Abs(currRot - mvDegree) < rotationUnit)
            {
                //Debug.Log("within turn distance");

                rb.rotation = mvDegree;
            }
            else
            {
                rb.rotation -= rotationUnit;
            }
            //rb.AddTorque((-horizontalAxis * rotationSpeed) * (rb.velocity.magnitude / 10.0f));
        }

        addDriftForce();

        //move speed based on rotation
        //Debug.Log("rotDistanceAngle " + rotDistanceAngle);
        //velocity handling
        float rotSpeedRatio = (1 - (Mathf.Abs(targetDegree) / 180));

        return rotSpeedRatio;
    }

    void addDriftForce()
    {
        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;

        //Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(rightAngleFromForward), Color.black);

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);

        //Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(relativeForce), Color.grey);

        rb.AddForce(rb.GetRelativeVector(relativeForce));
    }

    //acelerates this buffaloid game object with rigidbody forces, moving it in a forward direction
    void ForceAccelerate(float rotSpeedRatio, float targetSpeed)
    {
        Vector2 speed = transform.up * acceleration;

        Debug.Log(name + "- current speed: " + rb.velocity.magnitude);
        Debug.Log(name + "- friend speed: " + friendSpeed);
        Debug.Log(name + "- target speed: " + targetSpeed);

        //if ( rb.velocity.magnitude < targetSpeed || (friendSpeed > targetSpeed + .15f && rb.velocity.magnitude < friendSpeed + 0.4f &) )
        if ( rb.velocity.magnitude < targetSpeed || 
            (friendSpeed > targetSpeed + .1f && Vector2.Dot(transform.up, friendDir.normalized ) > matchSpeedDotAngle && rb.velocity.magnitude < friendSpeed + 0.15f ) )
        {
            rb.AddForce(speed);
        }
    }

    public float getRBSpeed()
    {
        return rb.velocity.magnitude;
    }

    //rotates object in stuck state
    public void torqueRotate(float mag, float dir)
    {
        //Debug.Log("torqueRotate call...");
        rb.AddTorque(mag * dir);
    }

    //reverse movement called from stuck state
    public void reverseMove(float speed )
    {

        Vector2 backwardsForce = transform.up * -1;
        Debug.DrawRay(transform.position, backwardsForce, Color.yellow);
        rb.AddForce(backwardsForce * speed);
    }

    public void preyCheck()
    {
        foreach (GameObject player in players)
        {
            if (player && player.activeInHierarchy)
            {
                var heading = player.transform.position - transform.position;
                var distance = heading.magnitude;
                /*Debug.Log("distance: " + distance);
                Debug.Log("heading: " + heading);
                Debug.Log("forward: " + _owner.transform.forward);*/

                float dot = Vector2.Dot(heading.normalized, transform.up);
                //Debug.Log("dot: " + dot);

                if (dot > 0.85f && distance < 5f)
                {
                    stateMachine.ChangeState(new ChaseState(player));
                }
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        if(closestColliderPoints != null && closestColliderPoints.Count > 0)
        {
            /*
            Gizmos.color = Color.magenta;
            foreach (Collider2D c in current_colliders)
            {
                Gizmos.DrawSphere(c.gameObject.transform.position, 0.2f);
            }*/
            Gizmos.color = Color.cyan;
            foreach (Vector2 v in closestColliderPoints )
            {
                Gizmos.DrawSphere(v, 0.05f);
            }
        }
    }

    void GetCurrentMove()
    {
        avoidObstacleDir = getAvoid(gameObject.GetComponent<BoxCollider2D>(), obstacleTags);
        avoidFriendsDir = getAvoid(gameObject.GetComponent<CircleCollider2D>(), friendTags);

        //Vector2 edgeAvoidDir = getAvoidEdges();

        List<GameObject> friends = getFriends();
        friendSpeed = getFriendSpeed(friends);

        Vector2 cohesionBoidDir = getCohesion(friends, boidTags);
        Vector2 cohesionRiderDir = getCohesion(friends, riderTags);
        Vector2 alignmentBoidDir = getAlignment(friends, boidTags);
        Vector2 alignmentRiderDir = getAlignment(friends, riderTags);


        //Debug.Log("avoid dir: " + avoidDir );
        //Debug.Log("edge avoid dir: " + edgeAvoidDir);
        //Debug.Log("cohesion dir: " + cohesionDir);
        cohesionDir = ((cohesionBoidDir * cohesionBoidWeight) + (cohesionRiderDir * cohesionRiderWeight)) / 2;
        alignmentDir = ((alignmentBoidDir * alignBoidWeight) + (alignmentRiderDir * alignRiderWeight)) / 2;

       


        //move = (avoidWeight * avoidDir) + (avoidEdgeWeight * edgeAvoidDir) 
        //+ (cohesionWeight * cohesionDir) + (alignWeight * alignmentDir);
        //move = avoidDir + edgeAvoidDir;
        move = (avoidObstacleWeight * avoidObstacleDir) + (avoidFriendsWeight * avoidFriendsDir) + cohesionDir + alignmentDir;

        currentMove = move;
        friendDir = cohesionDir;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, alignmentDir, Color.white);
        Debug.DrawRay(transform.position, avoidObstacleDir, Color.red);
        Debug.DrawRay(transform.position, avoidFriendsDir, Color.yellow);
        Debug.DrawRay(transform.position, cohesionDir.normalized, Color.green);

        if (debugger != null)
        {
            debugger.SetStateText(stateMachine.currentState);
        }

        stateMachine.Update();
    }
}
