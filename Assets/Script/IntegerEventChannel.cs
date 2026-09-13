using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntEventChannel", menuName = "EventChannel/IntEventChannel")]
public class IntegerEventChannel : ScriptableObject
{
    public event UnityAction<float> OnEventRaised;
    public void RaiseEvent(float value)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value); 
        }
    }

}
