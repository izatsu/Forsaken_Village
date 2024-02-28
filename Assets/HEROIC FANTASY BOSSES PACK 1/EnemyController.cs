
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviourPunCallbacks
{

    [Header("Move Points")]
    [SerializeField] Transform[] movePoints;

    [Header("Vision Range")]
    [SerializeField] float visionRange = 5f;
    [SerializeField] float returnTimeOutSize = 20f;

    [Header("Attack Settings")]
    [SerializeField] float attackRange1 = 3f;
    [SerializeField] float attackRange2 = 5f;
    [SerializeField] float attackCooldown = 1f;


    [Header("Animation")]
    [SerializeField] Animator animator;

    [Header("LookAt Target")]
    [SerializeField] LookAtTarget lookAtTargetScript;

    [Header("VFX Attack")]
    [SerializeField] GameObject vfxAttackPrefab;
    [SerializeField] Transform vfxTarget;
    [SerializeField] GameObject vfxEnemySkill;
    [SerializeField] Transform vfxEnemySkillTransform;

    NavMeshAgent navMeshAgent;
    int currentMovePoint;
    bool playerInSight;
    float timePlayerLeftSight;

    bool isAttacking;
    float lastAttackTime;


    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentMovePoint = Random.Range(0, movePoints.Length);
        SetNextDestination();
    }

    void Update()
    {

        /*if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            SetNextDestination();
        }*/           
        if (PlayerInVisionRange())
        {
            timePlayerLeftSight = 0f;
            playerInSight = true;
            FollowOrAttackPlayer();
        }
        else if (playerInSight && !PlayerInVisionRange())
        {
            PlayerLeftSight();
        }
        else
        {
            if(Vector3.Distance(transform.position, navMeshAgent.destination)<0.1f)
            {
                SetNextDestination();
            }
            
        }
    }
    void SetNextDestination()
    {
        currentMovePoint = GetRandomMovePoint();
        Vector3 targetPosition = movePoints[currentMovePoint].position;
        navMeshAgent.SetDestination(targetPosition);
    }

    int GetRandomMovePoint()
    {
        int newPoint = currentMovePoint;
        while (newPoint == currentMovePoint)
        {
            newPoint = Random.Range(0, movePoints.Length);
        }
        return newPoint;
    }

    bool PlayerInVisionRange()
    {
        //Tim tat ca gameobject co tag Player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= visionRange)
            {
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                return true;
            }
        }
        return false;
    }

    void FollowOrAttackPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestPlayer = null;
        // Tinh toan lay khoang cach cua player gan nhat de attack
        float minDistance = Mathf.Infinity;

        //Chay vong lap de kiem tra player gan nhat
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = player;
            }
        }

        //Sau khi lay duoc player gan nhat thi tan cong
        if (nearestPlayer != null)
        {
            //navMeshAgent.SetDestination(nearestPlayer.transform.position);
            if (minDistance <= attackRange1 && Time.time - lastAttackTime > attackCooldown)
            {
                Attack(1,nearestPlayer.transform);
            }
            else if (minDistance > attackRange1 && minDistance <= attackRange2)
            {
                Attack(2,nearestPlayer.transform);
            }
            else
            {
                navMeshAgent.SetDestination(nearestPlayer.transform.position);
            }
        }
    }

    private void Attack(int attackType, Transform playerTransform)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            navMeshAgent.isStopped = true;
            StartCoroutine(AttackAnimationCoroutine(attackType,playerTransform));
        }
       // navMeshAgent.SetDestination(playerTransform.position);
    }

    IEnumerator AttackAnimationCoroutine(int attackType, Transform playerTransform)
    {
        Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
        lookAtTargetScript.SetTarget(playerTransform);

        if (attackType == 1)
        {
            animator.SetBool("Attack1", true);
        }
        else if (attackType == 2)
        {
            animator.SetBool("Attack2", true);
            GameObject vfxSkill = Instantiate(vfxEnemySkill, vfxEnemySkillTransform.position, Quaternion.identity);
            Destroy(vfxSkill, 2);
            yield return new WaitForSeconds(1.8f);
            GameObject attackVFX = Instantiate(vfxAttackPrefab, vfxTarget.position, Quaternion.identity);
            attackVFX.transform.forward = transform.forward;
            //GameObject attackVFX = Instantiate(vfxAttackPrefab, transform.position, Quaternion.identity);
            Destroy(attackVFX, 5);
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

        // Dat lai animation va tiep tuc di chuyen theo diem
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        isAttacking = false;
        navMeshAgent.isStopped = false;
        // lastAttackTime = Time.time;
        lookAtTargetScript.SetTarget(null);
    }

    void PlayerLeftSight()
    {
        if(!PlayerInVisionRange())
        {
            timePlayerLeftSight += Time.deltaTime;
            if (timePlayerLeftSight >= returnTimeOutSize)
            {
                playerInSight = false;
                timePlayerLeftSight = 0f;
                SetNextDestination();

            }
            else
            {
                FollowOrAttackPlayer();
            }
        }
        
       
    }

}
