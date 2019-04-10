using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{

    private enum TriStateType
    {
        OFF,
        RIGHT,
        LEFT,
    }

    [SerializeField] private float uprightTorque = 100; 
    [SerializeField] private Transform child;
    [SerializeField] private Rigidbody2D ArmRigidbody;
    
    [SerializeField] private bool AutoSetInterface = true;
    [SerializeField] private PinController RightPin;
    [SerializeField] private PinController LeftPin;
    

    // private Rigidbody2D ArmRigidbody;

    //Define 3 Lever positions
    private Vector3 rightSnap = new Vector3(0.5f,0.5f,0);
    private Vector3 topSnap = Vector3.zero;
    private Vector3 leftSnap = new Vector3(-0.5f,0.5f,0);
    private float snapAngle = 30;
    private TriStateType state;


    void OnValidate()
    {
        if(AutoSetInterface)
        {
            AssignPins();
        }
    }
    
    void Awake()
    {
        if(AutoSetInterface)
            AssignPins();
    }

    void AssignPins()
    {
        foreach (var pin in GetComponentsInChildren<PinController>())
        {
            if(pin.name.Contains("Left"))
                LeftPin = pin;
            else if(pin.name.Contains("Right"))
                RightPin = pin;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case TriStateType.OFF:
                LeftPin.SetState(false);
                RightPin.SetState(false);
                break;
            case TriStateType.RIGHT:
                LeftPin.SetState(false);
                RightPin.SetState(true);
                break;
            case TriStateType.LEFT:
                LeftPin.SetState(true);
                RightPin.SetState(false);
                break;
        }
    }


    private Vector3 snap = Vector3.zero;

    void FixedUpdate()
    {
        DebugHelper();

        // snap = rightSnap;
        // Find nearest snap point
        float angle = Vector2.SignedAngle(child.up,transform.up);
        
        if(angle > snapAngle)
        {
            snap = rightSnap;
            state = TriStateType.RIGHT;
        }
        else if (angle < -snapAngle)
        {
            snap = leftSnap;
            state = TriStateType.LEFT;
        }
        else /* if (angle < snapAngle && angle > -snapAngle)*/
        {
            snap = transform.up;
            state = TriStateType.OFF;
        }

        // Find difference in rotation from current postion to snap postion
        var rot = Quaternion.FromToRotation(child.up, snap);

        // add small torque to make lever move towards snap
        ArmRigidbody.AddTorque(rot.z*uprightTorque);
    }

    void DebugHelper()
    {
        Debug.DrawLine(transform.position, transform.position + child.up, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.up, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.up, Color.blue);
        Debug.DrawLine(transform.position, transform.position + new Vector3(0.5f,0.5f,0), Color.yellow);
        Debug.DrawLine(transform.position, transform.position + new Vector3(-0.5f,0.5f,0), Color.yellow);
        Debug.DrawLine(transform.position, transform.position + snap, Color.magenta);
        Debug.Log("1: " + Vector2.SignedAngle(child.up,transform.up));
    }

}
