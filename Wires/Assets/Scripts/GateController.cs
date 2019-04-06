using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GateController : MonoBehaviour
{
    private enum GateType
    {
        NOT,
        AND,
        OR,
        XOR,
    }

    [Header("Sprites")]
    [SerializeField] private Sprite NOTSprite;
    [SerializeField] private Sprite ANDSprite;
    [SerializeField] private Sprite ORSprite;
    [SerializeField] private Sprite XORSprite;
    private Sprite SelectedSprite;

    [Header("Choose Gate")]
    [SerializeField] private GateType Gate;

    [Header("Gate interface")]
    [SerializeField] private bool AutoSetInterface = true;
    [SerializeField] private PinController[] InputPins = new PinController[1];
    [SerializeField] private PinController OutputPin;

    private Collider2D col;

    void OnValidate()
    {
        if (Gate != GateType.NOT)
        {
            Array.Resize(ref InputPins, 2);
        }
        else if(Gate == GateType.NOT)
        {
            Array.Resize(ref InputPins, 1);
        }
        if(AutoSetInterface)
            SetInterface();
        SelectSprite();
    }

    void SelectSprite()
    {
        switch (Gate)
        {
            default:
            case GateType.NOT:
                SelectedSprite = NOTSprite;
                break; 
            case GateType.AND:
                SelectedSprite = ANDSprite;
                break; 
            case GateType.OR:
                SelectedSprite = ORSprite;
                break; 
            case GateType.XOR:
                SelectedSprite = XORSprite;
                break; 
        }
        GetComponent<SpriteRenderer>().sprite = SelectedSprite;

    }

    void SetInterface()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {

            if (child.name == "Output")
            {
                OutputPin = child.GetComponent<PinController>();
                OutputPin.input = false;
            }
            else if (child.name.Contains("Input"))
            {
                child.gameObject.SetActive(false);
                child.gameObject.GetComponent<PinController>().input = true;
                if (Gate != GateType.NOT)
                {
                    if(child.name.Contains("1"))
                    {
                        InputPins[0] = child.GetComponent<PinController>();
                        child.gameObject.SetActive(true);
                    }
                    else if(child.name.Contains("3"))
                    {
                        InputPins[1] = child.GetComponent<PinController>();
                        child.gameObject.SetActive(true);
                    }
                }
                else if(Gate == GateType.NOT)
                {
                    if(child.name.Equals("Input") || child.name.Contains("2"))
                    {
                        InputPins[0] = child.GetComponent<PinController>();
                        child.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(ToolController.Tool == ToolController.ToolType.EDITOR)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if(col.OverlapPoint(mousePosition))
                {
                    Debug.Log("test");
                    Gate = (GateType)(((int)Gate +1)%Enum.GetNames(typeof(GateType)).Length);
                    Debug.Log(Gate.ToString());
                    SelectSprite();
                    SetInterface();
                }
            }
        }
        switch (Gate)
        {
            default:
            case GateType.NOT:
                OutputPin.SetState(!InputPins[0].GetState());
                break;

            case GateType.AND:
                OutputPin.SetState(InputPins[0].GetState() && InputPins[1].GetState());
                break;

            case GateType.OR:
                OutputPin.SetState(InputPins[0].GetState() || InputPins[1].GetState());
                break;

            case GateType.XOR:
                OutputPin.SetState(!InputPins[0].GetState() != !InputPins[1].GetState());
                break;
        }
    }
}
