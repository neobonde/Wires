using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{

    public enum ToolType
    {
        NONE,
        MOVE,
        WIRE,
        WIRE_CUTTER,
        EDITOR,
    }

    public static ToolType Tool;
    [SerializeField] private ToolType _tool;

    // Start is called before the first frame update
    void Start()
    {
        _tool = ToolType.MOVE;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            _tool = ToolType.MOVE;
        
        if( Input.GetKeyDown(KeyCode.Alpha2))
            _tool = ToolType.WIRE;
        
        if( Input.GetKeyDown(KeyCode.Alpha3))
            _tool = ToolType.WIRE_CUTTER;

        if( Input.GetKeyDown(KeyCode.Alpha4))
            _tool = ToolType.EDITOR;

        Tool = _tool;



    }

}
