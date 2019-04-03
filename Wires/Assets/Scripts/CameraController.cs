using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 smoothTime = new Vector2(0.05f,0.05f);
    [SerializeField] public Rect bounds = new Rect(0,0,10,10);
    [SerializeField] private Transform target;
    
    private Camera camera;
    private Vector3 position;
    private Vector2 velocity;
    private Vector3 newPosition;

    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        //Smooth follow of target
        position.x = Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, smoothTime.x);
        position.y = Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity.y, smoothTime.y);
        position.z = transform.position.z;

        // Clamping movement to bounds
        position.x = Mathf.Clamp(position.x,bounds.xMin,bounds.xMax);
        position.y = Mathf.Clamp(position.y,bounds.yMin,bounds.yMax);

        // Apply new postion
        transform.position = position;
    }
}
