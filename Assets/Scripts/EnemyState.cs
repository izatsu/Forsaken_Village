using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public static EnemyState instance { get; private set; }
    private PhotonView _view;
    
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;

    public bool isDie;

    private EnemyController _enemyController;

    public bool playerAttack = false;
    
    private void Awake()
    {
        isDie = false;
        _view = GetComponent<PhotonView>();
        _enemyController = GetComponent<EnemyController>();
    }

    private void Start()
    {
        if (instance == null)
            instance = this;

        currentHealth = maxHealth;
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

            if (currentHealth <= 0)
            {
                Debug.Log("Die");
            }
        }
    }
    
}
