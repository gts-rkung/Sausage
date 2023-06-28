using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    public static Global singletonInstance = null;
    [Header("----Generic----")]
    const int saveVer = 1;
    public bool inited;
    public int money = 50;
    int sceneIndex;
    string sceneName;
    public string SceneName
    {
        get
        {
            return sceneName;
        }
    }
    public float walkSpeed = 2f;
    public float basicHeight = 0.4f;
    public float slashPeriod = 0.4f;
    const float destroyAllFighterBaseDelay = 0.3f;
    public float inLineDistance = 0.6f;
    public float fightRange = 1.1f;
    public float sliceAddForceMin = 2f;
    public float sliceAddForceMax = 3f;
    public float disapperDistance = 8f;
    public enum State
    {
        //Waiting,
        Playing,
        Win,
        Lose,
    };
    public State state;
    [Header("----Demo And Debug----")]
    public bool moneyCheat;
    public bool horseCheat;
    public bool isReplay;
    [Header("----Items In Scene----")]
    public ParticleSystem confettiVfx;
    [Header("----Scene Specified Settings----")]
    public bool infiniteWaves;
    [System.Serializable]
    public struct EnemyWave
    {
        public float delay;
        public int count;
        public float minDur;
        public float maxDur;
        public float minLen;
        public float maxLen;
        public string columns;
        public EnemyWave(float d, int c, float d1, float d2, float l1, float l2, string cl)
        {
            delay = d;
            count = c;
            minDur = d1;
            maxDur = d2;
            minLen = l1;
            maxLen = l2;
            columns = cl;
        }
    }
    public EnemyWave[] enemyWaves;
    public EnemyWave[] enemyRunnerWaves;
    public EnemyWave[] enemyLancerWaves;
    public EnemyWave[] enemyBossWaves;

    UiSafeArea uiSafeArea;

    void Awake()
    {
        singletonInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
#if !(DEVELOPMENT_BUILD || UNITY_EDITOR)  // disable debug log if not in development build or editor
        Debug.unityLogger.logEnabled = false;
#endif
        */
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneName = SceneManager.GetActiveScene().name;

        AdjustCameraFieldOfView();

        if (PlayerPrefs.GetInt("saveVer", 1) != saveVer)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("saveVer", saveVer);
        }
        money = PlayerPrefs.GetInt("money", 50);
        if (moneyCheat)
        {
            money = 99999;
        }

        uiSafeArea = UiSafeArea.singletonInstance;
        Debug.Assert(uiSafeArea, "uiSafeArea not found");
        Debug.Assert(confettiVfx, "confettiVfx not assigned");


        YsoCorp.GameUtils.YCManager.instance.OnGameStarted(sceneIndex + 1);
    
        inited = true;
    }

    void AdjustCameraFieldOfView()
    {
        /* Auto adjust camera field of view according screen ratio
            9:20 = 0.45 -> FOV 60
            9:16 = 0.5625 -> FOV 51
        */
        float ratio = (float)Screen.width / (float)Screen.height;
        float fov = (ratio - 0.45f) / (0.5625f - 0.45f) * (51f - 60f) + 60f;
        fov = Mathf.Clamp(fov, 51f, 60f);
        print("Set main camera field of view to " + fov);
        Camera.main.fieldOfView = fov;

        /* for recording in landscape */
        /*
        if (Screen.width > Screen.height)
        {
            print("Landscape detected. Adjust cameral rotation and FOV");
            if (Camera.main.transform.eulerAngles.x > 16f)
            {
                Camera.main.transform.eulerAngles = new Vector3(16f, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
            }
            Camera.main.fieldOfView = 29f;
        }
        */
    }

    /*
    public void DelayLoadNextScene(float delay = 8f)
    {
        StartCoroutine(DelayLoad());

        IEnumerator DelayLoad()
        {
            yield return new WaitForSeconds(delay);
            int next = sceneIndex + 1;
            if (next >= SceneManager.sceneCountInBuildSettings)
            {
                next = 0;
            }
            SceneManager.LoadScene(next);
        }
    }
    */

    public void Win()
    {
        if (state != State.Playing)
        {
            return;
        }
        state = State.Win;
        YsoCorp.GameUtils.YCManager.instance.OnGameFinished(true);
        confettiVfx.Play();
        uiSafeArea.Win();
        EnemiesPool.singletonInstance.AllDie();
    }

    public void Lose()
    {
        if (state != State.Playing)
        {
            return;
        }
        state = State.Lose;
        YsoCorp.GameUtils.YCManager.instance.OnGameFinished(false);
        uiSafeArea.Lose();
        StartCoroutine(DestroyAllFigtherBases());

        IEnumerator DestroyAllFigtherBases()
        {
            var bases = FindObjectsOfType<FighterBase>();
            foreach(var b in bases)
            {
                if(b.gameObject.activeSelf)
                {
                    b.Damage(1000);
                    yield return new WaitForSeconds(destroyAllFighterBaseDelay);
                }
            }
        }
    }

    public void LoadScene(bool win)
    {
        PlayerPrefs.Save();
        int next = win ? sceneIndex + 1: sceneIndex;
        if (next >= SceneManager.sceneCountInBuildSettings)
        {
            next = 0;
        }
        SceneManager.LoadScene(next);
    }

    // useful function that returns either 1 or -1
    static public int RandomNegative()
    {
        return Random.Range(0, 2) == 0 ? -1 : 1;
    }
}
