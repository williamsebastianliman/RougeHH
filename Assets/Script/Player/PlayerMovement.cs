using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputAction clickAction;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private int speed;
    [SerializeField] private PlayerPositionSO playerPositionSO;
    [SerializeField] private DungeonConfigurationSO config;
    [SerializeField] private MapSO mapSO;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float threshold = 0.01f;
    [SerializeField] private GameObject sword;
    [SerializeField] private PlayerStatSO stat;
    [SerializeField] private GameObject handEffect;
    [SerializeField] private GameObject auraEffect;
    [SerializeField] private GameObject enchanceEffect;
    public bool isMoving;
    private bool stopAfterThis = false;
    private List<Tile> tileList = new List<Tile>();
    private int currentDest;
    private Animator anim;

    private Camera mainCamera;
    private Rigidbody rb;
    private PlayerState currentState;
    private AStar aStar;
    private Vector2 savedPosition = new Vector2(-10, -10);
    private GameObject savedTile = null;

    private Enemy savedEnemy;
    public Enemy deadEnemy;
    private string animationName;
    public bool justAttack = false;
    public bool alrAttack = false;
    public bool inAttack = false;



    private SkillManager skillManager;
    private AudioManager audioManager;
    private void OnEnable()
    {
        mainCamera = Camera.main;
        Debug.Log("Init: " + mainCamera);
        rb = GetComponent<Rigidbody>();
        SwitchState(new ExploringState(this));
        aStar = new AStar(mapSO);
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        skillManager = GameObject.FindGameObjectWithTag("SkillManager").GetComponent<SkillManager>();
        clickAction.Enable();
        clickAction.performed += HandleInput;
    }
    private void OnDisable()
    {
        clickAction.performed -= HandleInput;
    }
    public bool isCrit()
    {
        int num = Random.Range(0, 100);
        if(num<stat.critChance.attr)
        {
            return true;
        }
        return false;
    }
    public void PlayAnimation(string animationName)
    { 
        anim.CrossFade(animationName, 0f);
    }
    public int getDamage()
    {
        int randomMin = 1 + (int)(stat.attack.attr / 10);
        int randomMax = 5 + (int)(2 * stat.attack.attr / 10);
        int randomOffset = Random.Range(randomMin, randomMax);
        return (int)stat.attack.attr + randomOffset;
    }
    private void Update()
    {
        if(stat.isLifesteal)
        {
            handEffect.SetActive(true);
        }
        else
        {
            handEffect.SetActive(false);
        }
        if(stat.isBuff)
        {
            auraEffect.SetActive(true);
        }
        else
        {
            auraEffect.SetActive(false);
        }
        if(stat.enchanceBuff)
        {
            enchanceEffect.SetActive(true);
        }
        else
        {
            enchanceEffect.SetActive(false);
        }
        alreadyBuff();
    }
    public void playerAttack()
    {
        sword.SetActive(true);
        audioManager.Play("PlayerPunch");
        inAttack = true;
        float raw = getDamage();
        raw = raw + raw * stat.bonusDamage;
        bool flag = isCrit();
        if(flag)
        {
            mainCamera.gameObject.GetComponent<CameraMovement>().shakeForTime(0.1f);
            raw = raw * stat.critMultiplier.attr;
        }
        animationName = getAttackAnimation();
        PlayAnimation(animationName);
        bool isDead = savedEnemy.getObject().GetComponent<EntityInfo>().takeDamage((int) raw, flag);

        if(isDead)
        {
            savedEnemy.getObject().GetComponent<EnemyController>().enemyDead();
            deadEnemy = savedEnemy;
        }
        
    }
    public void checkFinishedAttack()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animationName))
        {
            if (stateInfo.normalizedTime >= 1f && !anim.IsInTransition(0))
            {
                sword.SetActive(false);
                PlayAnimation("Idle");
                justAttack = true;
                inAttack = false;
            }
        }
    }
    public string getAttackAnimation()
    {
        int idx = Random.Range(0, 3);
        if (idx == 0)
        {
            return "Attack1";
        }
        else if (idx == 1)
        {
            return "Attack2";
        }
        return "Attack3";
    }

    public void HandleInput(InputAction.CallbackContext context)
    {
        if(config.blockAllInput)
        {
            return;
        }
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Debug.Log("main Camera click: " + mainCamera);
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));
        Debug.Log("Click!");
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundMask))
        {
            Debug.Log("get Hit!");
            if (hitInfo.collider != null)
            {
                Debug.Log("Hit Something!");
                TileInfo info = hitInfo.collider.GetComponent<TileInfo>();
                Tile tempTile = mapSO.getTile(info.getX(), info.getY());
                
                if (tempTile.IsEnemy()&&!inAttack)
                {
                    Debug.Log("Enemy Here!");
                    savedEnemy = tempTile.getEnemy();
                }
                if (isMoving)
                {
                    stopAfterThis = true;
                    return;
                }
                foreach(Tile tile in tileList)
                {
                    tile.getTile().GetComponent<TileInfo>().deselectObject();
                }
                int tileCount = tileList.Count;
                if(tileCount > 0)
                {
                    isMoving = true;
                    currentDest = tileList.Count - 2;
                }
            }
        }
    }
    public void debugPath()
    {
/*        foreach(Tile tile in tileList)
        {
            Debug.Log("Path: " + tile.getX()+" "+ tile.getY());
        }*/
    }
    public void hoverTile()
    {
        if (inAttack)
        {
            return;
        }
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundMask))
        {
            if (hitInfo.collider != null && !isMoving)
            {
                GameObject tileObj = hitInfo.collider.gameObject;
                TileInfo tileInfo = tileObj.GetComponent<TileInfo>();
                int coorX = tileInfo.getX();
                int coorY = tileInfo.getY();
                Vector2 tempTile = new Vector2(coorX, coorY);
                if (tempTile != savedPosition)
                {
                    savedPosition = tempTile;
                    foreach (Tile tile in tileList)
                    {
                        tile.getTile().GetComponent<TileInfo>().deselectObject();
                    }
                    tileList = new List<Tile>();
                    tileList = aStar.algorithm(playerPositionSO.getPlayerX(), playerPositionSO.getPlayerY(), (int)savedPosition.x, (int)savedPosition.y);
                    debugPath();
                    int tileCount = tileList.Count;
                    for(int i= 0;i < tileCount-1;i++)
                    {
                        tileList[i].getTile().GetComponent<TileInfo>().selectObject();
                    }
                }
            }
            else if (hitInfo.collider != null && isMoving)
            {
                GameObject temp = hitInfo.collider.gameObject;
                if (savedTile != null)
                {
                    savedTile.GetComponent<TileInfo>().deselectObject();
                }
                savedTile = temp;
                int x = savedTile.GetComponent<TileInfo>().getX();
                int y = savedTile.GetComponent<TileInfo>().getY();
                if (!aStar.isATileWall(x,y))
                {
                    savedTile.GetComponent<TileInfo>().selectObject();
                }
            }
        }
        else
        {
            if (savedTile != null)
            {
                savedTile.GetComponent<TileInfo>().deselectObject();
            }
            foreach (Tile tile in tileList)
            {
                tile.getTile().GetComponent<TileInfo>().deselectObject();
            }
        }
    }
    public void movePlayer()
    {
        if (isMoving&&!inAttack)
        {
            if (currentDest < 0)
            {
                tileList.Clear();
                isMoving = false;
                currentDest = 0;
                rb.velocity = Vector3.zero;
                return;
            }
            if(currentDest>tileList.Count-1||currentDest<0)
            {
                tileList.Clear();
                isMoving = false;
                currentDest = 0;
                rb.velocity = Vector3.zero;
                return;
            }
            Vector3 dest = new Vector3(tileList[currentDest].getX() * 2, playerPositionSO.getPlayerHeight(), tileList[currentDest].getY() * 2);
            Vector3 initial = new Vector3(transform.position.x, playerPositionSO.getPlayerHeight(),transform.position.z);
            Vector3 moveDir = (dest - initial).normalized;
            transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, dest) <= 0)
            {
                playerPositionSO.changePos((int)(dest.x) / 2, (int)(dest.z) / 2);
                currentDest--;
                if (stopAfterThis)
                {
                    stopAfterThis = false;
                    isMoving = false;
                    return;
                }
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    
    public void listenSkillInput()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha0 + i)))
            {
                int count = skillManager.skillList.Count;
                if(i>count)
                {
                    return;
                }
                skillManager.activateSkill(i);
            }
        }
    }
    public void alreadyBuff()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Buff"))
        {
            if (stateInfo.normalizedTime >= 1f && !anim.IsInTransition(0))
            {
                PlayAnimation("Idle");
            }
        }
    }
    public void movePlayerCombat()
    {
        if (isMoving)
        {
            int lastIdx = tileList.Count - 2;
            if (lastIdx < 0)
            {
                isMoving = false;
                rb.velocity = Vector3.zero;
                return;
            }
            Vector3 dest = new Vector3(tileList[lastIdx].getX() * 2, playerPositionSO.getPlayerHeight(), tileList[lastIdx].getY() * 2);
            Vector3 initial = new Vector3(transform.position.x, playerPositionSO.getPlayerHeight(), transform.position.z);
            Vector3 moveDir = (dest - initial).normalized;
            transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, dest) <= 0)
            {
                playerPositionSO.changePos((int)(dest.x) / 2, (int)(dest.z) / 2);
                isMoving = false;
                rb.velocity = Vector3.zero;
                tileList.Clear();
                return;
            }
        }
        else
        {
            if (savedEnemy != null && isRTA() && !alrAttack)
            {
                alrAttack = true;
                playerAttack();
            }
            if (savedEnemy != null && isRTA())
            {
                checkFinishedAttack();
                rotatePlayer();
                rb.velocity = Vector3.zero;
            }
        }
    }
    public bool isRTA()
    {
        if (savedEnemy == null)
        {
            return false;
        }
        if (savedEnemy.getX() == playerPositionSO.getPlayerX()+1 && savedEnemy.getY() == playerPositionSO.getPlayerY())
        {
            return true;
        }
        if (savedEnemy.getX() == playerPositionSO.getPlayerX()-1 && savedEnemy.getY() == playerPositionSO.getPlayerY())
        {
            return true;
        }
        if (savedEnemy.getX() == playerPositionSO.getPlayerX() && savedEnemy.getY() == playerPositionSO.getPlayerY()+1)
        {
            return true;
        }
        if (savedEnemy.getX() == playerPositionSO.getPlayerX() && savedEnemy.getY()== playerPositionSO.getPlayerY()-1)
        {
            return true;
        }
        return false;
    }
    public void SwitchState(PlayerState newState)
    {
        if(currentState!=null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }
    public void clearList()
    {
        foreach (Tile tile in tileList)
        {
            tile.getTile().GetComponent<TileInfo>().deselectObject();
        }
        if(savedTile!=null)
        {
            savedTile.GetComponent<TileInfo>().deselectObject();
            savedTile = null;
        }
        
        tileList.Clear();
    }
    public bool isMove()
    {
        return isMoving;
    }
    public PlayerState getCurrentState()
    {
        return currentState;
    }
    public void clearSavedEnemy()
    {
        savedEnemy = null;
    }
    public void rotatePlayer()
    {
        if(savedEnemy == null)
        {
            return;
        }
        Vector3 dest = new Vector3(savedEnemy.getX() * 2, playerPositionSO.getPlayerHeight(), savedEnemy.getY() * 2);
        Vector3 inital = transform.position;
        Vector3 moveDir = (dest - inital).normalized;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, 20 * Time.deltaTime);
    }
}
