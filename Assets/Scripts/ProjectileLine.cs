using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))] //ensures that the GameObject this script is attached to has a LineRenderer component
public class ProjectileLine : MonoBehaviour
{
    
   static List <ProjectileLine> PROJ_LINES = new List<ProjectileLine>();
   private const float DIM_MULT = 0.75f;
   
    //These private variables start with a "_" to indicate that they are private. This is a common naming convention in C# to help distinguish between private and public variables.
    private LineRenderer _line;
    private bool _drawing = true;
    private Projectile _projectile;


    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 1;
        _line.SetPosition(0, transform.position); //Get a reference to the LineRenderer and assign its first position (i.e., point)

        _projectile = GetComponentInParent<Projectile>();  //Allows you to search up the Hierarchy from this Game Object to look for components (in this case, the Projectile (Script) component
                                                            // of the Projectile Game Object that is the parent of this Projectile Line GameObject).
        ADD_LINE(this);
    }

    void FixedUpdate()
    {
        if(_drawing)
        {
            _line.positionCount++;  //Each frame that this ProjectileLine exists, it increases the positionCount of the LineRenderer (i.e., the number of points in the line) and appends the current world postion to this transform to the line.
            _line.SetPosition(_line.positionCount - 1, transform.position);
            //If the Projectile Rigidbody is sleeping, stop drawing
            if(_projectile != null)
            {
                if(!_projectile.awake) //As long as a Projectile component was found up the Hierarchy, this Projectile line will continue drawing until _projectile.awake is false, at which time it will not only stop drawing but also sever the connection that it has to _projectile (just to keep things clean).
                {
                    _drawing = false;
                    _projectile = null;
                }
            }

        }
    }

    void OnDestroy()
    {
        //Remove this ProjectileLine from PROJ_LINES
        PROJ_LINES.Remove(this);
    }

    static void ADD_LINE(ProjectileLine line)
    {
        Color col;
        //Iterate over all the old lines and dim them
        foreach(ProjectileLine pl in PROJ_LINES)
        {
            col = pl._line.startColor;
            col = col * DIM_MULT;
            pl._line.startColor = pl._line.endColor = col;
        }
        //Add the new line to the list
        PROJ_LINES.Add(line);
    }
}
