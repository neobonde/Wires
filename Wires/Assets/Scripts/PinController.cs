using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{

    [SerializeField] public bool input;
    [SerializeField] private Transform wirePrefab;
    private Transform wire;
    private Camera cam;
    private CircleCollider2D col;
    private bool connected = false;
    [HideInInspector] public static WireController connectingWire;
    private bool state;

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
        
        if(Input.GetMouseButtonDown(0) && !connected && ToolController.Tool == ToolController.ToolType.WIRE){
            ConnectWire();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && connected)
        {
            CancelWire();
        }
    }

    void ConnectWire()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(col.OverlapPoint(mousePosition))
        {
            Debug.Log(name);
            // If no wire is currently being connected
            if(connectingWire == null)
            {
                //Create new wire
                wire = Instantiate(wirePrefab,Vector2.zero,Quaternion.identity);
                //Create a static reference to new wire
                connectingWire = wire.GetComponent<WireController>();
                // Connect wire and reserve this pin from further connections
                connected = connectingWire.SetConnection(this);
            }
            // If a wire is being connected
            else if(connectingWire != null)
            {
                // Connect this wire to the second pin
                connected = connectingWire.SetConnection(this);
                // If connection was made
                if(connected)
                    // Remove static reference to wire, so a new wire can be created
                    connectingWire = null;

            }

        }
    }

    public void DisconnectWire()
    {
        if(input)
        {
            state = false;
        }
        connected = false;
    }

    public Transform GetParentDevice()
    {
        return transform.parent;
    }

    void CancelWire()
    {
        Destroy(connectingWire.gameObject);
        connectingWire = null;
        connected = false;
    }

    public bool GetState()
    {
        return state;
    }

    // Maybe check if the the pin is an output and the setter is legal setter?
    public void SetState(bool _state)
    {
        if(connected)
            state = _state;
    }

}
