// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// #if UNITY_EDITOR
// using UnityEditor;


// [ExecuteInEditMode]
// [RequireComponent(typeof(PinController),typeof(CircleCollider2D))]
// public class PinEditMode : MonoBehaviour
// {
//     [SerializeField] private bool _editMode;

//     static bool editMode;
//     private Tool prevTool = Tool.None;
//     private Tool oldTool = Tool.None;

//     private CircleCollider2D col;
//     private PinController pin;

//     void Awake()
//     {
//         col = GetComponent<CircleCollider2D>();
//         pin = GetComponent<PinController>();
//     }

//     void OnValidate()
//     {
//         col = GetComponent<CircleCollider2D>();
//         pin = GetComponent<PinController>();
//         editMode = _editMode;
//         if(_editMode)
//         {
//             oldTool = Tools.current;
//             Tools.current = Tool.None;
//         }else if(!_editMode && Tools.current == Tool.None)
//         {
//             Tools.current = oldTool;
//             oldTool = Tool.None;
//         }
//     }



//     void OnEnable()
//     {
//         if(!Application.isEditor)
//             Destroy(this);
//         SceneView.onSceneGUIDelegate += OnSceneGUI;
//     }

    
//     void OnSceneGUI(SceneView scene)
//     {
//         prevTool = Tools.current;
//         if((prevTool != Tools.current) && _editMode)
//         {
//             _editMode = false;
//             OnValidate();
//         }

//         if(!_editMode)
//             return;

//         HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
//         if(col == null)
//             col = GetComponent<CircleCollider2D>();

//         if(pin == null)
//             pin = GetComponent<PinController>();

//         Event e = Event.current;    
        
//         mousePosition = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition).origin;
//         DrawHandle();
//         HandleClick(e);
//         HandleUpdate();
//     }


//     Vector2 mousePosition = Vector2.zero;

//     void HandleUpdate()
//     {
        
//         if(pin.getWire() != null)
//             pin.getWire().ForceUpdate(mousePosition);
//     }

//     void HandleClick(Event e)
//     {
//         if(e.type == EventType.MouseDown && e.button == 0)
//         {
//             if((pin.getWire() == null)){
//                 pin.ConnectWire(mousePosition);
//             }else if((pin.getWire() != null) && (PinController.connectingWire == null)){
//                 pin.MoveWire(mousePosition);
//             }

            
//             e.Use();
//         }
//     }



//     void DrawHandle()
//     {
//         // Handles.color = new Color(0.6190f,0.2957f,0.0852f);
//         // Handles.DrawWireDisc(transform.position, Vector3.back, col.radius);
//         Handles.color = new Color(0.6190f,0.2957f,0.0852f,0.1f);
//         Handles.DrawSolidDisc(transform.position, Vector3.back, col.radius);
//     }



// }
// #endif