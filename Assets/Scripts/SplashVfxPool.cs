using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashVfxPool : MonoBehaviour
{
    public static SplashVfxPool singletonInstance = null;
    [SerializeField] ParticleSystem origFighterSplash;
    [SerializeField] ParticleSystem origEnemySplash;
    List<ParticleSystem> fighterVfxList = new List<ParticleSystem>();
    List<ParticleSystem> enemyVfxList = new List<ParticleSystem>();

    void Awake()
    {
        singletonInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(origFighterSplash, "origFighterSplash not assigned");
        Debug.Assert(origEnemySplash, "origEnemySplash not assigned");
        fighterVfxList.Add(origFighterSplash);
        for (int i = 1; i < 20; i++)
        {
            var c = Instantiate(origFighterSplash, Vector3.zero, origFighterSplash.transform.rotation, origFighterSplash.transform.parent);
            c.name = "Fighter Splash " + i;
            fighterVfxList.Add(c);
        }
        enemyVfxList.Add(origEnemySplash);
        for (int i = 1; i < 20; i++)
        {
            var c = Instantiate(origEnemySplash, Vector3.zero, origEnemySplash.transform.rotation, origEnemySplash.transform.parent);
            c.name = "Enemy Splash " + i;
            enemyVfxList.Add(c);
        }
    }

    public void PlayVfx(System.Type type, Vector3 pos)
    {
        Debug.Assert(type == typeof(Fighter) || type == typeof(Enemy), "unknown class " + type);
        var list = (type == typeof(Fighter)) ? fighterVfxList : enemyVfxList;
        foreach (var v in list)
        {
            if (!v.isPlaying)
            {
                v.transform.position = pos + new Vector3(0f, 1f, 0f);
                v.Play();
                return;
            }
        }
    }
}
