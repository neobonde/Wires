using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[ExecuteInEditMode]
public class WireEditMode : MonoBehaviour
{

    WireController wire;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Application.isPlaying)
        {
            if(wire == null)
                wire = GetComponent<WireController>();
            wire.InitConnections();
            wire.Updater(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            // wire.UpdatePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
