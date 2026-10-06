using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Set in Inspector")]    //A Compiler attribute. Gives specific instructions to either Unity or the compiler.
                                    //The header attribute tells Unity to create a header in the Inspector view of this script
                                    //The Inscribed section contains fileds you are meant to set within the inspector.
                                    //The dynamic section contains fields that will be set dynamically when the game is running.
    public GameObject prefabProjectile;
    public float velocityMult = 10f;
    
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos; //Stores the 3D world position of launchPoint
    public GameObject projectile; //a reference to the new Projectile instance that is created
    public bool aimingMode; // normally false, but is set to true when the player presses mouse button 0 down over the slingshot
    private Rigidbody projectileRigidbody;
    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position; 

        
    }

    void OnMouseEnter()
    {
        // print("SlingShot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        // print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        //the player has pressed the mouse over the keyboard
        aimingMode = true;
        //Instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //start it at the launchPoint
        projectile.transform.position = launchPos;
        //Set it to isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;    //not the safest way to do this due to the projectile prefab not having a RigidBody atttached
                                                                    //GetComponent<Rigidbody>() call would return null and wourl attempt to set the isKinematic field of null (which isn't possible).
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
    void Update()
    {
        //If Slingshot is not in aimingMode, don't run this code
        if(!aimingMode) return;

        //Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Find the delta from the launchPos to the MousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;

        //Limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize(); //Converts all the Magnitude of 1 if it's outside a certain range.
            mouseDelta *= maxMagnitude; //Still maintaint the direction components.
        }

        //Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if(Input.GetMouseButtonUp(0)) //This is a 0 not an o, this will only return true on the frame (the Update) that the player released the mouse button.
        {
            //The mouse has been released
            aimingMode = false;
            //Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.isKinematic = false;
            //projRB.isKinematic = false; //Allows the projectile to now fly through the air based on physics simulation.
            // projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            // projRB.velocity = -mouseDelta * velocityMult;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile; //Set the _MainCamera POI
            projectile = null;  //This severs the connection beteen this instance of the Slingshot script and the projectile GameObject.
                                //This is a good clean-up task to do. If we do not sever this connection and later code destroyed that projectile,
                                //then this remaining reference to it would prevent the computer memory holding the projectile from being reused for toher things.
        }
    }
}