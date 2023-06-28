using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageSlice : MonoBehaviour
{
    Rigidbody mRigidbody;
    Global global;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        global ??= FindObjectOfType<Global>();
        mRigidbody ??= GetComponent<Rigidbody>();
        mRigidbody.velocity = Vector3.zero;
        mRigidbody.angularVelocity = Vector3.zero;
    }

    public void Throw(Vector3 dir)
    {
        Init();
        //transform.localScale = new Vector3(0.3f, Random.Range(0.3f, 0.4f), 0.3f);
        gameObject.SetActive(true);
        StartCoroutine(AutoDeactive());
        var force = dir * Random.Range(global.sliceAddForceMin, global.sliceAddForceMax) +
            new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 2f), 0f);
        mRigidbody.AddForce(force, ForceMode.Impulse);
    }

    IEnumerator AutoDeactive()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
