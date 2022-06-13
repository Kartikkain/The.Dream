using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorDirection : MonoBehaviour
{
    [SerializeField] float RotationDirection;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(Vector3.forward * RotationDirection);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
