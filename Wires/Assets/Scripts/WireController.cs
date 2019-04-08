using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour
{
    private LineRenderer wire;
    private EdgeCollider2D col;

    private List<PinController> connections;
    private Vector3 connection1;
    private Vector3 connection2;
    private bool state = false;


    public bool SetConnection(PinController pin)
    {        
        if(connections.Count >= 2 )
            return false;

        if(connections.Count > 0)
        {
            foreach (var conPin in connections)
            {
                if(conPin.GetParentDevice() == pin.GetParentDevice())
                    return false;
            }
        }

        connections.Add(pin);
        return true;
    }

    void Awake()
    {
        connections = new List<PinController>();
        wire = GetComponent<LineRenderer>();
        col = GetComponent<EdgeCollider2D>();
        col.edgeRadius = wire.startWidth;
    }


    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateStates();
        CheckConnections();
    }


    void UpdateStates()
    {
        foreach (var connection in connections)
        {
            if(connection.input)
            {
                connection.SetState(state);
            }else
            {
                state = connection.GetState();
            }
        }
    }


    void UpdatePosition()
    {

        if(connections.Count == 1)
        {
            // If tool changes cancel wire
            if(ToolController.SelectedTool != ToolController.ToolType.WIRE)
                CutWire();
            
            //If one connection exists the one side of the wire follows the mouse
            connection1 = new Vector3(connections[0].transform.position.x, connections[0].transform.position.y, transform.position.z);
            connection2 = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            wire.SetPosition(0,connection1);
            wire.SetPosition(1,connection2);
            col.points = new Vector2[] { connection1, connection2};
        }
        else if(connections.Count == 2)
        {
            // If reference to pins is lost disconnect self
            if (!CheckConnections())
                return;
            //if there are two connections then the wire is stuck between to gates
            connection1 = new Vector3(connections[0].transform.position.x, connections[0].transform.position.y, transform.position.z);
            connection2 = new Vector3(connections[1].transform.position.x, connections[1].transform.position.y, transform.position.z);
            wire.SetPosition(0,connection1);
            wire.SetPosition(1,connection2);
            col.points = new Vector2[] { connection1, connection2};
        }

        if(ToolController.SelectedTool == ToolController.ToolType.WIRE_CUTTER)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if(col.OverlapPoint(mousePosition))
                {
                    CutWire();
                }

            }
        }
    }

    bool CheckConnections()
    {
        foreach (var pin in connections)
        {
            if(pin == null)
            {
                CutWire();
                return false;
            }
            else if(!pin.gameObject.activeSelf)
            {
                CutWire();
                return false;
            }
        }
        return true;
    }


    public void CutWire()
    {
        foreach (var pin in connections)
        {
            pin.DisconnectWire();
        }
        Destroy(gameObject);
    }
}
