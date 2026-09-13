using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager: MonoBehaviour
{
    [SerializeField] DungeonConfigurationSO config;
    [SerializeField] PlayerPositionSO playerPos;
    private TurnState currentState;
    private List<Enemy> enemies = new List<Enemy>();
    private List<Enemy> aggroEnemy = new List<Enemy>();
    public PlayerMovement playerMovement;
    private bool isNowMove;
    private bool combatMode;
    private bool exploreMode;
    private bool initial = true;
    public bool inCombat = false;

    private int savedX;
    private int savedY;
    private int savedEnemyCount;
    private bool isAgro = false;
    private AudioManager audioManager;
    [SerializeField] private SkillManager skillManager;
    private void Start()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        SwitchState(new IdleTurnState(this, playerMovement));
        savedX = playerPos.getPlayerX();
        savedY = playerPos.getPlayerY();
        checkEnemy();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.Play("ExploreMusic");
        audioManager.Play("BattleMusic");
        audioManager.Pause("BattleMusic");
    }
    private bool observeChangeTiles()
    {
        if(savedX!=playerPos.getPlayerX() || savedY !=playerPos.getPlayerY())
        {
            savedX = playerPos.getPlayerX();
            savedY = playerPos.getPlayerY();
            audioManager.Play("Footstep");
            skillManager.minusCooldown();
            return true;
        }
        return false;
    }
    private float ecludian(int x, int y, int x1, int y1)
    {
        return Mathf.Sqrt(Mathf.Pow(x1 - x,2) + Mathf.Pow(y1 - y,2));
    }
    private bool playerAttackObserver()
    {
        if(playerMovement.justAttack)
        {
            playerMovement.justAttack = false;
            return true;
        }
        return false;
    }
    private void checkEnemy()
    {
        int enemyCount = enemies.Count;
        for (int i = enemyCount - 1; i >= 0; i--)
        {
            Enemy e = enemies[i];
            EnemyController enemyController = e.getObject().GetComponent<EnemyController>();
            if (ecludian(playerPos.getPlayerX(), playerPos.getPlayerY(), e.getX(), e.getY()) > e.getAlertValue() && enemyController.getCurrentState() is EnemyAlertState)
            {
                enemies.RemoveAt(i);
                enemyController.SwitchState(new EnemyIdleState(enemyController));
            }
        }
        int alertCount = enemies.Count;
        if(alertCount==0&&savedEnemyCount>0)
        {
            Debug.Log("Enemy 0 go back to explore mode after this tile!");
            savedEnemyCount = 0;
            exploreMode = true;
        }
        foreach(Enemy e in config.enemies)
        {
            if(ecludian(playerPos.getPlayerX(), playerPos.getPlayerY(), e.getX(), e.getY()) < e.getAlertValue())
            {
                bool alr = false;
                foreach(Enemy e1 in enemies)
                {
                    if(e==e1)
                    {
                        alr = true;
                    }
                }
                Debug.Log(e);
                EnemyController enemyController = e.getObject().GetComponent<EnemyController>();
                if (!alr && enemyController.getCurrentState() is EnemyIdleState)
                {
                    enemies.Add(e);
                    
                    enemyController.SwitchState(new EnemyAlertState(enemyController, e, playerPos));
                    combatMode = true;
                }
            }
        }
        savedEnemyCount = enemies.Count;
    }
    private void checkAgroEnemy()
    {
        isAgro = false;
        foreach(Enemy e in enemies)
        {
            EnemyController controller = e.getObject().GetComponent<EnemyController>();
            controller.getCurrentState().Reset();
            if(controller.isLOS)
            {
                bool alr = false;
                foreach (Enemy e1 in aggroEnemy)
                {
                    if (e == e1)
                    {
                        alr = true;
                    }
                }
                if(!alr)
                {
                    audioManager.Pause("ExploreMusic");
                    audioManager.Resume("BattleMusic");
                    audioManager.Play("Alert");
                    isAgro = true;
                    aggroEnemy.Add(e);
                    EnemyController enemyController = e.getObject().GetComponent<EnemyController>();
                    enemyController.SwitchState(new EnemyAggroState(enemyController, e));
                }
            }
        }
        if(isAgro)
        {
            SwitchState(new PlayerTurnState(this, playerMovement));
        }
    }
    private bool observePlayerMovement()
    {
        if (!isNowMove && playerMovement.isMove())
        {
            isNowMove = true;
        }
        if (isNowMove && !playerMovement.isMove())
        {
            isNowMove = false;
            return true;
        }
        return false;
    }
    public void Update()
    {
        if(config.blockAllInput)
        {
            return;
        }
        currentState.UpdateState();
        bool isChange = observeChangeTiles();
        if (isChange)
        {
            checkEnemy();
            checkAgroEnemy();
        }
        if ((isChange && combatMode) || (combatMode && initial))
        {
            combatMode = false;
            playerMovement.SwitchState(new CombatState(playerMovement));
        }
        if(exploreMode&&isChange)
        {
            exploreMode = false;
            playerMovement.SwitchState(new ExploringState(playerMovement));
        }
        if(currentState is PlayerTurnState && Input.GetKeyDown(KeyCode.Space))
        {
            skillManager.minusCooldown();
            foreach (Enemy e in aggroEnemy)
            {
                EnemyController controller = e.getObject().GetComponent<EnemyController>();
                if (controller.isRTA() && controller.getCurrentState() is EnemyAggroState)
                {
                    controller.SwitchState(new EnemyRTAState(controller, e));
                }
                else if (!controller.isRTA() && controller.getCurrentState() is EnemyRTAState)
                {
                    controller.SwitchState(new EnemyAggroState(controller, e));
                }
            }
            SwitchState(new EnemyTurnState(this, aggroEnemy));
        }
        if (isChange)
        {
            initial = false;
        }
        bool flag = playerAttackObserver();
        if (observePlayerMovement()&&currentState is PlayerTurnState||flag&&currentState is PlayerTurnState)
        {
            if(flag && currentState is PlayerTurnState)
            {
                
                skillManager.makeAllToggleSkillCooldown();
                skillManager.minusCooldown();
            }
            playerMovement.alrAttack = false;
            if (playerMovement.deadEnemy != null)
            {
                enemies.Remove(playerMovement.deadEnemy);
                aggroEnemy.Remove(playerMovement.deadEnemy);
                config.removeEnemy(playerMovement.deadEnemy);
                playerMovement.deadEnemy = null;
                if (aggroEnemy.Count <= 0)
                {
                    audioManager.Pause("BattleMusic");
                    audioManager.Resume("ExploreMusic");
                    SwitchState(new IdleTurnState(this, playerMovement));
                }
                else
                {
                    foreach (Enemy e in aggroEnemy)
                    {
                        EnemyController controller = e.getObject().GetComponent<EnemyController>();
                        if (controller.isRTA() && controller.getCurrentState() is EnemyAggroState)
                        {
                            controller.SwitchState(new EnemyRTAState(controller, e));
                        }
                        else if (!controller.isRTA() && controller.getCurrentState() is EnemyRTAState)
                        {
                            controller.SwitchState(new EnemyAggroState(controller, e));
                        }
                    }
                    SwitchState(new EnemyTurnState(this, aggroEnemy));
                }
            }
            else
            {
                foreach (Enemy e in aggroEnemy)
                {
                    EnemyController controller = e.getObject().GetComponent<EnemyController>();
                    if (controller.isRTA() && controller.getCurrentState() is EnemyAggroState)
                    {
                        controller.SwitchState(new EnemyRTAState(controller, e));
                    }
                    else if (!controller.isRTA() && controller.getCurrentState() is EnemyRTAState)
                    {
                        controller.SwitchState(new EnemyAggroState(controller, e));
                    }
                }
                SwitchState(new EnemyTurnState(this, aggroEnemy));
            }
        }
    }
    public void SwitchState(TurnState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    public void addAggroEnemy(Enemy enemy)
    {
        aggroEnemy.Add(enemy);
    }
}
