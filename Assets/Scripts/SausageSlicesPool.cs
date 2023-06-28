using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SausageSlicesPool : MonoBehaviour
{
    public static SausageSlicesPool singletonInstance = null;
    [SerializeField] SausageSlice origFighterSlice;
    [SerializeField] SausageSlice origEnemySlice;
    List<SausageSlice> fighterSlicesList = new List<SausageSlice>();
    List<SausageSlice> enemySlicesList = new List<SausageSlice>();
    const int maxNumber = 20;
    public bool testThrow;

    void Awake()
    {
        singletonInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(origFighterSlice, "origFighterSlice not assigned");
        Debug.Assert(origEnemySlice, "origEnemySlice not assigned");
        origFighterSlice.gameObject.SetActive(false);
        origEnemySlice.gameObject.SetActive(false);

        fighterSlicesList.Add(origFighterSlice);
        for (int i = 1; i < maxNumber; i++)
        {
            var c = Instantiate(origFighterSlice, Vector3.zero, Quaternion.identity, transform);
            c.name = "Fighter Slice " + i;
            fighterSlicesList.Add(c);
        }
        enemySlicesList.Add(origEnemySlice);
        for (int i = 1; i < maxNumber; i++)
        {
            var c = Instantiate(origEnemySlice, Vector3.zero, Quaternion.identity, transform);
            c.name = "Enemy Slice " + i;
            enemySlicesList.Add(c);
        }

    }

    public void Throw(System.Type type, Vector3 pos, Vector3 dir)
    {
        Debug.Assert(type == typeof(Fighter) || type == typeof(Enemy), "unknown class " + type);
        var list = (type == typeof(Fighter)) ? fighterSlicesList : enemySlicesList;
        foreach (var s in list)
        {
            if (!s.gameObject.activeSelf)
            {
                s.transform.position = pos + new Vector3(0f, 0.35f, 0f);
                s.Throw(-dir);
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (testThrow)
        {
            Throw(typeof(Fighter), new Vector3(Random.Range(-2f, 2f), 0f, 0f), Vector3.one);
            testThrow = false;
        }
    }
}
