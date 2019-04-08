using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolController : MonoBehaviour
{

    public enum ToolType
    {
        NONE,
        MOVE,
        WIRE,
        WIRE_CUTTER,
        EDITOR,
        REMOVE,
        CREATE,
    }

    public enum ItemType
    {
        GATE,
        PLATE,
        LED,
        LEVER,
        WEIGHT,
        SPLITTER,
    }


    [SerializeField] private ToolType tool;
    [SerializeField] private ItemType item;
    [SerializeField] private Text uiText;
    
    [Header("Items")]
    [SerializeField] private Transform GatePrefab;
    [SerializeField] private Transform PlatePrefab;
    [SerializeField] private Transform LedPrefab;
    [SerializeField] private Transform LeverPrefab;
    [SerializeField] private Transform WeightPrefab;
    [SerializeField] private Transform SplitterPrefab;

    public static ToolType SelectedTool;
    public static ItemType SelectedItem;

    public static bool AllowTool;

    // Start is called before the first frame update
    void Start()
    {   
        SetAllow(true);
        tool = ToolType.MOVE;
        item = ItemType.GATE;
    }

    // Update is called once per frame
    void Update()
    {
        if(AllowTool)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
                tool = ToolType.MOVE;
            
            if( Input.GetKeyDown(KeyCode.Alpha2))
                tool = ToolType.WIRE;
            
            if( Input.GetKeyDown(KeyCode.Alpha3))
                tool = ToolType.WIRE_CUTTER;

            if( Input.GetKeyDown(KeyCode.Alpha4))
                tool = ToolType.EDITOR;
            if( Input.GetKeyDown(KeyCode.Alpha5))
                tool = ToolType.REMOVE;

        }
        else
        {
            tool = ToolType.NONE;
        }
        if(uiText != null)
        {
            uiText.text = "Tool: " + (char.ToUpper(tool.ToString()[0]) + tool.ToString().Substring(1));
        }
        SelectedTool = tool;
        SelectedItem = item;
    }

    public void SetTool(int t)
    {

        tool = (ToolType)t;
    }

    public void SetItem(Text label)
    {
        string cmpString = label.text.ToUpper();
        if(cmpString.Contains(ItemType.GATE.ToString()))
        {
            item = ItemType.GATE;
        }
        else if(cmpString.Contains(ItemType.PLATE.ToString()))
        {
            item = ItemType.PLATE;
        }
        else if(cmpString.Contains(ItemType.LED.ToString()))
        {
            item = ItemType.LED;
        }
        else if(cmpString.Contains(ItemType.LEVER.ToString()))
        {
            item = ItemType.LEVER;
        }
        else if(cmpString.Contains(ItemType.WEIGHT.ToString()))
        {
            item = ItemType.WEIGHT;
        }
        else if(cmpString.Contains(ItemType.SPLITTER.ToString()))
        {
            item = ItemType.SPLITTER;
        }
        Debug.Log("Item: " + item.ToString());
    }


    public void CreateItem()
    {
        switch (SelectedItem)
        {
            default:
            case ItemType.GATE:
                Instantiate(GatePrefab,Vector3.zero,Quaternion.identity);
                break;
            case ItemType.PLATE:
                Instantiate(PlatePrefab,Vector3.zero,Quaternion.identity);
                break;
            case ItemType.LED:
                Instantiate(LedPrefab,Vector3.zero,Quaternion.identity);
                break;
            case ItemType.LEVER:
                Instantiate(LeverPrefab,Vector3.zero,Quaternion.identity);
                break;
            case ItemType.WEIGHT:
                Instantiate(WeightPrefab,Vector3.zero,Quaternion.identity);
                break;
            case ItemType.SPLITTER:
                Instantiate(SplitterPrefab,Vector3.zero,Quaternion.identity);
                break;
        }
    }


    public void SetAllow(bool _allow)
    {
        AllowTool = _allow;
        tool = ToolType.MOVE;
    }



}
