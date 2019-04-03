using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsoleController : MonoBehaviour
{

    [Header("Events")]
	[Space]
	public UnityEvent OnUseEvent;
	public UnityEvent OnLeaveEvent;

    void Awake()
    {
        if (OnUseEvent == null)
            OnUseEvent = new UnityEvent();

        if (OnLeaveEvent == null)
            OnLeaveEvent = new UnityEvent();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            OnUseEvent.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            OnLeaveEvent.Invoke();
        }
    }

}
