using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoFarBuildings : MonoBehaviour
{
    [SerializeField] int rows = 5, columns = 5;
    [SerializeField] Vector3 scale = new Vector3(2f, 16f, 2f);
    [SerializeField][Range(0f, 1f)] float scaleVariation = 0.5f;
    [SerializeField] float gap = 4f;
    [SerializeField][Range(0f, 1f)] float gapVariation = 0.5f;
    [SerializeField] bool trigger = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!trigger)
        {
            return;
        }
        try
        {
            Debug.Assert(transform.childCount >= 1, "must have a child");
            while (transform.childCount > 1)
            {
                DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);
            }
            var cube = transform.GetChild(0);
            cube.localPosition = Vector3.zero + RandomGaps();
            cube.localScale = scale + RandomScales();
            for (int i = 1; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            for (int x = 0; x < rows; x++)
            {
                for (int z = 0; z < columns; z++)
                {
                    var clone = Instantiate(cube,
                        Vector3.zero,
                        Quaternion.identity,
                        transform);
                    clone.name = cube.name + " " + x + "," + z;
                    clone.localPosition = new Vector3(x * gap, 0f, z * gap) + RandomGaps();
                    clone.localScale = scale + RandomScales();
                }
            }
            trigger = false;
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
            trigger = false;
        }
        
        Vector3 RandomGaps()
        {
            return new Vector3(Random.Range(-gapVariation, gapVariation),
                Random.Range(-gapVariation, gapVariation),
                Random.Range(-gapVariation, gapVariation));
        }

        Vector3 RandomScales()
        {
            return new Vector3(Random.Range(-scaleVariation, scaleVariation),
                Random.Range(-scaleVariation, scaleVariation),
                Random.Range(-scaleVariation, scaleVariation));
        }
    }
}
