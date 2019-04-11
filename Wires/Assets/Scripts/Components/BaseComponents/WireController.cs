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

    void Awake()
    {
        connections = new List<PinController>();
        wire = GetComponent<LineRenderer>();
        col = GetComponent<EdgeCollider2D>();
        col.edgeRadius = wire.startWidth*2;
    }

    void Update()
    {
        UpdatePosition();
        UpdateStates();
        CheckConnections();
    }

    void OnDestroy()
    {
        foreach (var pin in connections)
        {
            if(pin.input)
                pin.SetState(false);
        }
        Destroy(gameObject);
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
        // If reference to pins is lost disconnect self
        if (!CheckConnections())
            return;

        if(connections.Count == 1)
        {
            // If tool changes cancel wire
            if(ToolController.SelectedTool != ToolController.ToolType.WIRE)
                Cut();
            
            //If one connection exists the one side of the wire follows the mouse
            connection1 = new Vector3(connections[0].transform.position.x, connections[0].transform.position.y, transform.position.z);
            connection2 = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            wire.SetPosition(0,connection1);
            wire.SetPosition(1,connection2);
            col.points = new Vector2[] { connection1, connection2};
        }
        else if(connections.Count == 2)
        {
            
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
                    Cut();
                }

            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && connections.Count == 1)
        {
            Disconnect();
        }
    }

    bool CheckConnections()
    {
        if(connections.Count <= 0)
        {
            Destroy(this);
            return false;
        }
        foreach (var pin in connections)
        {
            if(!pin.isActiveAndEnabled)
            {
                Destroy(this);
                return false;
            }
        }
        return true;
    }

    
    public WireController Connect(PinController pin)
    {        
        if(connections.Count >= 2 )
            return this;

        if(connections.Count > 0)
        {
            foreach (var connection in connections)
            {
                if(connection.GetParentDevice() == pin.GetParentDevice())
                    return null;
                if(connection.input == pin.input)
                    return null;
            }
        }

        connections.Add(pin);
        return this;
    }

    private bool Disconnect()
    {
        if(connections.Count <= 0)
            return false;

        connections.Clear();
        return true;
    }

    public bool Disconnect(PinController pin)
    {
        if(connections.Count <= 0)
            return false;
        if(pin.input)
            pin.SetState(false);
        else
            state = false;
        return connections.Remove(pin);
    }


    public void Cut()
    {
        Destroy(this);
    }
}
