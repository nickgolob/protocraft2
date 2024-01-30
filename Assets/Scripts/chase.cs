using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public GameObject prey;

    private readonly float _speed = 6;

    void Update()
    {
        Transform prey_transform = prey.transform;
        Vector3 delta = prey_transform.position - transform.position;
        transform.position += delta.normalized * Time.deltaTime * _speed;
    }
}
