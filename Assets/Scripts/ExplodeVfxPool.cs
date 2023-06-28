using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeVfxPool : MonoBehaviour
{
    public static ExplodeVfxPool singletonInstance = null;
    [SerializeField] ParticleSystem origHitVfx;
    [SerializeField] ParticleSystem origExplodeVfx;
    List<ParticleSystem> hitVfxList = new List<ParticleSystem>();
    List<ParticleSystem> explodeVfxList = new List<ParticleSystem>();

    void Awake()
    {
        singletonInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(origHitVfx, "origHitVfx not assigned");
        Debug.Assert(origExplodeVfx, "origExplodeVfx not assigned");
        hitVfxList.Add(origHitVfx);
        for (int i = 1; i < 12; i++)
        {
            var c = Instantiate(origHitVfx, Vector3.zero, origHitVfx.transform.rotation, origHitVfx.transform.parent);
            c.name = "Hit VFX " + i;
            hitVfxList.Add(c);
        }
        explodeVfxList.Add(origExplodeVfx);
        for (int i = 1; i < 6; i++)
        {
            var c = Instantiate(origExplodeVfx, Vector3.zero, origExplodeVfx.transform.rotation, origExplodeVfx.transform.parent);
            c.name = "Explode VFX " + i;
            explodeVfxList.Add(c);
        }
    }

    public void PlayExlode(Vector3 pos)
    {
        foreach (var v in explodeVfxList)
        {
            if (!v.isPlaying)
            {
                v.transform.position = pos;
                v.Play();
                return;
            }
        }
    }

    public void PlayHit(Vector3 pos)
    {
        foreach (var v in hitVfxList)
        {
            if (!v.isPlaying)
            {
                v.transform.position = pos;
                v.Play();
                return;
            }
        }
    }
}
