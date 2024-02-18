using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyVfx : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
