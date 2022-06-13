using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] Camera cam;
    Vector2 starPos;
    float ZPos;
    Vector2 travel;
    float clippingPlane;
    float parallaxFactor;
    float distanceFromSubject;

    
    void Start()
    {
        starPos = transform.position;
        ZPos = transform.position.z;
    }
    private void FixedUpdate()
    {
        distanceFromSubject = transform.position.z - Player.instance.transform.position.z;
        travel = (Vector2)cam.transform.position - starPos;
        clippingPlane = (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
        parallaxFactor = Mathf.Abs(distanceFromSubject) / clippingPlane;
    }

    void Update()
    {
        Vector2 newpos = starPos + travel * parallaxFactor;
        transform.position = new Vector3(newpos.x, starPos.y, ZPos);
    }
}
