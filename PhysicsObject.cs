using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier=1f;
    protected bool Grounded;
    [System.NonSerialized]public  Vector2 targetVelocity;
    protected float minGroundNormalY = 0.65f;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected RaycastHit2D[] hitbuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitbufferlist = new List<RaycastHit2D>(16);
    protected ContactFilter2D contactFilter;
    protected float minDistance = 0.001f;
    protected float shellRadius = 0.01f;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;
      
        
    }

 
   

    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;
        Grounded = false;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 move = moveAlongGround * deltaPosition;
        Movement(move, false);
         move = Vector2.up * deltaPosition;
        Movement(move,true);
    }

    void Movement(Vector2 move,bool YMovement)
    {
        float distance = move.magnitude;
            if(distance > minDistance)
        {
         int count = rb2d.Cast(move, contactFilter, hitbuffer, distance + shellRadius);
            hitbufferlist.Clear();
            for(int i=0;i<count;i++)
            {
                hitbufferlist.Add(hitbuffer[i]);
            }
            for(int i=0;i<hitbufferlist.Count;i++)
            {
                Vector2 currentNormal = hitbufferlist[i].normal;
                if(currentNormal.y>minGroundNormalY)
                {
                    Grounded = true;
                    if(YMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }

                }
                float projection = Vector2.Dot(velocity, currentNormal);
                if(projection<0)
                {
                    velocity = velocity - projection * currentNormal;
                }
                float modifiedDistance = hitbufferlist[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rb2d.position = rb2d.position + move.normalized * distance; 
    }
}
