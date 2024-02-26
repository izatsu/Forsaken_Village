using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState instance { get; private set; }
    private PhotonView _view;
    
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;

    public bool isDie;

    public DissolvingControllerTut dissolvingControllerTut;

    public GameObject textDie;

    private void OnEnable()
    {
        if(textDie != null)
            textDie.SetActive(false);
    }

    private void OnDisable()
    {
        if(textDie != null)
            textDie.SetActive(true);
    }

    private void Awake()
    {
        isDie = false;
        _view = GetComponent<PhotonView>();
        textDie = GameObject.FindGameObjectWithTag("TextDie");
        textDie.SetActive(false);
    }

    private void Start()
    {
        if (instance == null)
            instance = this;

        currentHealth = maxHealth;
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            _view.RPC(nameof(SetIsDie), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void SetIsDie()
    {
        _view.RPC(nameof(AnimationDead), RpcTarget.AllBuffered);
        StartCoroutine(PlayerDead());
        isDie = true;
        currentHealth = maxHealth;
    }

    IEnumerator PlayerDead()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<PlayerCamera>().newCam.SetActive(false);
        dissolvingControllerTut.Revival();
        gameObject.SetActive(false);
        textDie.SetActive(true);
    }

    [PunRPC]
    void AnimationDead()
    {
        StartCoroutine(dissolvingControllerTut.DissolveCo());
    } 
    
} 
