using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] GameObject Impact;
    [SerializeField] float Amplitude;
    [SerializeField] float time;
    protected Transform projectileOrigin;
    Rigidbody2D Prb;
    protected float FireBallPower = 7f;
    public float projectileSpeed = 7f;
    protected float launch = 0;
    protected float damage = 30f;
    Vector2 orignalScale;

    private void Awake()
    {
        orignalScale = gameObject.transform.localScale;
        projectileOrigin = GameObject.FindGameObjectWithTag("AttackPoint").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        Prb = GetComponent<Rigidbody2D>(); 
       if(projectileOrigin.localScale.x<0)
        {
            transform.localScale = new Vector2(-orignalScale.x, orignalScale.y);
        Prb.velocity =new Vector2 (-projectileSpeed,0);
          //  Impact.transform.localScale = new Vector2(-1,1);
            
        }
        else
        {
        Prb.velocity =new Vector2 (projectileSpeed,0);
          //  Impact.transform.localScale = new Vector2(1,1);

        }


    }


    private void OnTriggerEnter2D(Collider2D OtherCollider)
    {
        CameraEffect.instance.CameraShake(Amplitude, time);
        Instantiate(Impact, transform.position,Quaternion.identity) ;
        GameObject OtherObject = OtherCollider.gameObject;
        if (OtherObject.GetComponent<Health>())
        {
            OtherObject.GetComponent<Health>().Hit(damage,launch);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject,3f);
            
        }
    }
}
