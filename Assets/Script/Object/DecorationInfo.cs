using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationInfo : MonoBehaviour
{
   [SerializeField] private float itemHeight;
   [SerializeField] private bool isWalkable;
   public float getItemHeight()
   {
        return itemHeight;
   }
   public void getItemHeight(float itemHeight)
   {
        this.itemHeight = itemHeight;
   }
   public bool getWalkable()
   {
        return isWalkable;
   }
}
