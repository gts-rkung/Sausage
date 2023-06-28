using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour, ITrap
{
    [SerializeField] float rotateSpeed = 270f;
    bool toggle;
    FightersPool fightersPool;
    EnemiesPool enemiesPool;

    // Start is called before the first frame update
    void Start()
    {
        fightersPool = FindObjectOfType<FightersPool>();
        Debug.Assert(fightersPool, "fightersPool not found");
        enemiesPool = FindObjectOfType<EnemiesPool>();
        Debug.Assert(enemiesPool, "enemiesPool not found");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        toggle = !toggle;
        if (toggle)
        {
            HurtSausageInRange();
        }
    }

    public void HurtSausageInRange()
    {
        var pos = new Vector3(transform.position.x, 0f, transform.position.z);
        foreach (var s in fightersPool.GetList())
        {
            if (s.gameObject.activeSelf &&
                Vector3.Distance(s.transform.position, pos) < 1.1f)
            {
                s.HurtByObstacles(gameObject);
            }
        }
        /* TODO all lists */
        foreach (var s in enemiesPool.GetList())
        {
            if (s.gameObject.activeSelf &&
                Vector3.Distance(s.transform.position, pos) < 1.1f)
            {
                s.HurtByObstacles(gameObject);
            }
        }
    }
}
