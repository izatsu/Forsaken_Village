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
        else
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
            currentHealth -= damage;
            healthbar.value = currentHealth;

            if (currentHealth <= 0)
            {
                Debug.Log("Die");
            }
        }
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
