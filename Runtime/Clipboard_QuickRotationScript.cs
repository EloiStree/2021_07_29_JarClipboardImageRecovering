using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clipboard_QuickRotationScript : MonoBehaviour
{
    public Vector3 m_rotationSpeed= Vector3.one;
    public float m_speed=90;

    void Update()
    {
        transform.Rotate(m_rotationSpeed, Time.deltaTime* m_speed);
        
    }
}
