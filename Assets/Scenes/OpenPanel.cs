using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
public class OpenPanel : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
       
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Debug.Log("Da bat");
    }
    private void OnEnable()
    {
        
        animator.Play("PanelOpen");
        
    }
}
