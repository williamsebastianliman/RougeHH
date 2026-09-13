using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private GameObject col;
    private Material defaultMaterial;
    private Renderer renderers;
    private void Start()
    {
        x = (int)transform.position.x / 2;
        y = (int)transform.position.z / 2;
        renderers = GetComponent<Renderer>();
        defaultMaterial = renderers.material;
    }
    public void setX(int x)
    {
        this.x = x;
    }
    public void setY(int y)
    {
        this.y = y;
    }
    public int getX()
    {
        return x;
    }
    public int getY()
    {
        return y;
    }
    public void deselectObject()
    {
        if(renderers!= null)
        {
            renderers.material = defaultMaterial;
        }
    }
    public void selectObject()
    {
        if(renderers!=null)
        {
            renderers.material = selectedMaterial;
        }
    }
    public void enableCollider()
    {
        col.SetActive(true);
    }
    public void disableCollider()
    {
        col.SetActive(false);
    }
}
