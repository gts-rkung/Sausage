using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBase : MonoBehaviour, IBase
{
    public int hp = 20;
    public bool isPressed;
    FightersPool fightersPool;
    Fighter currentFighter;
    ExplodeVfxPool explodeVfxPool;
    [SerializeField] Transform button;
    Vector3 initButtonPosition;
    Global global;

    // Start is called before the first frame update
    void Start()
    {
        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global not found");
        Debug.Assert(button, "button not assigned");
        initButtonPosition = button.localPosition;
        fightersPool = FindObjectOfType<FightersPool>();
        Debug.Assert(fightersPool, "fightersPool not found");
        explodeVfxPool = FindObjectOfType<ExplodeVfxPool>();
        Debug.Assert(explodeVfxPool, "explodeVfxPool not found");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            if (currentFighter)
            {
                currentFighter.Grow(Time.deltaTime);
            }
        }
    }

    void OnMouseDown()
    {
        currentFighter = fightersPool.Spawn(transform.position);
        button.localPosition = initButtonPosition + new Vector3(0f, -0.5f, 0f);
        isPressed = true;
    }

    void OnMouseUp()
    {
        NoLongerPressed();
    }

    void OnMouseExit()
    {
        NoLongerPressed();
    }

    void NoLongerPressed()
    {
        if (currentFighter)
        {
            currentFighter.Walk();
            currentFighter = null;
        }
        button.localPosition = initButtonPosition;
        isPressed = false;
    }

    public bool Damage(int pt = 1)
    {
        hp -= pt;
        //print("damage " + name + " hp " + hp);
        if (hp <= 0)
        {
            //print(" . destroy");
            explodeVfxPool.PlayExlode(transform.position);
            global.Lose();
            gameObject.SetActive(false);
            return false;
        }
        explodeVfxPool.PlayHit(transform.position);
        return true;
    }
}
