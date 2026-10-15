using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidBodySleep : MonoBehaviour
{
    private int sleepCooldown = 4;
    private Rigidbody rigid;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if(sleepCooldown > 0)
        {
            rigid.Sleep();
            sleepCooldown--;
        }
    }
}
