using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{

    [SerializeField] private Sprite OpenSprite;
    [SerializeField] private Sprite ClosedSprite;
    private PinController output;

    [SerializeField] private BoxCollider2D trigger;

    private SpriteRenderer sr;

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
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = OpenSprite;
        output = GetComponentInChildren<PinController>();
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
        if(other.tag == "Player" || other.tag == "Weight")
        {
            output.SetState(true);
            sr.sprite = ClosedSprite;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.IsChildOf(transform))
            return;
        if(other.tag == "Player" || other.tag == "Weight")
        {
            output.SetState(false);
            sr.sprite = OpenSprite;
        }
    }

}
