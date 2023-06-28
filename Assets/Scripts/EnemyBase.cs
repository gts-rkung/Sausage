using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IBase
{
    public int hp = 20;
    ExplodeVfxPool explodeVfxPool;
    [SerializeField] TMPro.TextMeshPro hpText;
    [SerializeField] Transform spinTeeth;
    Global global;

    // Start is called before the first frame update
    void Start()
    {
        global = Global.singletonInstance;
        Debug.Assert(global, "global not found");
        explodeVfxPool = ExplodeVfxPool.singletonInstance;
        Debug.Assert(explodeVfxPool, "explodeVfxPool not found");
        Debug.Assert(hpText, "hpText not assigned");
        Debug.Assert(spinTeeth, "spinTeeth not assigned");
        hpText.text = hp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        spinTeeth.transform.Rotate(0f, 180f * Time.deltaTime, 0f);
    }

    public bool Damage(int pt = 1)
    {
        --hp;
        //print("damage " + name + " hp " + hp);
        hpText.text = hp.ToString();
        if (hp <= 0)
        {
            //print(" . destroy");
            explodeVfxPool.PlayExlode(transform.position);
            gameObject.SetActive(false);
            var ebs = FindObjectsOfType<EnemyBase>();
            if (ebs.Length <= 0)
            {
                global.Win();
            }
            return false;
        }
        explodeVfxPool.PlayHit(transform.position);
        return true;
    }
}
