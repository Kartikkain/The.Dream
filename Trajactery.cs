using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajactery : MonoBehaviour
{
    Transform attackPoint;
    float Height;
    Vector3 target;
   

    private void Start()
    {
       
        attackPoint = GameObject.FindGameObjectWithTag("ThrowPoint").transform;
        target = GameObject.FindGameObjectWithTag("Player").transform.position-attackPoint.position;

        Height = target.y + target.magnitude / 2;
       // float height = Height / 2;
        Height = Mathf.Max(0.01f, Height);
        float angle;
        float v0;
        float time;
       
        CalculateHeight(target, Height, out v0, out angle, out time);
        StopAllCoroutines();
        StartCoroutine(Projectile(v0, angle,time));
        
    }
    private void Update()
    {
       
    }

    
    private float QuadraticEquation(float a,float b, float c,float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }
    private void CalculateHeight(Vector3 target,float h,out float v0,out float angle,out float time)
    {
        float Xtarget = target.x;
        float Ytarget = target.y;
        float g = -Physics2D.gravity.y;
        float H = h / 2;
        float b = Mathf.Sqrt(2 * g * H);
        float a = (-0.5f * g);
        float c = -Ytarget;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = tplus > tmin ? tplus : tmin;

        angle = Mathf.Atan(b*time/Xtarget);
        v0 = b / Mathf.Sin(angle);
       
       
    }
    private void PathCalculator(Vector3 target,float angle,out float v0,out float time)
    {
        float Xtarget = target.x;
        float Ytarget = target.y;
        float g = -Physics2D.gravity.y;

        float v1 = Mathf.Pow(Xtarget, 2) * g;
        float v2 = 2 * Xtarget * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * Ytarget * Mathf.Pow(Mathf.Cos(angle), 2);

        v0 = Mathf.Sqrt(v1 / (v2 - v3));
        time = Xtarget /( v0 * Mathf.Cos(angle));
        if(v0 <= Mathf.Epsilon) { return; }
        if (time <= Mathf.Epsilon) { return; }
        

    }
    IEnumerator Projectile(float v0, float angle,float time)
    {
        float t = 0;
        while(t<(time+5f))
        {
            float x0 = v0 * t * Mathf.Cos(angle);
            float y0 = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics2D.gravity.y * Mathf.Pow(t,2);
            transform.position = attackPoint.position + new Vector3(x0, y0,0);
            t += Time.deltaTime;
            yield return null;
        }
        
          
        
    }


    
}
