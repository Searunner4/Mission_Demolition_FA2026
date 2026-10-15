using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))] //ensures that the GameObject this script is attached to has a LineRenderer component
public class Goal : MonoBehaviour
{
    //A static field accessible by code anywhere.
    static public bool goalMet = false;
    void OnTriggerEnter(Collider other)
    {
        //When the trigger is hit by something
        //Check to see if it's a Projectile
        Projectile proj = other.GetComponent<Projectile>();
        if(proj != null)
        {
            //If it is a Projectile, set goalMet to true
            Goal.goalMet = true;
            //Also set the alpha of the colro to higher opacity
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.75f;
            mat.color = c;

        }
    }
}
