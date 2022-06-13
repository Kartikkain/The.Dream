using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaDou : MonoBehaviour
{
    public float jumpPower = 20f;
    private void OnCollisionEnter2D(Collision2D othercollision)
    {
        GameObject other = othercollision.gameObject;
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player>().jump(jumpPower);
            Debug.Log("jump");
        }
        
    }
}
