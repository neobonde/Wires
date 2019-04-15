using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[RequireComponent(typeof(LineRenderer),typeof(EdgeCollider2D))]
public class WireController : MonoBehaviour
{
    private int connectedPins = 0;
    public PinController[] connections = new PinController[2];
    private Vector2 mousePosition = Vector2.zero;

    private LineRenderer wireRenderer;
    private EdgeCollider2D clickableArea;
    private bool state = false;

    void Awake()
    {
        wireRenderer = GetComponent<LineRenderer>();
        clickableArea = GetComponent<EdgeCollider2D>();
        InitConnections();
    }

    public void InitConnections()
    {
        transform.position = Vector2.zero;
        connectedPins = 0;
        foreach (var pin in connections)
        {
            if(pin != null)
            {
                connectedPins ++;
            }
        }
    }



    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Updater(mousePosition);
    }

    public void Updater(Vector2 mousePos)
    {
        if(wireRenderer == null)
            wireRenderer = GetComponent<LineRenderer>();

        if(clickableArea == null)
            clickableArea = GetComponent<EdgeCollider2D>();

        if(connectedPins == 1)
        {
            int i = connections[0] == null?1:0;
            wireRenderer.SetPosition(0,new Vector3(connections[i].transform.position.x, connections[i].transform.position.y, transform.position.z));
            wireRenderer.SetPosition(1,new Vector3(mousePos.x, mousePos.y, transform.position.z));
        
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                connections[i].wire = null;
                Destroy(gameObject);
                connectedPins --;
            }

            if(ToolController.SelectedTool != ToolController.ToolType.WIRE)
            {
                connections[i].wire = null;
                Destroy(gameObject);
                connectedPins --;
            }        
        }
        else if(connectedPins == 2)
        {
            Vector3 pos1 = new Vector3(connections[0].transform.position.x, connections[0].transform.position.y, transform.position.z);
            Vector3 pos2 = new Vector3(connections[1].transform.position.x, connections[1].transform.position.y, transform.position.z);
            wireRenderer.SetPosition(0,pos1);
            wireRenderer.SetPosition(1,pos2);
            clickableArea.points = new Vector2[] { pos1, pos2};

            foreach (var pin in connections)
            {
                if(pin.input)
                    pin.SetState(state);
                else
                    state = pin.GetState();
            }

            if(Input.GetMouseButtonDown(0) && (ToolController.SelectedTool == ToolController.ToolType.WIRE_CUTTER) && Application.isPlaying)
            {
                if(clickableArea.OverlapPoint(mousePosition))
                {
                    foreach (var pin in connections)
                    {
                        pin.wire = null;
                    }
                    Destroy(gameObject);
                }
            }

        }
    }

    void OnDestroy()
    {
        foreach (var pin in connections)
        {
            if(pin != null)
                pin.wire = null;
        }
        Destroy(gameObject);
    }


    public WireController SetConnection(PinController pin)
    {
        if (connectedPins >= 2)
            return null;
        
        if(connectedPins == 1)
        {
            int i = connections[0] == null?1:0;
            if (pin.transform.parent == connections[i].transform.parent)
                return null;

            if(pin.input == connections[i].input)
                return null;
        }
        
        connections[connectedPins] = pin;
        connectedPins++;
        return this;
    }

    public bool RemoveConnection(PinController pin)
    {
        if(connectedPins <= 0)
            return false;

        for(int i = 0; i < 2; i++){
            if(connections[i] == pin){
                connections[i] = null;
                connectedPins--;
                return true;
            }
        }
        return false;
    }
}
