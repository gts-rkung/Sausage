using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightersPool : MonoBehaviour
{
    public static FightersPool singletonInstance = null;
    [SerializeField] Fighter originalFighter;
    List<Fighter> list = new List<Fighter>();
    Global global;
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
        Debug.Assert(originalFighter, "originalFighter not assigned");
        originalFighter.gameObject.SetActive(false);
        list.Add(originalFighter);
        for (int i = 1; i < maxNumber; i++)
        {
            var c = Instantiate(originalFighter, Vector3.zero, originalFighter.transform.rotation, originalFighter.transform.parent);
            c.name = "Fighter " + i;
            list.Add(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Fighter Spawn(Vector3 position)
    {
        foreach (var c in list)
        {
            if (!c.gameObject.activeSelf)
            {
                c.transform.position = position + new Vector3(0f, -0.5f, 0f);
                c.New();
                return c;
            }
        }
        return null;
    }

    public Fighter GetFighterInFront(Vector3 position)
    {
        foreach (var c in list)
        {
            if (c.gameObject.activeSelf &&
                Mathf.Abs(c.transform.position.x - position.x) < 0.01f &&
                c.transform.position.z > position.z &&
                c.transform.position.z - position.z < 1f) 
            {
                return c;
            }
        }
        return null;
    }
        
    public List<Fighter> GetList()
    {
        return list;
    }
}
