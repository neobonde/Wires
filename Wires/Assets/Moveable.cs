using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{

    private List<Collider2D> colliders;
    private Rigidbody2D rb;

    void Awake()
    {
        colliders = new List<Collider2D>(GetComponents<Collider2D>());
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool followMouse = false;
    private Vector2 mousePosition;
    private Vector2 diffVector;
    // Update is called once per frame
    void Update()
    {
        if(ToolController.Tool == ToolController.ToolType.MOVE)
        {
            if(Input.GetMouseButtonDown(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                foreach (var col in colliders)
                {
                    if(col.OverlapPoint(mousePosition))
                    {
                        followMouse = true;
                    }
                }
            }
        }
        else
        {
            followMouse = false;
        }


        if(followMouse)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePosition);
            if(Input.GetMouseButtonUp(0))
            {
                followMouse = false;
            }
        }
    }

    void FixedUpdate()
    {
        if(followMouse)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePosition);
        }
    }

}
