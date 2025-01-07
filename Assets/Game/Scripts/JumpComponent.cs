using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    [SerializeField] float _force;
    [SerializeField] Rigidbody2D _rigidbody;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.AddForce(Vector3.up * _force);
        }
    }
}
