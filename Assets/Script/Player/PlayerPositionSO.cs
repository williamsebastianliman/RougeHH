using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerPosSO", menuName = "Character/Position")]
public class PlayerPositionSO : ScriptableObject
{
    [SerializeField] private int playerX = 0;
    [SerializeField] private int playerY = 0;
    private float playerHeight = 1.05f;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] public MapSO mapSO;

    public bool isClicked;
    public void changePos(int x, int y)
    {
        playerX = x;
        playerY = y;
    }
    public int getPlayerX()
    {
        return this.playerX;
    }
    public int getPlayerY()
    {
        return this.playerY;
    }
    public GameObject getPlayer()
    {
        return playerGameObject;
    }
    public void setPlayer(GameObject playerGameObject)
    {
        this.playerGameObject = playerGameObject;
    }
    public float getPlayerHeight()
    {
        return this.playerHeight;
    }
}
