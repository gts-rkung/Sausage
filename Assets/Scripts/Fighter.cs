using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Sausage
{
    Enemy attackingEnemy = null;
    EnemyBase attackingBase = null;
    List<EnemyBase> enemyBasesList;

    // Start is called before the first frame update
    void Start()
    {
        InitCommon();
        defaultDirection = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = defaultDirection;
        enemyBasesList = new List<EnemyBase>(FindObjectsOfType<EnemyBase>());
        //Debug.Assert(enemyBasesList.Count > 0, "enemyBases not found");
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Moving:
                if (fightersPool.GetFighterInFront(transform.position) == null)
                {
                    transform.position += new Vector3(0f,
                        0f,
                        global.walkSpeed * Time.deltaTime);
                }
                if (transform.position.z > global.disapperDistance)
                {
                    gameObject.SetActive(false);
                    return;
                }
                TopSausageDropDown();
                var e = enemiesPool.GetEnemyInFightRange(transform.position);
                if (e != null)
                {
                    attackingEnemy = e;
                    attackingBase = null;
                    Turn(attackingEnemy.transform);
                    Fight();
                }
                else // if no nearby enemy, check base
                {
                    CheckEnemyBaseWithinAttackRange();
                }
                break;
            case State.Fighting:
                Fight();
                TopSausageDropDown();
                break;
            default:
                break;
        }
        Wiggle();
        stateTime += Time.deltaTime;
    }

    public void New()
    {
        InitCommon();
        transform.rotation = defaultDirection;
        topSausage.localPosition = new Vector3(0f, 0f, 0f);
        topHeadBone.localScale = new Vector3(1f, 1f, 1f);
        topRootBone.localScale = new Vector3(1f, 0f, 1f);
        attackingEnemy = null;
        attackingBase = null;
        dropSpeed = 0f;
        stateTime = 0f;
        state = State.GrowingUp;
        gameObject.SetActive(true);
    }

    public override void Fight()
    {
        if (state != State.Fighting)
        {
            // start fighting
            smanAnimator.SetBool("attack", true);
            smanAnimator.SetBool("run", false);
            dropSpeed = 0f;
            stateTime = 0f;
            state = State.Fighting;
            if (attackingEnemy != null)
            {
                attackingEnemy.Turn(transform);
                attackingEnemy.Fight();
                attackingBase = null;
            }
            return;
        }
        if (stateTime < global.slashPeriod)
        {
            return;
        }
        
        if (attackingEnemy != null)  // fighting an enemy
        {
            CutOnePieceOff();
            DieIfNotTallEnough();
            attackingEnemy.CutOnePieceOff();
            attackingEnemy.DieIfNotTallEnough();
            //print(name + " <color=red>reduced</color> " + transform.localScale.y + " " + enemy.transform.localScale.y);
            if (state == State.Fighting)
            {
                if (attackingEnemy.state == Enemy.State.Dead)
                {
                    transform.rotation = defaultDirection;
                    Walk();
                    attackingEnemy = null;
                }
            }
            else // dead
            {
                if (attackingEnemy.state == Enemy.State.Fighting)
                {
                    attackingEnemy.Walk();
                }
            }
        }
        else if (attackingBase != null)  // fighting a base
        {
            if (!attackingBase.Damage())
            {
                attackingBase = null;
                Walk();
            }
            // if enemy near by, turn to enemy
            var e = enemiesPool.GetEnemyInFightRange(transform.position);
            if (e != null)
            {
                attackingEnemy = e;
                attackingBase = null;
                Turn(attackingEnemy.transform);
                state = State.Moving;
                Fight();
            }
        }
        stateTime = 0f;
    }

    void CheckEnemyBaseWithinAttackRange()
    {
        foreach (var b in enemyBasesList)
        {
            if (!b.gameObject.activeSelf)
            {
                continue;
            }
            var bp = b.transform.position;
            if (Mathf.Abs(transform.position.x - bp.x) < 0.1f) // same column
            {
                if (Mathf.Abs(transform.position.z - bp.z) < 1.2f)
                {
                    attackingBase = b;
                    Turn(b.transform);
                    Fight();
                    return;
                }
            }
            else if (Mathf.Abs(transform.position.x - bp.x) < 1.1f) // nearby column
            {
                if (Mathf.Abs(transform.position.z - bp.z) < 0.2f)
                {
                    attackingBase = b;
                    Turn(b.transform);
                    Fight();
                    return;
                }
            }
        }
    }

    new public void DieIfNotTallEnough()
    {
        base.DieIfNotTallEnough();
        if (attackingEnemy != null &&
            attackingEnemy.gameObject.activeSelf)
        {
            attackingEnemy.Walk();
        }
    }
}
