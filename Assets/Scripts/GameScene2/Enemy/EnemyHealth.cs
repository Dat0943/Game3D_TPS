using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health;
    [HideInInspector] public bool isDead;

    MeleeEnemyStateManager meleeEnemy;
    Coroutine resetSpeedCoroutine;

    void Awake()
    {
        meleeEnemy = GetComponent<MeleeEnemyStateManager>();
    }

    public void TakeDamage(float damage)
    {
        if(health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                EnemyDeath();
            }
            else
            {
                meleeEnemy.navMesh.speed = meleeEnemy.knockBackSpeed;

                if (resetSpeedCoroutine != null)
                {
                    StopCoroutine(resetSpeedCoroutine);
                }
                resetSpeedCoroutine = StartCoroutine(ResetSpeedAfterDelay());
            }
        }
    }

    IEnumerator ResetSpeedAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        meleeEnemy.navMesh.speed = meleeEnemy.currentSpeed;
    }

    void EnemyDeath()
    {
        isDead = true;
        meleeEnemy.navMesh.isStopped = true;
        meleeEnemy.anim.enabled = false;
        meleeEnemy.TriggerRagdoll();
        Destroy(gameObject, 5f);
    }
}
