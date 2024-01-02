using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Transform tranPlayer;
    [SerializeField] float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        tranPlayer = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (tranPlayer != null)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 direction = (tranPlayer.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
