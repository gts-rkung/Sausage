using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sausage : MonoBehaviour
{
    public enum State
    {
        GrowingUp,
        Moving,
        Fighting,
        Dead,
    }
    public State state;
    public float stateTime;
    bool inited;
    protected Quaternion defaultDirection;
    protected float dropSpeed;
    protected Global global;
    protected FightersPool fightersPool;
    protected EnemiesPool enemiesPool;
    protected SausageSlicesPool sausageSlicesPool;
    public Transform topSausage;
    public Transform topRootBone;
    public Transform topSpine1Bone;
    public Transform topSpine2Bone;
    public Transform topHeadBone;
    public Transform bottomBone;
    public Transform face;
    public Animator smanAnimator;
    SplashVfxPool splashVfxPool;
    float lastHitObstacle;

    protected void InitCommon()
    {
        if (inited)
        {
            return;
        }
        global = Global.singletonInstance;
        Debug.Assert(global, "global not found");
        
        Debug.Assert(topHeadBone, "topHeadBone not assigned");
        Debug.Assert(topRootBone, "topRootBone not assigned");
        Debug.Assert(topSpine1Bone, "topSpine1Bone not assigned");
        Debug.Assert(topSpine2Bone, "topSpine2Bone not assigned");
        Debug.Assert(bottomBone, "bottomBone not assigned");
        Debug.Assert(smanAnimator, "smanAnimator not assigned");

        fightersPool = FightersPool.singletonInstance;
        Debug.Assert(fightersPool, "fightersPool not found");
        enemiesPool = EnemiesPool.singletonInstance;
        Debug.Assert(enemiesPool, "enemiesPool not found");
        splashVfxPool = SplashVfxPool.singletonInstance;
        Debug.Assert(splashVfxPool, "splashVfxPool not found");
        sausageSlicesPool = SausageSlicesPool.singletonInstance;
        Debug.Assert(sausageSlicesPool, "sausageSlicesPool not found");
        inited = true;
    }

    public void Grow(float s)
    {
        topRootBone.localScale += new Vector3(0f,
                s * 1f,
                0f);
        if (topRootBone.localScale.y > 0.3f)
        {
            topHeadBone.localScale = new Vector3(1f,
                1f / topRootBone.localScale.y,
                1f);
        }
    }

    public void Walk()
    {
        if (DieIfNotTallEnough())
        {
            return;
        }
        transform.position -= new Vector3(0f, transform.position.y, 0f);
        Turn();
        smanAnimator.SetBool("attack", false);
        smanAnimator.SetBool("run", true);
        stateTime = 0f;
        state = State.Moving;
    }

    public virtual void Fight()
    {
    }

    public void Turn(Transform tf = null)
    {
        if (tf != null)
        {
            transform.LookAt(tf);
        }
        else
        {
            transform.rotation = defaultDirection;
        }
    }

    public void Die()
    {
        splashVfxPool.PlayVfx(GetType(), transform.position);
        state = State.Dead;
        gameObject.SetActive(false);
    }

    protected bool DieIfNotTallEnough()
    {
        if (topRootBone.localScale.y < global.basicHeight * 0.5f)
        {
            Die();
            return true;
        }
        return false;
    }

    public void CutOnePieceOff()
    {
        float y = topRootBone.localScale.y;
        y -= global.basicHeight;
        topRootBone.localScale = new Vector3(1f,
            y,
            1f);
        if (y > 0.1f)
        {
            topHeadBone.localScale = new Vector3(1f,
                1f / y,
                1f);
        }
        else
        {
            topHeadBone.localScale = new Vector3(1f,
                1f,
                1f);
        }
        topSausage.localPosition = new Vector3(0f,
            global.basicHeight,
            0f);
        dropSpeed = 0f;
        DieIfNotTallEnough();
        sausageSlicesPool.Throw(GetType(), transform.position, transform.forward);
    }

    protected void TopSausageDropDown()
    {
        if (topSausage.localPosition.y > 0f)
        {
            dropSpeed += 9.8f * Time.deltaTime;
            topSausage.localPosition -= new Vector3(0f, dropSpeed * Time.deltaTime, 0f);
        }
        else
        {
            topSausage.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    protected void Wiggle()
    {
        float h = Mathf.Clamp(topRootBone.localScale.y, 0.1f, 1f) * 1.2f;
        float s = Mathf.Sin(Time.time * 6.28f) * h;
        topRootBone.localRotation = Quaternion.Euler(90f + 3f * s, 0f, 0f);
        topSpine1Bone.localRotation = Quaternion.Euler(9f * s, 0f, 0f);
        topSpine2Bone.localRotation = Quaternion.Euler(3f * s, 0f, 0f);
        topHeadBone.localRotation = Quaternion.Euler(-6f * s, 0f, 0f);
    }

    public void HurtByObstacles(GameObject obj)
    {
        if (Time.time - lastHitObstacle > 0.4)
        {
            lastHitObstacle = Time.time;
            CutOnePieceOff();
        }
    }
}
