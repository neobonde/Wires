using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Removeable : MonoBehaviour
{
    // Editor Changeable variables
    [Header("Clickable Colliders")]
    [SerializeField] private bool autosetColliders = true;
    [SerializeField] private List<Collider2D> colliders = new List<Collider2D>();

    // Private variables
    private Vector2 mousePosition = Vector2.zero;

    void OnValidate()
    {
        SetComponents();
    }

    void OnAwake()
    {
        SetComponents();
    }

    void SetComponents()
    {
        if(autosetColliders)
            colliders = new List<Collider2D>(GetComponents<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if((ToolController.SelectedTool == ToolController.ToolType.REMOVE ) && Input.GetMouseButtonDown(0))
        {
            foreach (var col in colliders)
            {
                if(col.OverlapPoint(mousePosition))
                {
                    Destroy(col.gameObject);
                    break;
                }
            }
        }
    }
}
