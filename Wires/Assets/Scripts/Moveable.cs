using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{

    [SerializeField] private float uprightTorque = 100;
    [SerializeField] private float angularDrag = 5;

    private List<Collider2D> colliders;
    private Rigidbody2D rb;

    private bool oldIgnoreCollision;


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
    private float oldDrag = 0;
    // Update is called once per frame
    void Update()
    {
        if(ToolController.SelectedTool == ToolController.ToolType.MOVE)
        {
            if(Input.GetMouseButtonDown(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                foreach (var col in colliders)
                {
                    if(col.OverlapPoint(mousePosition))
                    {
                        oldDrag = rb.angularDrag;
                        rb.angularDrag = angularDrag;
                        //Ignore collisions with player during move
                        Collider2D playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
                        foreach (var collider in GetComponents<Collider2D>())
                        {
                            if(!collider.isTrigger)
                                oldIgnoreCollision |= Physics2D.GetIgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(),collider);
                                Physics2D.IgnoreCollision(playerRb,col,true);
                        }


                        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), GetComponent<Collider2D>(),true);
                        followMouse = true;
                    }
                }
            }
        }
        else
        {
            followMouse = false;
        }

        if(ToolController.SelectedTool == ToolController.ToolType.REMOVE)
        {
            if(Input.GetMouseButtonDown(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                foreach (var col in colliders)
                {
                    if(col.OverlapPoint(mousePosition))
                    {
                        // If moveable has pins disconnect them
                        foreach (var pin in GetComponentsInChildren<PinController>())
                        {
                            pin.DisconnectWire();
                        }
                        Destroy(col.gameObject);
                    }

                }
            }
        }


        if(followMouse)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePosition);
            if(Input.GetMouseButtonUp(0))
            {
                rb.angularDrag = oldDrag;
                //Reset old collision state
                Collider2D playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
                foreach (var collider in GetComponents<Collider2D>())
                {
                    if(!collider.isTrigger)
                        Physics2D.IgnoreCollision(playerRb,collider,oldIgnoreCollision);
                    else
                        Physics2D.IgnoreCollision(playerRb,collider,true);
                }
                Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), GetComponent<Collider2D>(),oldIgnoreCollision);
                followMouse = false;
            }
        }
    }

    void FixedUpdate()
    {
        if(followMouse)
        {
            
            var rot = Quaternion.FromToRotation(transform.up, Vector2.up);
            rb.AddTorque(rot.z*uprightTorque);

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePosition);
        }
    }

}
