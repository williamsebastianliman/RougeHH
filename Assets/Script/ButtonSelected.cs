using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelected : MonoBehaviour, IPointerEnterHandler
{
    public AudioManager audioManager;
    public bool isDisabled = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDisabled)
        {
            audioManager.Play("MenuSelect");
        }
    }
    public void onClick()
    {
        if(!isDisabled)
        {
            audioManager.Play("MenuClicked");
        }
    }
}
