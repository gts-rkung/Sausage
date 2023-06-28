using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Sausage
{
    public enum Type
    {
        Normal,
        Runner,
        Lancer,
        Boss,
    };
    Type type;
    public Type EnemyType
    {
        get
        {
            return type;
        }
    }
    Fighter attackingFighter = null;
    FighterBase attackingBase = null;
    List<FighterBase> fighterBasesList;

    // Start is called before the first frame update
    void Start()
    {
        InitCommon();
        defaultDirection = Quaternion.Euler(0f, 180f, 0f);
        transform.rotation = defaultDirection;
        fighterBasesList = new List<FighterBase>(FindObjectsOfType<FighterBase>());
        Debug.Assert(fighterBasesList.Count > 0, "fighterBases not found");
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Moving:
                if (enemiesPool.GetEnemyInFront(transform.position) == null)
                {
                    float speed = -global.walkSpeed;
                    if (type == Type.Runner)
                    {
                        speed *= 2;
                    }
                    transform.position += new Vector3(0f,
                        0f,
                        speed * Time.deltaTime);
                }
                if (transform.position.z < -global.disapperDistance)
                {
                    gameObject.SetActive(false);
                }
                TopSausageDropDown();
                CheckFighterBaseWithinAttackRange();
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

    public void New(Global.EnemyWave ew)
    {
        InitCommon();
        transform.rotation = defaultDirection;
        topSausage.localPosition = new Vector3(0f, 0f, 0f);
        topHeadBone.localScale = new Vector3(1f, 1f, 1f);
        topRootBone.localScale = new Vector3(1f, 0f, 1f);
        attackingFighter = null;
        attackingBase = null;
        dropSpeed = 0f;
        Grow(Random.Range(ew.minLen, ew.maxLen));
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
            return;
        }
        if (stateTime < global.slashPeriod)
        {
            return;
        }
        if (attackingBase != null)  // fighting a base
        {
            if (!attackingBase.Damage())
            {
                attackingBase = null;
                Walk();
            }
        }
        stateTime = 0f;
    }

    void CheckFighterBaseWithinAttackRange()
    {
        foreach (var b in fighterBasesList)
        {
            if (!b.gameObject.activeSelf)
            {
                continue;
            }
            var bp = b.transform.position;
            if (Mathf.Abs(transform.position.x - bp.x) < 0.1f) // same column
            { 
                if (Mathf.Abs(transform.position.z - bp.z) < 0.6f)
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
        if (enemiesPool.wavesRemaining <= 0 &&
            !enemiesPool.IsThereAnyActiveEnemy())
        {
            global.Win();
        }
    }
}
