using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeIdleState : MeleeEnemyBaseState
{
    float timer;

    public override void EnterState(MeleeEnemyStateManager enemy)
    {
        timer = 0;
    }

    public override void UpdateState(MeleeEnemyStateManager enemy)
    {
        if (enemy.enemyHealth.isDead) return;

        // Chuyển sang patrol
        timer += Time.deltaTime;
        if (timer > enemy.idleTime)
        {
            enemy.SwitchState(enemy.MeleePatrol);
        }

        // Chuyển sang chase
        if(FindNearestPlayer(enemy) != null)
        {
            enemy.player = FindNearestPlayer(enemy).transform;
            float distanceFromPlayer = Vector3.Distance(enemy.player.position, enemy.transform.position);

            if(distanceFromPlayer < enemy.detectionArea)
            {
                enemy.SwitchState(enemy.MeleeChase);
            }
        }
    }

    GameObject FindNearestPlayer(MeleeEnemyStateManager enemy)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        foreach (var player in players)
        {
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);

            if(distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player;
            }
        }
        return nearestPlayer;
    }
}
