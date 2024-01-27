using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageGeneric : MonoBehaviour
{
    [SerializeField]
    LayerMask collisionLayer;
    
    float colliderRadius;

    [SerializeField]
    bool is_Player, is_Enemy;

    
    void Start()
    {
        colliderRadius = 0.8f;

        
    }

    void FixedUpdate()
    {
        DetectCollision();
    }

    void DetectCollision()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, colliderRadius, collisionLayer);
        if (hit.Length > 0)
        {
           
        }
    }
}
