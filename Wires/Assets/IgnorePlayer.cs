using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class IgnorePlayer : MonoBehaviour
{

    [SerializeField] private bool autosetColliders = false;
    [SerializeField] private List<Collider2D> colliders = new List<Collider2D>();

    private Collider2D playerCollider; 

    void OnValidate()
    {
        SetComponents();
    }

    void Awake()
    {
        SetComponents();
        foreach (var collider in colliders)
        {
            Physics2D.IgnoreCollision(collider,playerCollider,true);
        }
    }

    void SetComponents()
    {
        if(autosetColliders)
            colliders = new List<Collider2D>(GetComponents<Collider2D>());
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
