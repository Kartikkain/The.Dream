using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mover;

public class Boss : PhysicsObject
{
    [SerializeField] float ChaseSpeed ;
    [SerializeField] float ChaseRange;
    [SerializeField] float CombatRange;
    [SerializeField] float ShootRange;
    [SerializeField] float Transmissionrange;
    [SerializeField] float raycastDistance;
    [SerializeField] float offset;
    [SerializeField] LayerMask fireball;
    [SerializeField] LineRenderer line;
    [SerializeField]  float JumpSpeed;
    [SerializeField] GameObject shockWave;
    [SerializeField] GameObject LazerPoint;
    [SerializeField] float AssignTime;
    [SerializeField] LineRenderer bossLazer;
    [SerializeField] ParticleSystem LazerCollisionEffect;
    float currentTime;
    Vector2 currentDirection;
    Vector2 DeltaDirection;
    Vector3 dir;

    Transform Target;
    Movement Move;
    Health health;
    Animator animator;
    Material material;
    CapsuleCollider2D capsule;
    private void Awake()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        Move = GetComponent<Movement>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        currentTime = AssignTime;
        material = GetComponent<SpriteRenderer>().material;
        capsule = GetComponent<CapsuleCollider2D>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (health.recovered)
        {

            BossComputeVelocity();
        }
    }

    protected void BossComputeVelocity()
    {
        Vector2 BossMove = Vector2.zero;
        BossMove.x = Move.MoveDirection;
        Move.MoveTowardTarget(Target);
        Move.Chase(ChaseRange, CombatRange);
        Move.Combat(CombatRange);
        Move.Shoot(ShootRange, ChaseRange);
        if (transform.localScale.x < 0)
        {
            dir = Vector2.left;
            
        }
        else
        {
            dir = Vector2.right ;
            

        }
        Vector2 origin = new Vector2(transform.GetChild(0).position.x, transform.GetChild(0).position.y+offset);
        RaycastHit2D hit = Physics2D.Raycast(origin, dir , raycastDistance, fireball); ;
        if (hit)
        {
          line.SetPosition(0, origin);
          line.SetPosition(1, hit.point);
            Transmission();
            Debug.Log("Transmit");
        }
        else
        {
            line.SetPosition(0, origin);
            line.SetPosition(1,origin +(Vector2) dir * 100);
        }
        

        if (Move.isFollow)
        {
            targetVelocity = BossMove * ChaseSpeed;
        }
        else
        {
            targetVelocity = BossMove * 0;
        }

        if (health.MaxHealth <= (health.AssignHealth / 2))
        {
            Move.AnimatorChange = true;
            animator.SetBool("BossLevel2", true);
           
        }
        if(health.MaxHealth <= (health.AssignHealth / 4))
        {
            Debug.Log("quater");
            float BossToTargetDistance = Target.position.x - transform.position.x;
            if (Mathf.Abs(BossToTargetDistance) < Transmissionrange)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                animator.SetTrigger("EnemyPowerWave");
                    currentTime = AssignTime;
                }

            }
        }

        
    }
    private void Jump()
    {
        if (Grounded)
        {
            velocity.y = JumpSpeed;
        }
    }

    private void Transmission()
    {
        float BossToTargetDistance = Target.position.x - transform.position.x;
        if (Mathf.Abs(BossToTargetDistance) < Transmissionrange)
        {
        animator.SetTrigger("BossTransmission");
            //transform.position = new Vector3(Target.position.x + 2 * Mathf.Sign(BossToTargetDistance), transform.position.y,transform.position.z);
            StartCoroutine(BossTransmission());
          
        }
        else
        {
            animator.SetTrigger("Jump");

        }

    }

    private  IEnumerator BossTransmission()
    {
        capsule.enabled = false;
        gravityModifier = 0;
        material.SetFloat("_Is_Fade_Off", 0.0f);
        Zipp();
        yield return new WaitForSeconds(1f);
        material.SetFloat("_Is_Fade_Off", 1.0f);
        capsule.enabled = true;
        gravityModifier = 1;
        animator.SetTrigger("BossTransmission");
    }
    
    private void Zipp()
    {
        float BossToTargetDistance = Target.position.x - transform.position.x;
        transform.position = new Vector3(Target.position.x + 2 * Mathf.Sign(BossToTargetDistance), transform.position.y, transform.position.z);

    }
    private void ShockWave()
    {
        Instantiate(shockWave, transform.position, Quaternion.identity);
    }

    private void BossPowerWaveWidth(float width)
    {
        bossLazer.startWidth = width;
        bossLazer.endWidth = width;

    }
    private void BossPowerWave()
    {
        StartCoroutine(LazerPowerWave());
    }
    private IEnumerator LazerPowerWave()
    {
        RaycastHit2D lazerhit = Physics2D.Raycast(LazerPoint.transform.position, dir);
        if (lazerhit)
        {
            Player player = lazerhit.transform.GetComponent<Player>();
            if (player!=null)
            {
                Debug.Log("player");
            }
            Vector3 hitOrigin = new Vector3(lazerhit.point.x, lazerhit.point.y,LazerPoint.transform.position.z);
                bossLazer.SetPosition(0, LazerPoint.transform.position);
                bossLazer.SetPosition(1, hitOrigin);
            LazerCollisionEffect.transform.position = hitOrigin;
            LazerCollisionEffect.Play();
        }
        else
        {
            bossLazer.SetPosition(0, LazerPoint.transform.position);
            bossLazer.SetPosition(1, LazerPoint.transform.position + dir * 100);
        }
        bossLazer.enabled = true;
        yield return new WaitForSeconds(0.75f);
        bossLazer.enabled = false;
             
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CombatRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ShootRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Transmissionrange);


    }
}
