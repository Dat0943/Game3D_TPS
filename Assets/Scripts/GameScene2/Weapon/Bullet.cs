using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    float timer;

    [HideInInspector] public Vector3 direction;
    [HideInInspector] public WeaponManager weapon;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeToDestroy)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemy.TakeDamage(weapon.damage);
        }

        Destroy(gameObject);
    }
}
