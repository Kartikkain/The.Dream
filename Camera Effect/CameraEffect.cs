using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEffect : MonoBehaviour
{
    public static CameraEffect instance { get; private set; }
    protected CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin;
    protected CinemachineVirtualCamera virtualCamera;
    float ShakeTimer;
    float ShakeTimerTotal;
    float ShakeAmplitude;
   
    
    // Start is called before the first frame update
    void Awake()
    {
        //instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
       
        basicMultiChannelPerlin =virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // basicMultiChannelPerlin.m_AmplitudeGain = 0;

        
    }

    public void CameraShake(float Amplitude,float time) 
    {
        basicMultiChannelPerlin.m_AmplitudeGain = Amplitude;
        ShakeTimer = time;
        ShakeTimerTotal = time;
        ShakeAmplitude = Amplitude;
        
    }

    private void Update()
    {
        instance = this;

        if (ShakeTimer > 0)
        {


            ShakeTimer -= Time.deltaTime;
            if (ShakeTimer <= 0)
            {
   
                CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                basicMultiChannelPerlin.m_AmplitudeGain = 0;
                    // Mathf.Lerp(ShakeAmplitude, 0f, ShakeTimerTotal);
            }
           
        }

       

    }
}
