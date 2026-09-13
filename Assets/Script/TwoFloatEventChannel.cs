using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TwoFloatEventChannel", menuName = "EventChannel/TwoFloatEvent")]
public class TwoFloatEventChannel : ScriptableObject
{
    public event UnityAction<float, float> OnEventRaised;
    public void RaiseEvent(float value, float value1)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value,value1);
        }
    }
}
