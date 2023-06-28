using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesPool : MonoBehaviour
{
    public static EnemiesPool singletonInstance = null;
    [SerializeField] Enemy originalEnemy;
    [SerializeField] Enemy originalRunner = null;
    [SerializeField] Enemy originalLancer = null;
    [SerializeField] Enemy originalBoss = null;
    List<Enemy> listNormal = new List<Enemy>();
    List<Enemy> listRunner = new List<Enemy>();
    List<Enemy> listLancer = new List<Enemy>();
    List<Enemy> listBoss = new List<Enemy>();
    Global global;
    public int wavesRemaining = 0;
    const int maxNumber = 20;

    void Awake()
    {
        singletonInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        global = Global.singletonInstance;
        Debug.Assert(global, "global not found");
        Debug.Assert(originalEnemy, "originalEnemy not assigned");
        originalEnemy.gameObject.SetActive(false);
        listNormal.Add(originalEnemy);
        for (int i = 1; i < maxNumber; i++)
        {
            var c = Instantiate(originalEnemy, Vector3.zero, originalEnemy.transform.rotation, originalEnemy.transform.parent);
            c.name = "Enemy " + i;
            listNormal.Add(c);
        }
        wavesRemaining++;
        StartCoroutine(AutoSpawn(Enemy.Type.Normal));

        if (originalRunner)
        {
            originalRunner.gameObject.SetActive(false);
            listRunner.Add(originalRunner);
            for (int i = 1; i < maxNumber; i++)
            {
                var c = Instantiate(originalRunner, Vector3.zero, originalRunner.transform.rotation, originalRunner.transform.parent);
                c.name = "Enemy Runner " + i;
                listRunner.Add(c);
            }
            wavesRemaining++;
            StartCoroutine(AutoSpawn(Enemy.Type.Runner));
        }
    }

    public Enemy Spawn(Enemy.Type type, Vector3 position, Global.EnemyWave ew)
    {
        var list = listNormal;
        if (type == Enemy.Type.Runner)
            list = listRunner;
        else if (type == Enemy.Type.Lancer)
            list = listLancer;
        else if (type == Enemy.Type.Boss)
            list = listBoss;

        foreach (var c in list)
        {
            if (!c.gameObject.activeSelf)
            {
                c.transform.position = position;
                c.New(ew);
                c.Walk();
                return c;
            }
        }
        return null;
    }

    IEnumerator AutoSpawn(Enemy.Type type)
    {
        Global.EnemyWave[] waves = global.enemyWaves;
        if (type == Enemy.Type.Runner)
            waves = global.enemyRunnerWaves;
        else if (type == Enemy.Type.Lancer)
            waves = global.enemyLancerWaves;
        else if (type == Enemy.Type.Boss)
            waves = global.enemyBossWaves;

        do
        {
            for (int i = 0; i < waves.Length; i++)
            {
                var w = waves[i];
                yield return new WaitForSeconds(w.delay);
                for (int j = 0; j < w.count; j++)
                {
                    if (global.state != Global.State.Playing)
                    {
                        yield break;
                    }
                    string c = w.columns[Random.Range(0, w.columns.Length)].ToString();
                    int x = System.Convert.ToInt32(c) - 2;
                    Spawn(type, new Vector3(x, 0, global.disapperDistance), w);
                    if (j < w.count - 1)
                    {
                        yield return new WaitForSeconds(Random.Range(w.minDur, w.maxDur));
                    }
                }
            }
        } while (global.infiniteWaves);
        wavesRemaining--;
    }

    public Enemy GetEnemyInFightRange(Vector3 position)
    {
        List<Enemy>[] lists = { listBoss, listLancer, listRunner, listNormal };
        foreach (var l in lists)
        {
            foreach (var c in l)
            {
                if (c.gameObject.activeSelf &&
                    Vector3.Distance(c.transform.position, position) < global.fightRange)
                {
                    return c;
                }
            }
        }
        return null;
    }

    public Enemy GetEnemyInFront(Vector3 position)
    {
        List<Enemy>[] lists = { listBoss, listLancer, listRunner, listNormal };
        foreach (var l in lists)
        {
            foreach (var c in l)
            {
                if (c.gameObject.activeSelf &&
                    Mathf.Abs(c.transform.position.x - position.x) < 0.01f &&
                    position.z > c.transform.position.z &&
                    position.z - c.transform.position.z < global.inLineDistance)
                {
                    return c;
                }
            }
        }
        return null;
    }

    public bool IsThereAnyActiveEnemy()
    {
        List<Enemy>[] lists = { listBoss, listLancer, listRunner, listNormal };
        foreach (var l in lists)
        {
            foreach (var c in l)
            {
                if (c.gameObject.activeSelf)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /* TODO return all lists */
    public List<Enemy> GetList()
    {
        return listNormal;
    }

    public void AllDie()
    {
        List<Enemy>[] lists = { listBoss, listLancer, listRunner, listNormal };
        foreach (var l in lists)
        {
            foreach (var c in l)
            {
                if (c.gameObject.activeSelf)
                {
                    c.Die();
                }
            }
        }
    }
}
