using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public TextMeshPro stateText;
    public float moveSpeed = 3f;
    public float rotationSpeed = 30f;
    public float threshold = 0.1f;
    [SerializeField] private MapSO mapSO;
    [SerializeField] private PlayerPositionSO playerPos;
    [SerializeField] private PlayerStatSO playerStat;
    [SerializeField] private EntityInfo info;
    public float attack;

    private Animator anim;

    private Vector2 currentPos;
    private List<Tile> tileList = new List<Tile>();
    private AStar aStar;
    private Rigidbody rb;
    private EnemyState currentState;
    public bool isMoving;
    public List<Tile> pathList = new List<Tile>();
    public Enemy enemy;
    private bool isAttack;
    private bool alrAttack = false;
    public bool justMove;
    private bool inMove = false;
    private bool alrAlternate = false;
    int lastIndex = -1;

    private string animationName = "";

    public bool isLOS;
    [SerializeField] private LayerMask mask;

    public GameObject sword;
    AudioManager audioManager;
    private Tile tempTile = null;
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
        currentPos = new Vector2(transform.position.x / 2, transform.position.z / 2);
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        SwitchState(new EnemyIdleState(this));
        aStar = new AStar(mapSO);
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    public EnemyState getCurrentState()
    {
        return currentState;
    }
    public void SwitchState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }
    public void clear()
    {
        pathList.Clear();
    }
    public void enemyTurn()
    {
        clear();
        pathList = aStar.algorithm(enemy.getX(), enemy.getY(), playerPos.getPlayerX(), playerPos.getPlayerY());
        isMoving = true;
    }
    public string getAttackAnimation()
    {
        int idx = Random.Range(0, 3);
        if(idx==0)
        {
            return "Attack1";
        }
        else if(idx==1)
        {
            return "Attack2";
        }
        return "Attack3";
    }
    public Tile checkValidPath()
    {
        Tile downTile = null;
        Tile upTile = null;
        Tile leftTile = null;
        Tile rightTile = null;
        int x = enemy.getX();
        int y = enemy.getY();
        if(enemy.getX()+1 < mapSO.getRow()&&mapSO.getTile(x+1,y).isFloor())
        {
            upTile = mapSO.getTile(enemy.getX() + 1, enemy.getY());
        }
        if(enemy.getX()-1>=0 && mapSO.getTile(x - 1, y).isFloor())
        {
            downTile = mapSO.getTile(enemy.getX() - 1, enemy.getY());
        }
        if(enemy.getY()-1>=0 && mapSO.getTile(x, y-1).isFloor())
        {
            leftTile = mapSO.getTile(enemy.getX(), enemy.getY() - 1);
        }
        if(enemy.getY()+1<mapSO.getCol() && mapSO.getTile(x, y+1).isFloor())
        {
            rightTile = mapSO.getTile(enemy.getX(), enemy.getY() + 1);
        }
        Tile thisTile = mapSO.getTile(enemy.getX(), enemy.getY());
        List<Tile> tileList = new List<Tile>
        {
            upTile,
            downTile,
            leftTile,
            rightTile
        };
        Tile shortestTile = thisTile;
        float shortestDistance = Vector3.Distance(new Vector3(playerPos.getPlayerX() * 2, 0, playerPos.getPlayerY() * 2), new Vector3(shortestTile.getX() * 2, 0, shortestTile.getY() * 2));
        foreach(Tile tile in tileList)
        {
            if(tile == null)
            {
                continue;
            }
            if(tile.IsWall())
            {
                continue;
            }
            Vector3 targetPos = new Vector3(tile.getX()*2, 0, tile.getY()*2);
            Vector3 ppos = new Vector3(playerPos.getPlayerX()*2,0, playerPos.getPlayerY()*2);
            float distance = Vector3.Distance(ppos, targetPos);
            if (distance < shortestDistance)
            {
                shortestTile = tile;
                shortestDistance = distance;
            }
        }
        return shortestTile;

    }
    public void enemyMove()
    {
        if (isMoving)
        {
            if(!inMove)
            {
                lastIndex = pathList.Count - 2;
                inMove = true;
            }
            
            if (lastIndex < 0)
            {
                
                if(!alrAlternate)
                {
                    tempTile = checkValidPath();
                    alrAlternate = true;
                }
                if(tempTile!=null)
                {
                    Vector3 initial2 = new Vector3(enemy.getObject().transform.position.x, 1.05f, enemy.getObject().transform.position.z);
                    Vector3 dest2 = new Vector3(tempTile.getX() * 2, 1.05f, tempTile.getY() * 2);
                    Vector3 moveDir2 = (dest2 - initial2).normalized;
                    rb.velocity = moveDir2 * moveSpeed;
                    transform.forward = Vector3.Slerp(transform.forward, moveDir2, rotationSpeed * Time.deltaTime);
                    
                    if (Vector3.Distance(enemy.getObject().transform.position, dest2) <= threshold)
                    {
                        enemy.setCoor(((int)(dest2.x) / 2), (int)(dest2.z) / 2);
                        justMove = true;
                        isMoving = false;
                        inMove = false;
                        enemy.getObject().GetComponent<Rigidbody>().velocity = Vector3.zero;
                        tempTile = null;
                        alrAlternate = false;
                        return;
                    }
                }
                else
                {
                    isMoving = false;
                    justMove = true;
                    inMove = false;
                    alrAlternate = false;
                    rb.velocity = Vector3.zero;
                    return;
                }
                return;
            }
            Vector3 dest = new Vector3(pathList[lastIndex].getX() * 2, 1.05f, pathList[lastIndex].getY() * 2);
            Vector3 initial = new Vector3(enemy.getObject().transform.position.x, 1.05f, enemy.getObject().transform.position.z);
            Vector3 moveDir = (dest - initial).normalized;
            rb.velocity = moveDir * moveSpeed;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
            if (Vector3.Distance(enemy.getObject().transform.position, dest) <= threshold)
            {
                enemy.setCoor(((int)(dest.x) / 2), (int)(dest.z) / 2);
                justMove = true;
                isMoving = false;
                inMove = false;
                enemy.getObject().GetComponent<Rigidbody>().velocity = Vector3.zero;
                return;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    public bool isRTA()
    {
        if(enemy.getX() + 1 == playerPos.getPlayerX() && enemy.getY() == playerPos.getPlayerY())
        {
            return true;
        }
        if (enemy.getX() - 1 == playerPos.getPlayerX() && enemy.getY() == playerPos.getPlayerY())
        {
            return true;
        }
        if (enemy.getX() == playerPos.getPlayerX() && enemy.getY() +1 == playerPos.getPlayerY())
        {
            return true;
        }
        if (enemy.getX()== playerPos.getPlayerX() && enemy.getY() -1 == playerPos.getPlayerY())
        {
            return true;
        }
        return false;
    }
    public void enemyAnimation()
    {
        anim.SetBool("IsMoving", isMoving);
    }
    public void enemyDead()
    {
        audioManager.Play("Death");
        enemy.setDead();
        PlayAnimation("Death");
    }
    public void checkDead()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Death"))
        {
            if (stateInfo.normalizedTime >= 1f && !anim.IsInTransition(0))
            {
                Destroy(gameObject);
            }
        }
    }
    private void Update()
    {
        currentState.ExitState();
        checkDead();
    }
    public void PlayAnimation(string animationName)
    {
        anim.CrossFade(animationName, 0f);
    }
    public int getDamage()
    {
        int randomMin = 1 + (int)(attack / 10);
        int randomMax = 5 + (int)(2 * attack / 10);
        int randomOffset = Random.Range(randomMin, randomMax);
        return (int)attack + randomOffset;
    }
    public bool isCrit()
    {
        int chance = Random.Range(0, 100);
        if(chance < info.critChance)
        {
            return true;
        }
        return false;
    }
    public void enemyAttack()
    {
        if(!alrAttack)
        {
            if(sword!=null)
            {
                sword.SetActive(true);
            }
            audioManager.Play("EnemyPunch");
            int damage = getDamage();
            bool crit = isCrit();
            if(crit)
            {
                playerStat.takeDamage((int)(damage*info.critMultiplier),true);
                mainCamera.gameObject.GetComponent<CameraMovement>().shakeForTime(0.1f);
            }
            else
            {
                playerStat.takeDamage((int)(damage), false);
            }
            

            animationName = getAttackAnimation();
            PlayAnimation(animationName);
            alrAttack = true;
        }
    }
    public void checkFinishedAttack()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animationName))
        {
            if (stateInfo.normalizedTime >= 1f && !anim.IsInTransition(0))
            {
                if(sword!=null)
                {
                    sword.SetActive(false);
                }
                PlayAnimation("Idle");
                justMove = true;
                alrAttack = false;
            }
        }
    }
    public void shootRay()
    {
        Vector3 playerPosition = new Vector3(playerPos.getPlayerX()*2, playerPos.getPlayerHeight(), playerPos.getPlayerY()*2);
        Vector3 enemyPos = enemy.getObject().transform.position;

        Vector3 directionToPlayer = (playerPosition - enemyPos).normalized;
        Debug.DrawRay(transform.position, directionToPlayer * (enemy.getAlertValue() +3f), Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, enemy.getAlertValue() + 3f, mask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                isLOS = true;
            }
            else
            {
                isLOS = false;
            }
        }
    }
    public void rotateEnemy()
    {
        Vector3 dest= new Vector3(playerPos.getPlayerX() * 2, playerPos.getPlayerHeight(), playerPos.getPlayerY() * 2);
        Vector3 inital = transform.position;
        Vector3 moveDir = (dest - inital).normalized;
        transform.forward = Vector3.Slerp(transform.forward, moveDir,20*Time.deltaTime);
    }
}
