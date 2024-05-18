using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class EnemyState : MonoBehaviour
{
    public static EnemyState instance { get; private set; }
    private PhotonView _view;
    
    public bool isDie;

    private EnemyController _enemyController;

    public bool playerAttack = false;

    private Animator _animator;

    [Header("HealthBar")]
    public Slider healthbar;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private float visionRange = 20f;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        isDie = false;
        _view = GetComponent<PhotonView>();
        _enemyController = GetComponent<EnemyController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.maxValue = maxHealth;
        healthbar.value = currentHealth;
    }

    private void Update()
    {
        if (PlayerInVisionRange())
        {
            healthbar.gameObject.SetActive(true);
        }
        else if(!PlayerInVisionRange() || isDie)
        {
            healthbar.gameObject.SetActive(false);
        }
    }


    public void TakeDamage(int damage)
    {
        if (!_enemyController.pray.checkBookFull)
        {
            playerAttack = true;
        }
        else
        {
            _view.RPC(nameof(Attacked), RpcTarget.AllBuffered, damage);

            if (currentHealth <= 0)
            {
                Debug.Log("Die");
                _view.RPC(nameof(Die), RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void Die()
    {
        isDie = true;
        _animator.SetBool("isDie", true);
        Invoke(nameof(SetActiveBoss), 5f);
    }
    [PunRPC]
    private void Attacked(int damage)
    {
        currentHealth -= damage;
        healthbar.value = currentHealth;
    }
    private void SetActiveBoss()
    {
        this.gameObject.SetActive(false);
    }
    
    bool PlayerInVisionRange()
    {
        //Tim tat ca gameobject co tag Player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= visionRange)
            {
                return true;
            }
        }
        return false;
    }
    
}
