using Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PData;

public class Player : PhysicsObject,ISaveable
{
    public static Player instance { get; private set; }


    [Header("GameObject References")]               // GAMEOBJECT REFRENCE
    public GameObject Fireball;
    public GameObject Caloriebar;
    public GameObject Attackpoint;
    public GameObject moveablePlatform;
    public ParticleSystem RunParticles;
    public LineRenderer lineRenderer;
    [SerializeField]private ParticleSystem powerwaveVFX;


    [Header("Attack and Damage values")]           //DAMAGE AND ATTACK VALUE
    public float shoot = 50f;
    public float damage = 50f;
    protected float LazerDamage = 200f;
    protected float Absorption;
    protected float maxCal;
    public float maxcalories;
    protected float lazerLaunchpower;
    [SerializeField] float FireBallPower = 7f;
    protected float HitDirection;
    protected float hurtTime;
    protected float maxTime = 1f;
    [System.NonSerialized] public float Caloribarwidth;
    public float AttackRange = 1f;
    [SerializeField] float AssignHealth;
    float PlayerHealth;


    [Header("Movement value")]                    // MOVEMENT VALUES
    public int projectileDirection;
    public float speed = 2f;
    public float Jumpspeed = 10f;


    [Header("Vector refrence")]                   //VECTOR REFRENCE
    [SerializeField] Vector2 Size;
    [System.NonSerialized] public Vector2 orignalScale;
    protected Vector2 FireballOrigin;
    protected Vector2 playerToPlatformDifference;
    protected Vector2 platformPos;
    Vector3 LazerDirection;
    Vector3 currentDirection;


    [Header("Bool reference")]                    //BOOL REFERENCE
    [System.NonSerialized] public bool isActive;
    [System.NonSerialized] public bool IsGrabableOn;
    protected bool playerHurt;
    protected bool IsFat;

    [Header("LayerMask's")]                       //LAYER MASK'S
    public LayerMask playerlayer;
    [SerializeField] LayerMask Box;

                                                 
    [Header("ScriptableObject's")]               //SCRIPTABLEOBJECT'S
    [SerializeField] PlayerData player_Data;
    

          //CONFIGURATION

    [SerializeField] AttackEnabler Attackenabler;
    Animator animator;
    GameObject DamageCanvas;
    Animator damageCanvasAnimator;
    Material material;
    HingeJoint2D joint;
    
   

   
   

    private void Awake()
    {
        instance = this;
        maxCal = maxcalories;
        PlayerHealth = AssignHealth;
        DamageCanvas = GameObject.FindGameObjectWithTag("DamageCanvas");
        damageCanvasAnimator = DamageCanvas.GetComponent<Animator>();
    }

    
    void Start()
    {
        //Getting the component 

      
        animator = GetComponent<Animator>();
        joint = GetComponent<HingeJoint2D>();

        //Setting the value
       
        LazerDirection = Attackpoint.transform.right;
        FireballOrigin = Attackpoint.transform.localScale;
        orignalScale =gameObject.transform.localScale;
        hurtTime = maxTime;
    }

    private void Update()
    {
        
        ComputeVelocity();
        CaloriBar(Absorption);
        HealthBar();
        MakePlayerFat();
        MakePlayerSlim(); 
        
       
    }

