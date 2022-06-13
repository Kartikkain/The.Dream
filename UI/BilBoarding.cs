using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilBoarding : MonoBehaviour
{
   [SerializeField]float BilboardRadius;
    [SerializeField] LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D BilboardArea = Physics2D.OverlapCircle(transform.position, BilboardRadius,playerLayer);
         GameObject Bilboarding =  transform.GetChild(0).gameObject;
        if (BilboardArea)
        {
            Bilboarding.SetActive(true);
            if (Player.instance.IsGrabableOn)
            {

            Bilboarding.SetActive(false);
            }
        }
       
        else
        {
            Bilboarding.SetActive(false);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, BilboardRadius);
    }
}
