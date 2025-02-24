using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeChaseState : MeleeEnemyBaseState
{
    public override void EnterState(MeleeEnemyStateManager enemy)
    {
        if (enemy.enemyHealth.isDead) return;

        enemy.anim.SetBool("isChasing", true);

        enemy.navMesh.speed = enemy.chaseSpeed;
        enemy.currentSpeed = enemy.navMesh.speed;
    }

    public override void UpdateState(MeleeEnemyStateManager enemy)
    {
        if (enemy.enemyHealth.isDead) return;

        if (FindNearestPlayer(enemy) != null)
        {
            enemy.player = FindNearestPlayer(enemy).transform;
            var playerController = enemy.player.GetComponent<MovementStateManager>();

            if (playerController != null)
            {
                enemy.navMesh.SetDestination(enemy.player.transform.position);
                enemy.transform.LookAt(enemy.player.transform);

                float distanceFromPlayer = Vector3.Distance(enemy.player.position, enemy.transform.position);

                if (distanceFromPlayer > enemy.stopChasingDistance)
                {
                    enemy.navMesh.SetDestination(enemy.navMesh.transform.position);
                    ExitState(enemy, enemy.MeleePatrol);
                }

                if (distanceFromPlayer < enemy.attackingDistance)
                {
                    enemy.navMesh.SetDestination(enemy.navMesh.transform.position);
                    enemy.SwitchState(enemy.MeleeAttack);
                }
            }
        }
    }

    void ExitState(MeleeEnemyStateManager enemy, MeleeEnemyBaseState state)
    {
        enemy.anim.SetBool("isChasing", false);
        enemy.SwitchState(state);
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
