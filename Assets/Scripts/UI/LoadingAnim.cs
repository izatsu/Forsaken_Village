using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnim : MonoBehaviour
{
    public float waitBetween = 0.15f;
    List<Animator> animators;
    void Start()
    {
        animators = new List<Animator>(GetComponentsInChildren<Animator>());
        StartCoroutine(DoAnimation());
    }
    
    IEnumerator DoAnimation()
    {
        while (true)
        {
            foreach (var animator in animators)
            {
                animator.SetTrigger("OnAnim");
                yield return new WaitForSeconds(waitBetween);
            }
        }
    }

}
