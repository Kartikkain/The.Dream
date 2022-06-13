using System.Collections;
using System.Collections.Generic;
using Mover;
using UnityEngine;




public class Enemy : PhysicsObject
{
    public enum EnemyType {Walkable , jumpable, Still};
    [SerializeField] EnemyType enemyType;
    [SerializeField] float ChaseRange = 4f;
    [SerializeField] float AttackRange = 3f;
    [SerializeField] float CombatRange = 2f;
    [SerializeField] float ChaseSpeed = 2f;
    [SerializeField] float JumpSpeed = 2f;
    Transform Target;
    Movement move;
    Health health;
   
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        move = GetComponent<Movement>();
        Target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (health.recovered)
        {

            ComputeVelocity();
        }
    }

    protected void ComputeVelocity()
    {
        Vector2 EnemyMove = Vector2.zero;
        EnemyMove.x = move.MoveDirection;
        move.MoveTowardTarget(Target);
        if (enemyType == EnemyType.Walkable)
        {

            move.Chase(ChaseRange, CombatRange);
            move.Combat(CombatRange);
            move.Shoot(AttackRange, ChaseRange);

        }

        if (enemyType == EnemyType.jumpable)
        {
            move.Chase(ChaseRange, CombatRange);
            move.Combat(CombatRange);
            move.Shoot(AttackRange, ChaseRange);
            move.JumpDetection();
            Debug.Log("Jumpable");
        }

        if(enemyType == EnemyType.Still)
        {
            move.Shoot(AttackRange, ChaseRange);
        }
        
        if (move.isFollow)
        {
        targetVelocity = EnemyMove * ChaseSpeed;
        }
        else
        {
        targetVelocity = EnemyMove * 0;
        }
        
    }

    private void Jump()
    {
        if (Grounded)
        {
        velocity.y = JumpSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CombatRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
       

    }

  
}


