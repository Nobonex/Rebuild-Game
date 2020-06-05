using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemyController : MonoBehaviour
{
    public float LookRadius = 10f;

    private Transform target;

    private List<Transform> wanderPoints;
    private Transform currentWanderTarget;
    
    private NavMeshAgent agent;
    
    void Start()
    {
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint").Select(x => x.transform).ToList();
        ChooseWanderTarget();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(target.position, transform.position);

        if (distance <= LookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget(target.position);
            }
            
        }
        else
        {
            Wander();
            FaceTarget(currentWanderTarget.position);
        }
    }

    private void Wander()
    {
        if (Vector3.Distance(currentWanderTarget.position, transform.position) <= LookRadius/2)
        {
            ChooseWanderTarget();
        }

        agent.SetDestination(currentWanderTarget.position);
    }

    private void ChooseWanderTarget()
    {
        var random = new Random();
        currentWanderTarget = wanderPoints[random.Next(wanderPoints.Count)];
    }
    
    private void FaceTarget(Vector3 targetPosition)
    {
        var direction = (targetPosition - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}
