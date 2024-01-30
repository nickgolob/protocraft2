using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class run : MonoBehaviour
{
    public GameObject predator;
    
    private float _speed = 9;

    void Update()
    {
        Vector3 delta = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
        transform.position += delta * _speed * Time.deltaTime;
    }
}
