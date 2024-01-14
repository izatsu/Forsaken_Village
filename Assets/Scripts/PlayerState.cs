using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState instance { get; private set; }

    
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;

    public bool isDie;

    private void Awake()
    {
        isDie = false;
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
            isDie = true;
            //currentHealth = maxHealth;
            Debug.Log("Player isDie");
        }
    }
}