    protected  void ComputeVelocity()
    {
        
        isActive = true;
        Connection();
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        if(move.x>0 )
        {
            if (!IsGrabableOn)
            {
                transform.localScale = new Vector2(orignalScale.x, orignalScale.y);
                Attackpoint.transform.localScale = new Vector2(FireballOrigin.x, FireballOrigin.y);
                HitDirection = shoot;
            }
            if (IsFat)
            {

            animator.SetBool("FatRun", true);
            }
            else
            {

            animator.SetBool("Run", true);
            }
            if (Grounded)
            {

            GenerateRunParticles();
            }
            
        }
        else if(move.x<0)
        {
            if (IsFat)
            {

                animator.SetBool("FatRun", true);
            }
            else
            {

                animator.SetBool("Run", true);
            }
            
            if (Grounded)
            {

            GenerateRunParticles();
            }

            if (!IsGrabableOn)
            {
                transform.localScale = new Vector2(-orignalScale.x, orignalScale.y);
                Attackpoint.transform.localScale = new Vector2(-FireballOrigin.x, FireballOrigin.y);
                HitDirection = -shoot;
            }
            
        }
        else
        {
            if (IsFat)
            {

                animator.SetBool("FatRun", false);
            }
            else
            {

                animator.SetBool("Run", false);
            }
            
        }

        if(Input.GetButtonDown("Jump") && Grounded && !IsGrabableOn)
        {
            GenerateRunParticles();
            if (IsFat)
            {

            animator.SetTrigger("FatJump");
            }
            else
            {

            animator.SetTrigger("Jump");
            }
           
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            if (IsGrabableOn)
            {
                GrabableOff();

            }
            else
            {
                GrabableOn();
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (Grounded && !IsGrabableOn)
            {
                if (IsFat) 
                {
            
                    animator.SetTrigger("FatAttack");
                }
                else
                {

                    animator.SetTrigger("Attack");
                }
            }   
           
        }
        if(Input.GetButtonUp("Fire1"))
        {
            return;
        }

        if(Input.GetButtonDown("Fire2"))
        {
            if (Attackenabler == null || Attackenabler.name=="NoPower") return ;
            if (Absorption > 0)
            {


                if (Grounded && !IsGrabableOn)
                {
                    if (IsFat)
                    {

                        animator.SetTrigger("Fatfireball");
                    }
                    else
                    {

                        animator.SetTrigger("fireball");
                    }

                }
            }
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (Attackenabler == null || Attackenabler.name == "NoPower") return;
            if (Absorption >= (maxCal / 4))
            {


                if (Grounded && !IsGrabableOn)
                {
                    if (IsFat)
                    {

                        animator.SetTrigger("FatPowerWave");
                    }
                    else
                    {

                        animator.SetTrigger("PowerWave");
                    }

                }
            }
            else
            {
                return;
            }

        }

     
        if (playerHurt)
        {
            targetVelocity = move * 0;
            hurtTime -= Time.deltaTime;
            if (hurtTime < 0)
            {
                playerHurt = false;
                hurtTime = maxTime;
            }
        }
        else
        {
        targetVelocity = move * speed;

        }

    }

   private void fireBallThrow()
    {
            Instantiate(Fireball, Attackpoint.transform.position, Attackpoint.transform.rotation);
            CutSomeFat(FireBallPower);

    }

    private void PowerWave()
    {
         StartCoroutine(Shoot());
    }

    private void powerWaveWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;    

    }

  

   

