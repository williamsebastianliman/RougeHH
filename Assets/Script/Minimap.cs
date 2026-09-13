using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public MapSO map;

    public RawImage mapImage;
    public Texture2D mapTexture;
    private void Start()
    {
        int col = map.getCol();
        int row = map.getRow();
        mapTexture = new Texture2D(col, row);
        for(int i=0;i<row;i++)
        {
            for (int j = 0; j < col;j++)
            {
                Color color = Color.white;
                if(map.getTile(i,j).IsEnemy())
                {
                    Debug.Log("Enemy!");
                    color = Color.red;
                }
                else if(map.getTile(i,j).IsWall())
                {
                    Debug.Log("Wall");
                    color = Color.green;
                }
                else if(map.getTile(i,j).isFloor())
                {
                    Debug.Log("Space!");
                    color = Color.black;
                }
                mapTexture.SetPixel(i, j, color);
            }
        }
        mapTexture.Apply();
        mapImage.texture = mapTexture;
    }
}
