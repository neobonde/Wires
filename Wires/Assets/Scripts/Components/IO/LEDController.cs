using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDController : MonoBehaviour
{

    //Find Child pin
    //check state of child pin (AKA voltage of it)
    //Change sprite to reflect pin input!

    private PinController input;
    [SerializeField] private Sprite OffSprite;
    [SerializeField] private Sprite OnSprite;
    private SpriteRenderer sr;

    void OnValidate()
    {
        if(OffSprite != null)
        {
            sr = GetComponent<SpriteRenderer>();
            sr.sprite = OffSprite;
        }
    }

    void Awake()
    {
        input = GetComponentInChildren<PinController>();
        sr = GetComponent<SpriteRenderer>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GetState())
        {
            sr.sprite = OnSprite;
        }else
        {
            sr.sprite = OffSprite;
        }
    }
}
