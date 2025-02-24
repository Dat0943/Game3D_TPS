using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackState : MeleeEnemyBaseState
{
    public override void EnterState(MeleeEnemyStateManager enemy)
    {
        if (enemy.enemyHealth.isDead) return;

        enemy.anim.SetBool("isAttacking", true);

        enemy.player = FindNearestPlayer(enemy).transform;
        LookAtPlayer(enemy);
    }

    public override void UpdateState(MeleeEnemyStateManager enemy)
    {
        if (enemy.enemyHealth.isDead) return;

        LookAtPlayer(enemy);

        if (FindNearestPlayer(enemy) != null)
        {
            enemy.player = FindNearestPlayer(enemy).transform;
            float distanceFromPlayer = Vector3.Distance(enemy.player.position, enemy.transform.position);
            if (distanceFromPlayer > enemy.stopAttackingDistance)
            {
                ExitState(enemy, enemy.MeleeChase);
            }
        }
    }

    void ExitState(MeleeEnemyStateManager enemy, MeleeEnemyBaseState state)
    {
        enemy.anim.SetBool("isAttacking", false);
        enemy.SwitchState(state);
    }

    void LookAtPlayer(MeleeEnemyStateManager enemy)
    {
        Vector3 direction = enemy.player.position - enemy.navMesh.transform.position;
        enemy.navMesh.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = enemy.navMesh.transform.eulerAngles.y;
        enemy.navMesh.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    GameObject FindNearestPlayer(MeleeEnemyStateManager enemy)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        foreach (var player in players)
        {
            float distaceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);

            if (distaceToPlayer < shortestDistance)
            {
                shortestDistance = distaceToPlayer;
                nearestPlayer = player;
            }
        }
        return nearestPlayer;
    }
}
