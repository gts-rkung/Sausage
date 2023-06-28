using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI total;
    [SerializeField] TMPro.TextMeshProUGUI earning0;
    [SerializeField] TMPro.TextMeshProUGUI bigEarning;
    List<TMPro.TextMeshProUGUI> earningList = new List<TMPro.TextMeshProUGUI>();
    Vector3 earningInitPos;
    [SerializeField] int earningShitXRange = 30;
    Global global;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global not found");
        yield return new WaitUntil(() => global.inited);
        Debug.Assert(total, "total not assigned");
        total.text = global.money.ToString();
        Debug.Assert(earning0, "earning0 not assigned");
        Debug.Assert(bigEarning, "bigEarning not assigned");
        earningInitPos = earning0.transform.position;
        for (int i = 1; i < 10; i++)
        {
            var clone = Instantiate(earning0, earning0.transform.position, Quaternion.identity, transform);
            clone.name = "Earning (" + i + ")";
            earningList.Add(clone);
        }
    }
}
