
using UnityEngine;

namespace Mover
{
    public class Movement :MonoBehaviour
    {
        [System.NonSerialized]public float MoveDirection;
        [System.NonSerialized]public bool isFollow;
        [System.NonSerialized] public bool AnimatorChange;
        [SerializeField] ParticleSystem RunParticles;
        [SerializeField] float Timer;
        [SerializeField] float ProjectileTimer;
        [SerializeField] GameObject projectile;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float PunchRange= 0.5f;
        [SerializeField] float HurtPoint= 6f;
        [SerializeField] float lenght;
        [SerializeField] Vector2 offset;
        protected RaycastHit2D leftRay;
        protected RaycastHit2D RightRay;
        protected float MaxTime;
        protected float ProjectileMaxTime;
        protected float HalfHealth;
        float EnemyToTargetDistanceX;
        Vector2 Direction;
        Animator animator;
        Health health;
       
        
        private void Start()
        {

            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
            MaxTime = Timer;
            ProjectileMaxTime = ProjectileTimer;
            Direction = transform.localScale;
        }
        public void MoveTowardTarget(Transform Target)
        {
            if (Target == null) { return; }

            //Calculating enemy to target distance
            EnemyToTargetDistanceX = Target.position.x - transform.position.x;
           
            //Flip sprite

            if (EnemyToTargetDistanceX < 0)
            {
                transform.localScale = new Vector2(-Direction.x, Direction.y);
                

            }
            else
            {
                transform.localScale = new Vector2(Direction.x, Direction.y);
                

            }




        }

        public void Chase(float chaseRange,float combatRange)
        {
            
                if (Mathf.Abs(EnemyToTargetDistanceX) <= chaseRange && Mathf.Abs(EnemyToTargetDistanceX) > combatRange)
                {
                    animator.SetBool("EnemyRun", true);
                    RunParticles.Play();

                    isFollow = true;
                    ChaseDirection();
       
                }
                else
                {
                    animator.SetBool("EnemyRun", false);

                }
            
        }
    
       public void Shoot(float attackRange,float chaseRange)
       {
            if (Mathf.Abs(EnemyToTargetDistanceX) <= attackRange && Mathf.Abs(EnemyToTargetDistanceX) > chaseRange)
            {
                isFollow = false;

                ProjectileMaxTime -= Time.deltaTime;
                if (ProjectileMaxTime <= 0)
                {
                    if (!AnimatorChange)
                    {

                    animator.SetTrigger("EnemyShoot");
                    }
                    else
                    {
                        animator.SetTrigger("EnemyShockWave");
                    }
                    Debug.Log("fight");
                    ProjectileMaxTime = ProjectileTimer;
                }
            }
       }

        public void Combat(float combatRange)
        {
            if (Mathf.Abs(EnemyToTargetDistanceX) <= combatRange)
            {
                animator.SetBool("EnemyRun", false);

                isFollow = false;

                MaxTime -= Time.deltaTime;
                if (MaxTime <= 0)
                {
                   
                    animator.SetTrigger("EnemyAttack");
                    Debug.Log("Attack");
                    MaxTime = Timer;
                }
            }
        }


        //Chase
        private void ChaseDirection()
        {
            if (EnemyToTargetDistanceX < 0)
            {
                MoveDirection = -1;
            }
            else
            {
                MoveDirection = 1;
            }
        }


        //Check for jump
        public void JumpDetection()
        {
            RightRay = Physics2D.Raycast(new Vector2(transform.position.x + offset.x, transform.position.y + offset.y), Vector2.right, lenght, groundLayer);
            Debug.DrawRay(new Vector2(transform.position.x + offset.x, transform.position.y + offset.y), Vector2.right, Color.blue);
            if (RightRay.collider != null && isFollow)
            {
                animator.SetTrigger("EnemyJump");
                Debug.Log("right");
            }

            leftRay = Physics2D.Raycast(new Vector2(transform.position.x + offset.x, transform.position.y + offset.y), Vector2.left, lenght, groundLayer);
            Debug.DrawRay(new Vector2(transform.position.x + offset.x, transform.position.y + offset.y), Vector2.right, Color.blue);
            if (leftRay.collider != null && isFollow)
            {
                
                animator.SetTrigger("EnemyJump");
                Debug.Log("left");
            }
        }
        //Punch
        private void Punch()
        {
            Collider2D[] Fist = Physics2D.OverlapCircleAll(transform.GetChild(0).position,PunchRange, playerLayer);
            foreach (Collider2D target in Fist)
            {
                target.GetComponent<Player>().PlayerHealthDamage(HurtPoint);

            }
        }
        // Projectile 
        private void Projectile()
        {
            Instantiate(projectile, transform.GetChild(0).position, Quaternion.identity);
        }
        

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.GetChild(0).position, PunchRange);
        }
    }
}

