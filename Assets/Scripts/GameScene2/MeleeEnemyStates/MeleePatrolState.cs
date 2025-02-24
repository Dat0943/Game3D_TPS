using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleePatrolState : MeleeEnemyBaseState
{
    float timer;

    List<Transform> wayPointsList = new List<Transform>();

    public override void EnterState(MeleeEnemyStateManager enemy)
    {
        if (enemy.enemyHealth.isDead) return;

        enemy.anim.SetBool("isPatroling", true);

        enemy.navMesh.speed = enemy.patrolSpeed;
        enemy.currentSpeed = enemy.navMesh.speed;
        timer = 0;

        // Waypoint
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in waypointCluster.transform)
        {
            wayPointsList.Add(t);
        }
        Vector3 nextPosition = wayPointsList[Random.Range(0, wayPointsList.Count)].position;
        enemy.navMesh.SetDestination(nextPosition);
    }

    public override void UpdateState(MeleeEnemyStateManager enemy)
    {
        if (enemy.enemyHealth.isDead) return;

        // Nếu agent di chuyển đến waypoint, di chuyển đến waypoint tiếp theo
        if (enemy.navMesh.remainingDistance <= enemy.navMesh.stoppingDistance)
        {
            Vector3 nextPosition = wayPointsList[Random.Range(0, wayPointsList.Count)].position;
            enemy.navMesh.SetDestination(nextPosition);
        }

        // Chuyển sang Idle
        timer += Time.deltaTime;
        if (timer > enemy.patrolingTime)
        {
            enemy.navMesh.SetDestination(enemy.navMesh.transform.position);
            ExitState(enemy, enemy.MeleeIdle);
        }

        // Chuyển sang chase
        if(FindNearestPlayer(enemy) != null)
        {
            enemy.player = FindNearestPlayer(enemy).transform;
            float distanceFromPlayer = Vector3.Distance(enemy.player.position, enemy.transform.position);

            if(distanceFromPlayer < enemy.detectionArea)
            {
                enemy.navMesh.SetDestination(enemy.navMesh.transform.position);
                enemy.SwitchState(enemy.MeleeChase);
            }
        }
    }

    void ExitState(MeleeEnemyStateManager enemy, MeleeEnemyBaseState state)
    {
        enemy.anim.SetBool("isPatroling", false);
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

            if(distaceToPlayer < shortestDistance)
            {
                shortestDistance = distaceToPlayer;
                nearestPlayer = player;
            }
        }
        return nearestPlayer;
    }
}
