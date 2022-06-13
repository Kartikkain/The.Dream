using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile :MonoBehaviour
{
   public float HorizontalSpeed = 3f;
   protected Rigidbody2D rb;
    protected Transform target;
    protected float Cal = 3f;
    Vector2 originascale;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        originascale = transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector2 targetPos = (target.position - transform.position).normalized * HorizontalSpeed;
        rb.velocity = new Vector2(targetPos.x, 0);
        if (targetPos.x > 0)
        {
            transform.localScale = new Vector2(originascale.x, originascale.y);
        }
        else
        {
            transform.localScale = new Vector2(-originascale.x, originascale.y);

        }
    }

    

    private void OnTriggerEnter2D(Collider2D Othercollision)
    {
        GameObject PlayerObj = Othercollision.gameObject;
        if(PlayerObj.GetComponent<Player>())
        {
            PlayerObj.GetComponent<Player>().playerSize(Cal);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
