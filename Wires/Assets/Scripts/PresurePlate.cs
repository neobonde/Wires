using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{

    [SerializeField] private Sprite OpenSprite;
    [SerializeField] private Sprite ClosedSprite;
    [SerializeField] private BoxCollider2D trigger;


    private SpriteRenderer sr;
    private PinController output;

    private List <Collider2D> pressers;    

    void OnValidate()
    {
        if(OpenSprite != null)
        {
            sr = GetComponent<SpriteRenderer>();
            sr.sprite = OpenSprite;
        }
    }

    void Awake()
    {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), GetComponent<Collider2D>(),true);
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = OpenSprite;
        output = GetComponentInChildren<PinController>();
        pressers = new List<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sr.sprite == ClosedSprite && !output.GetState())
        {
            output.SetState(true);
        }
        if(sr.sprite == OpenSprite && output.GetState())
        {
            output.SetState(false);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.IsChildOf(transform))
            return;
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Ignore Collision"))
            return;

        // Keep track of what is currently on the plate
        pressers.Add(other);
        output.SetState(true);
        sr.sprite = ClosedSprite;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.IsChildOf(transform))
            return;
            
        if(other.gameObject.layer == LayerMask.NameToLayer("Ignore Collision"))
            return;
        
        // Keep track of what is currently on the plate
        pressers.Remove(other);
        //If nothing is on the plate diable the output
        if(pressers.Count == 0)
        {
            output.SetState(false);
            sr.sprite = OpenSprite;
        }
    }

}
