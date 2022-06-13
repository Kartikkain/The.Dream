using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    protected float lenght;
    protected float startPosX;
    protected float startPosY;
    public GameObject cam;
    [Range(0,1)] public float parallax;
    [Range(-1,1)] public float parallay;
    
    void Start()
    {
        startPosX= transform.position.x;
        startPosY = transform.position.y;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        
    }

    private void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallax));
        float distance = (cam.transform.position.x * parallax);
        float distanceY = cam.transform.position.y*parallay;
        transform.position = new Vector3(startPosX + distance,startPosY + distanceY, transform.position.z);
        
        if(temp > startPosX + lenght) 
        {
            startPosX += lenght;
            
        }
        else if(temp < startPosX - lenght)
        {
            startPosX -= lenght;
            
        }
    }
  
}
