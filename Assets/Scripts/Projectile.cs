using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //ensures that the GameObject this script is attached to has a Rigidbody component

public class Projectile : MonoBehaviour
{
    const int LOOKBACK_COUNT = 10; //Stores the maximum length that we want to allow the deltas List to reach. Const because the same value will be used for all instances of the Projectile class. Any consts are replaced b the literal number when the code is compiled.
    static List<Projectile> PROJECTILES = new List<Projectile>();

    [SerializeField]
    private bool _awake = true; //Want to be able to see it in the inspector and want other scripts to be able to read it, but not write to it. So we make it private and use a public property to access it.
    public bool awake
    {
        get {return _awake;}
        private set {_awake = value;}
    }

    private Vector3 prevPos;
    //This private List stores the history of Projectile's move distance
    private List<float> deltas = new List<float>(); 
    private Rigidbody rigid; 
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        awake = true;
        prevPos = new Vector3(1000, 1000, 0);   //set the initial values in the prevPos and deltas to a position and distance far from the actual position of the projectile.
                                                //This gives the Projectile some time to get moving before the initial delta value is removed from the deltas List, just in case there is a physics hiccup that might make the Projectile appear to not move in the first frames.
        deltas.Add(1000);

        PROJECTILES.Add(this); //Add this Projectile to the static List of all Projectiles
    }
    void FixedUpdate()
    {
        if(rigid.isKinematic || !awake) //if the Rigidbody is kinematic or the Projectile is asleep, we don't need to check for movement, so we return early. Checks between the Projectile is fired and when asleep first becomes true.
        {
            return;
        }
        Vector3 deltaV3 = transform.position - prevPos; //Calculate the distance the Projectile has moved since the last FixedUpdate. This is done by subtracting the previous position from the current position.
        deltas.Add(deltaV3.magnitude);
        prevPos = transform.position;

        //Limit lookback, once you get more than LOOKBACK_COUNT deltas, remove the oldest one. This is done to prevent the deltas List from growing indefinitely and consuming too much memory.
        while(deltas.Count > LOOKBACK_COUNT)
        {
            deltas.RemoveAt(0);
        }

        //Iterate over deltas and find the greatest one. If the max distance in deltas is lower than sleepThreshold, set awake to false and tell the Rigidbody to sleep (for at least one frame).
        float maxDelta = 0;
        
        foreach(float f in deltas)
        {
            if(f > maxDelta)
            {
                maxDelta = f;
            }
        }

        //If the Projectile hasn't move more than the sleepThreshold
        if(maxDelta <= Physics.sleepThreshold)
        {
            awake = false;
            rigid.Sleep();
        }
    }

    private void OnDestroy() //When the Projectile is destroyed, remove it from the static List of all Projectiles
    {
        PROJECTILES.Remove(this);
    }

    static public void DESTROY_PROJECTILES() //Static method to destroy all Projectiles in the PROJECTILES List
    {
        foreach(Projectile p in PROJECTILES)
        {
            Destroy(p.gameObject);
        }
        PROJECTILES.Clear(); //Clear the List after destroying all Projectiles
    }
}
