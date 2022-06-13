using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenbox : MonoBehaviour
{
    [SerializeField] float Height;
    [SerializeField] float Width;
    [SerializeField] LayerMask playerLayer;
    Collider2D frame;

    private void Update()
    {
        Vector2 Size = new Vector2(Width, Height);
        frame = Physics2D.OverlapBox(transform.position, Size, 0, playerLayer);


        for (int i = 0; i < transform.childCount; i++)
        {
            if (frame)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = frame ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }
}
