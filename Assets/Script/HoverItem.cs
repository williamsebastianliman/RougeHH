using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject imageCover;
    public AudioManager audioManager;
    public void OnPointerExit(PointerEventData eventData)
    {
        if(imageCover!=null)
        {
            imageCover.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(imageCover!=null)
        {
            imageCover.SetActive(true);
            audioManager.Play("MenuSelect");
        }
        
    }
}
