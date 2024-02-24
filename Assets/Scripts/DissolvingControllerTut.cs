using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingControllerTut : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect vfxGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    
    private Material[] _skinnedMaterials;

    private void Start()
    {
        if (skinnedMesh != null)
            _skinnedMaterials = skinnedMesh.materials;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(DissolveCo());
        }
    }

    public IEnumerator DissolveCo()
    {
        if (vfxGraph != null)
        {
            vfxGraph.Play();
        }
        if (_skinnedMaterials.Length > 0)
        {
            float counter = 0;
            while (_skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0;i  < _skinnedMaterials.Length; i++)
                {
                    _skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
