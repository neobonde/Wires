using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class WireConnector : EditorWindow
{

    bool toolEnabled;
    public Object wirePrefab;
    WireController wire;

    [MenuItem("Window/WireConnector")]
    public static void ShowWindow() {
        GetWindow(typeof(WireConnector));
    }

    Tool savedTool;
    void OnGUI()
    {
        GUILayout.Label ("Connect Wires", EditorStyles.boldLabel);
        bool toolEnabler = EditorGUILayout.BeginToggleGroup ("Enable Wire tool", toolEnabled);
        EditorGUILayout.BeginHorizontal();
        wirePrefab = EditorGUILayout.ObjectField(wirePrefab, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.ObjectField(wire,typeof(WireController),true);
        
        if(toolEnabled != toolEnabler)
        {
            if(toolEnabler)
            {
                savedTool = Tools.current;
                Tools.current = Tool.None;
                oldTool = Tool.None;
                toolEnabled = true;
            }
            else
            {
                toolEnabled = false;
                Tools.current = savedTool;
            }
        }

    }

    void OnEnable(){
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }
    void OnDisable(){
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    
    Tool oldTool;
    void OnSceneGUI(SceneView scene)
    {

        if(Tools.current != oldTool)
        {
            toolEnabled = false;
            Repaint();
        }    

        oldTool = Tools.current;
        Event e = Event.current;
        if(toolEnabled && !Application.isPlaying)
        {
            ToolController.SelectedTool = ToolController.ToolType.WIRE;
            if(e.type == EventType.MouseDown && e.button == 0)
            {
                Debug.Log("Click");
                LayerMask mask = LayerMask.GetMask("Connector");
                Ray ray = HandleUtility.GUIPointToWorldRay( e.mousePosition);
                RaycastHit2D[] hit2d = Physics2D.RaycastAll(ray.origin, ray.direction,10000,mask);
                foreach (var hit in hit2d)
                {
                    Debug.Log("Ray hit: " + hit.collider.name);
                    if(wire == null)
                    {
                        PinController pin = hit.collider.GetComponent<PinController>();
                        Undo.RecordObject(pin, "ConnectedWire");
                        pin.ConnectWire();
                        EditorUtility.SetDirty(pin);
                        wire = PinController.connectingWire;
                        Undo.FlushUndoRecordObjects();
                        // GameObject clone  = PrefabUtility.InstantiatePrefab(wirePrefab as GameObject) as GameObject;
                        // wire = clone.GetComponent<WireController>();
                        // PinController pin = hit.collider.GetComponent<PinController>();
                        // wire.SetConnection(pin);
                    }else
                    {
                        PinController pin = hit.collider.GetComponent<PinController>();
                        Undo.RecordObject(pin, "ConnectedWire");
                        pin.ConnectWire();
                        EditorUtility.SetDirty(pin);
                        wire = PinController.connectingWire;
                        Undo.FlushUndoRecordObjects();
                    }




                }
                e.Use();
            }
            if(wire != null)
                wire.Updater(HandleUtility.GUIPointToWorldRay(e.mousePosition).origin);
        }else{
            if(wire != null)
                DestroyImmediate(wire.gameObject);
        }

    }

}
