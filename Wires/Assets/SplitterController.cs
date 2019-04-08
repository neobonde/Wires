using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterController : MonoBehaviour
{

    [Header("Splitter interface")]
    [SerializeField] private bool AutoSetInterface = true;
    [SerializeField] private PinController InputPin;
    [SerializeField] private PinController[] OutputPins = new PinController[2];

    void OnValidate()
    {
        if(AutoSetInterface)
            SetInterface();
    }

    void SetInterface()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {

            if (child.name == "Input")
            {
                InputPin = child.GetComponent<PinController>();
                InputPin.input = true;
            }
            else if (child.name.Contains("Output"))
            {
                if(child.name.Contains("1"))
                {
                    OutputPins[0] = child.GetComponent<PinController>();
                    OutputPins[0].input = false;
                }
                else if(child.name.Contains("2"))
                {
                    OutputPins[1] = child.GetComponent<PinController>();
                    OutputPins[1].input = false;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(AutoSetInterface)
            SetInterface();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var pin in OutputPins)
        {
            pin.SetState(InputPin.GetState());
        }
    }
}
