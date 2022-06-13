using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saving;

public class Health : MonoBehaviour,ISaveable
{
    public enum HealthType{Enemies,Boss,Breakable};
    public HealthType Type;
    [System.NonSerialized] public bool recovered = true;
    public float AssignHealth;
    [SerializeField] float Calories = 5f;
    [SerializeField] float Amplitude = 3f;
    [SerializeField] float Time = 0.1f;
    [SerializeField] GameObject SpawnObject;
    [System.NonSerialized]public float MaxHealth;
    Animator animator;
    Enemy enemy;
    Boss boss;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        boss = GetComponent<Boss>();
        MaxHealth = AssignHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float Damage,float Launch)
    {
       
      CameraEffect.instance.CameraShake(Amplitude,Time);
      if(Type== HealthType.Enemies)
      {
         animator.SetTrigger("EnemyHurt");
         recovered = false;
         enemy.targetVelocity.x = Launch;
         MaxHealth -= Damage;
       
         if (MaxHealth <= 0)
         {
                Instantiate(SpawnObject, transform.position, Quaternion.identity);
                Die();
            

         }
            StartCoroutine(Hurt());
      }

      else if (Type == HealthType.Boss)
        {
            animator.SetTrigger("EnemyHurt");
            recovered = false;
            boss.targetVelocity.x = Launch;
            MaxHealth -= Damage;

            if (MaxHealth <= 0)
            {
                Die();
               

            }
            StartCoroutine(Hurt());
        }
        else
        {
            MaxHealth -= Damage;

            if (MaxHealth <= 0)
            {
               
                Instantiate(SpawnObject, transform.position, Quaternion.identity);
                Die();
              
            }
        }
    }

    private void Die()
    {
        if (Type == HealthType.Enemies)
        {
        GetComponent<SpriteRenderer>().enabled = false;
            if (GetComponent<CapsuleCollider2D>())
            {
                GetComponent<CapsuleCollider2D>().enabled = false;
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        
        animator.enabled = false;
        GetComponent<Enemy>().enabled = false;
        }
        else if (Type == HealthType.Boss)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            animator.enabled = false;
            GetComponent<Boss>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    IEnumerator Hurt()
    {
        yield return new WaitForSeconds(0.5f);
        recovered = true;
    }

    private void OnTriggerEnter2D(Collider2D othercollision)
    {
        if (Type == HealthType.Enemies)
        {
            GameObject other = othercollision.gameObject;
            if (other.GetComponent<Player>())
            {
                other.GetComponent<Player>().playerSize(Calories);
                Die();
            }
        }
    }
    public object CaptureState()
    {
        Dictionary<string, object> EnemyData = new Dictionary<string, object>();
        EnemyData["EnemyPosition"] = new SerializedVector3(transform.position);
        EnemyData["EnemyHealth"] = MaxHealth;
        return EnemyData;
    }

    public void RestoreState(object state)
    {
        Dictionary<string, object> EnemyData = (Dictionary<string,object>)state;
        transform.position = ((SerializedVector3)EnemyData["EnemyPosition"]).ToVector();
        MaxHealth = (float) EnemyData["EnemyHealth"];
        if (MaxHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
   
}
