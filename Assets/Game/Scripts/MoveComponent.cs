using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    [SerializeField] float _speed;
   
    [SerializeField] Transform _face;
    
    void Update()
    {
        var direction = Input.GetAxis("Horizontal");
        var step = direction * _speed * Time.deltaTime;
       
        transform.position += new Vector3(step, 0, 0);

        if ( math.abs(direction) > 0 )
            transform.localScale = direction < 0 ? new Vector3(-1, 1, 1) : new Vector3( 1 , 1, 1);
    }
}
