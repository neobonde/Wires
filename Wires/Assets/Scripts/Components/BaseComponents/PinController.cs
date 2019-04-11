using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{

    [SerializeField] public bool input;
    [SerializeField] private Transform wirePrefab;
    private WireController wire;
    private Camera cam;
    private CircleCollider2D col;
    // private bool connected = false;
    [HideInInspector] public static WireController connectingWire;
    private bool state;
    Vector2 mousePosition = Vector2.zero;

    void Awake()
    {
        cam = Camera.main;
        col = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Ray ray;
    private RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0) && (wire == null) && ToolController.SelectedTool == ToolController.ToolType.WIRE){
            ConnectWire();
        }else if(Input.GetMouseButtonDown(0) && (wire != null) && (connectingWire == null) && ToolController.SelectedTool == ToolController.ToolType.WIRE){
            MoveWire();
        }
    }

    void OnDestroy()
    {
        if(wire != null)
            Destroy(wire);
    }

    void ConnectWire()
    {
        if(col.OverlapPoint(mousePosition))
        {
            Debug.Log(name);
            // If no wire is currently being connected
            if(connectingWire == null)
            {
                //Create new wire
                wire = Instantiate(wirePrefab,wirePrefab.transform.position,Quaternion.identity).GetComponent<WireController>();
                //Create a static reference to new wire
                connectingWire = wire.GetComponent<WireController>();
                // Connect wire and reserve this pin from further connections
                wire = connectingWire.Connect(this);
            }
            // If a wire is being connected
            else if(connectingWire != null)
            {
                // Connect this wire to the second pin
                wire = connectingWire.Connect(this);
                // If connection was made
                if(wire != null)
                    // Remove static reference to wire, so a new wire can be created
                    connectingWire = null;

            }

        }
    }

    void MoveWire()
    {
        if(col.OverlapPoint(mousePosition))
        {
            if(wire.Disconnect(this))
            {
                connectingWire = wire;
                wire = null;
            }
            
        }
    }


    public Transform GetParentDevice()
    {
        return transform.parent;
    }

    public bool GetState()
    {
        return state;
    }

    // Maybe check if the the pin is an output and the setter is legal setter?
    public void SetState(bool _state)
    {
        if(wire != null)
            state = _state;
    }

}
