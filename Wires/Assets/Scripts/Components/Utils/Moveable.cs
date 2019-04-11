using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class Moveable : MonoBehaviour
{
    // Editor Changeable variables
    [Header("Dragging Parameters")]
    [SerializeField] private float movementForce = 150.0f;
    [SerializeField] private float linearDrag = 12.5f;
    [SerializeField] private float dropDistance = 5.0f;
    
    [Header("Uprighting Parameters")]
    [SerializeField] private float uprightTorque = 100.0f;
    [SerializeField] private float angularDrag = 5.0f;

    [Header("Clickable Colliders")]
    [SerializeField] private bool autosetColliders = true;
    [SerializeField] private List<Collider2D> clickableColliders = new List<Collider2D>();

    [Header("Physical Collider")]
    [SerializeField] private List<Collider2D> colliders = new List<Collider2D>();
    
    // Required Components
    private Rigidbody2D rb;

    [HideInInspector] public static bool mouseOccupied; 

    // Private variables
    private bool followMouse = false;
    private float oldAngularDrag = 0;
    private float oldLinearDrag = 0;
    private Dictionary<Collider2D,bool> oldIgnoreCollision = new Dictionary<Collider2D,bool>();
    private Vector2 mousePosition = Vector2.zero;
    private Vector2 grabOffset = Vector2.zero;
    private Collider2D playerCollider;



    void OnValidate()
    {
        SetComponents();
    }

    void Awake()
    {
        SetComponents();
    }

    void SetComponents()
    {
        if(autosetColliders)
            clickableColliders = new List<Collider2D>(GetComponents<Collider2D>());
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MoveToolUpdate();
    }

    void FixedUpdate()
    {
        if(followMouse)
        {
            FollowTarget(mousePosition+grabOffset,dropDistance);
            KeepUpright();       
        }
    }

    void MoveToolUpdate()
    {
        // If tool is move tool and mouse is clicked
        if((ToolController.SelectedTool == ToolController.ToolType.MOVE) && Input.GetMouseButton(0) && !followMouse && !mouseOccupied)
        {   
            // Look at all "clickable colliders"
            foreach (var col in clickableColliders)
            {   
                // if mouse is overlapping one we pick up the object, and stop looking at the other colliders
                if(col.OverlapPoint(mousePosition))
                {
                    PickUpObject();
                    break;
                }
            }
        }
        
        // If mouse button is released, release object.
        if(Input.GetMouseButtonUp(0) && followMouse)
        {
            DropObject();
        }

        if((ToolController.SelectedTool != ToolController.ToolType.MOVE) && followMouse)
        {
            // If tool is no longer Move tool drop the object
            DropObject();
        }
        
    }

    void PickUpObject()
    {
        // Save angular drag set in rigidbody component but use custom drag when beeing moved
        SaveSettings();
        rb.angularDrag = angularDrag;
        rb.drag = linearDrag;
        foreach (var collider in colliders)
        {
            Physics2D.IgnoreCollision(collider,playerCollider,true);
        }

        grabOffset = rb.position-mousePosition;

        //Ignore collisions with player during move 
        //TODO: 

        //Signal to fixed update that the object should follow mouse
        followMouse = true;
        mouseOccupied = true;
    }


    void FollowTarget(Vector2 target, float dropDistance)
    {
        // If the distance to the target is to long we drop the object
        if(Vector2.Distance(target,rb.position) > dropDistance)
        {
            DropObject();
        }
        // Add force in the direction of the target propotional to the distance, 
        // movement force variable and the rigidbody mass 
        rb.AddForce((target- rb.position)*movementForce*rb.mass);
    }

    void DropObject()
    {
        // Signal to fixed update to not follow mouse
        followMouse = false;
        mouseOccupied = false;
        RestoreSettings();
    }

    void KeepUpright()
    {   
        // If upside down nudge it
        if(Vector2.Angle(transform.up,Vector2.up) == 180.0f)
            rb.AddTorque(uprightTorque);
        Quaternion rot = Quaternion.FromToRotation(transform.up, Vector2.up);
        rb.AddTorque(rot.z*uprightTorque);
    }

    void SaveSettings()
    {
        oldAngularDrag = rb.angularDrag;
        oldLinearDrag = rb.drag;
        foreach (var collider in colliders)
        {
            oldIgnoreCollision[collider] = Physics2D.GetIgnoreCollision(collider, playerCollider);
        }
    }

    void RestoreSettings()
    {
        // Restore angular drag settings in the rigidbody
        rb.angularDrag = oldAngularDrag;
        rb.drag = oldLinearDrag;
        // Restore collsion settings
        foreach (KeyValuePair<Collider2D, bool> oldSetting in oldIgnoreCollision)
        {
            Physics2D.IgnoreCollision(oldSetting.Key,playerCollider,oldSetting.Value);
        }
    }

}