    IEnumerator Shoot()
    {
        if (Attackpoint.transform.localScale.x < 0)
        {
            currentDirection = -LazerDirection;
        }
        else
        {
            currentDirection = LazerDirection;
        }
        Vector3 originpoint = new Vector3(Attackpoint.transform.position.x, Attackpoint.transform.position.y, transform.position.z);
        
        RaycastHit2D hitInfo = Physics2D.Raycast(originpoint,currentDirection);
        if(hitInfo)
        {
            Health enemy = hitInfo.transform.GetComponent<Health>();
            if (enemy != null)
            {
                enemy.Hit(LazerDamage,lazerLaunchpower);
                
            }
            Debug.Log(hitInfo.transform.name);
            Vector3 hitorigin = new Vector3(hitInfo.point.x, hitInfo.point.y, transform.position.z);
            lineRenderer.SetPosition(0, originpoint);
            lineRenderer.SetPosition(1, hitorigin);
            powerwaveVFX.transform.position = hitorigin;
            powerwaveVFX.Play();

        }
        else
        {
            lineRenderer.SetPosition(0, originpoint);
            lineRenderer.SetPosition(1, originpoint + currentDirection * 100);
        }
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.75f);
        lineRenderer.enabled = false;
    }
    private void Attack()
    {
        

        Collider2D[] Punch = Physics2D.OverlapCircleAll(Attackpoint.transform.position, AttackRange, playerlayer);
       foreach(Collider2D enemies in Punch)
       {

           
            enemies.GetComponent<Health>().Hit(damage,HitDirection);

       }
        
    }

    public void jump(float jumpPower)
    {
        velocity.y = jumpPower; 
    }

    public void EnableFireBall(AttackEnabler enabler)
    {
        Attackenabler = enabler;
    }
    private void Connection()
    {
        Collider2D box = Physics2D.OverlapBox(Attackpoint.transform.position, Size,0,Box);
        if (box)
        {
            Rigidbody2D Boxrb = box.GetComponent<Rigidbody2D>();
            joint.connectedBody = Boxrb;
            Debug.Log("Connect to" + "  " + joint.connectedBody.name);

        }
        else
        {
            GrabableOff();

        }
    }
    private void GrabableOn()
    {

        if (joint.connectedBody == null)
        {
            joint.enabled = false;
        }
        else
        {
            joint.enabled = true;
            IsGrabableOn = true;

        }

    }

    private void GrabableOff()
    {
        joint.enabled = false;
        joint.connectedBody = null;
        IsGrabableOn = false;


    }

    public  void ObstacleHandler()
    {
        Destroy(gameObject);
    }

    public void CutSomeFat(float Energy)
    {
        Absorption -= Energy;
       
        PlayerDie();
    }


    public void playerSize(float Cal)
    {
        damageCanvasAnimator.SetTrigger("Damage");
        CameraEffect.instance.CameraShake(2f, 0.1f);


        player_Data.isHurt = true;
        
        playerHurt = true;

        if (IsFat)
        {

        animator.SetTrigger("FatHurt");
        }
        else
        {

        animator.SetTrigger("Hurt");
        }

        Absorption += Cal;

        
       
      
        if(Absorption>=maxCal)
        {
            FindObjectOfType<LoosScreenHandler>().YouLoose();
            
        }
    }

    private void PlayerDie()
    {
        if(Absorption < 0)
        {
            FindObjectOfType<LoosScreenHandler>().YouLoose();

        }
    }
    private void MakePlayerSlim()
    {
        if (Absorption < (maxCal / 2))
        {
            IsFat = false;
            animator.SetBool("Fat", false);
        }
    }
    private void MakePlayerFat()
    {
        if (Absorption >= (maxCal / 2))
        {

            IsFat = true;
            animator.SetBool("Fat", true);
            
        }
    }

    public void PlayerHealthDamage(float Damage)
    {
        damageCanvasAnimator.SetTrigger("Damage");
        CameraEffect.instance.CameraShake(2f, 0.1f);


        playerHurt = true;


        if (IsFat)
        {

            animator.SetTrigger("FatHurt");
        }
        else
        {

            animator.SetTrigger("Hurt");
        }

        
        PlayerHealth -= Damage;
        if (PlayerHealth <= 0)
        {
            FindObjectOfType<LoosScreenHandler>().YouLoose();

        }
    } 
    private void CaloriBar(float Absorbcalories)
    {
        player_Data.PCalories = Absorption / maxcalories;
      
    }
  
    private void HealthBar()
    {
        player_Data.PHealth=PlayerHealth/AssignHealth;
    }
    private void GenerateRunParticles()
    {
        RunParticles.Play();
    }


    public object CaptureState()
    {
        Dictionary<string, object> playerData = new Dictionary<string, object>();
       
            playerData["Position"] = new SerializedVector3(transform.position);
            playerData["Power"] = Attackenabler.name;
            playerData["Calories"] = Absorption; 

        return playerData;
    }

    public void RestoreState(object state)
    {
        Dictionary<string, object> playerData = (Dictionary<string, object>)state;
       

         transform.position = ((SerializedVector3)playerData["Position"]).ToVector();
         string PowerName = (string)playerData["Power"];
         AttackEnabler PowerEnabler = Resources.Load<AttackEnabler>(PowerName);
         Absorption = (float)playerData["Calories"];
        EnableFireBall(PowerEnabler);
       
       
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Attackpoint.transform.position, AttackRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Attackpoint.transform.position,Size);

    }
}
