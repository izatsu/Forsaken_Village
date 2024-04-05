using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public static PlayerState instance { get; private set; }
    private PhotonView _view;
    
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;

    public bool isDie;

    public DissolvingControllerTut dissolvingControllerTut;

    public GameObject textDie;

    public Image lowHp;

    [SerializeField] private float timeHeal = 10f;
    [SerializeField] private GameObject audioLowHp;
    

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
        if(textDie != null)
            textDie.SetActive(false);
    }

    private void Start()
    {
        if (instance == null)
            instance = this;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth == 1)
        {
            lowHp.gameObject.GetComponent<Animator>().SetBool("lowHp", true);
            audioLowHp.SetActive(true);
            timeHeal -= Time.deltaTime;
            if (timeHeal <= 0)
            {
                currentHealth += 1;
                timeHeal = 10f;
            }
        }
        else
        {
            lowHp.gameObject.GetComponent<Animator>().SetBool("lowHp", false);
            audioLowHp.SetActive(false);
        }
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
        if(textDie != null)
            textDie.SetActive(true);
    }

    [PunRPC]
    void AnimationDead()
    {
        StartCoroutine(dissolvingControllerTut.DissolveCo());
    } 
    
} 
