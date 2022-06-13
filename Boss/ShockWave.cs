using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField] float cal;
    float HorizontalSpeed=20f;
    Rigidbody2D rb;
    protected Transform target;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }
    // Start is called before the first frame update
    void Start()
    {
        Vector2 targetPos = (target.position - transform.position).normalized * HorizontalSpeed;

        rb.velocity = new Vector2(targetPos.x, 0f);
    }

    private void OnParticleCollision(GameObject other)
    {
        Player.instance.playerSize(cal);
    }
}
