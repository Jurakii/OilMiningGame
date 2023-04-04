using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 axis = Vector3.right;
    public float speed = 0f;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis * speed * Time.deltaTime);
    }
}
