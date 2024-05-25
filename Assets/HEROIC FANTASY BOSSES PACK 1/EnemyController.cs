
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviourPunCallbacks
{

    [Header("Move Points")]
    [SerializeField] Transform[] movePoints;

    [Header("Vision Range")]
    [SerializeField] float visionRange = 5f;
    [SerializeField] float returnTimeOutSize = 20f;

    [Header("Attack Settings")]
    [SerializeField] float attackRange = 3f;
    [SerializeField] float attackRange2 = 7f;
    [SerializeField] float attackCooldown = 3f;
    [SerializeField] float comboAttackCooldown = 2f;
    [SerializeField] float comboAttackDuration = 2.5f;
    [SerializeField] float attackSkillCooldown = 10f;



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

    private bool isComboAttacking;
    private bool canRandomAttack = true;
    private float lastAttackSkillTime;
    public bool checkAttackSkill = false;


    [Header("Sound VFX")] 
    private AudioSource _audioSource;
    [SerializeField] private AudioClip soundMove; 
    [SerializeField] private AudioClip soundMoveToPlayer;

    public Pray pray;

     public float timeDelayAttack = 10f;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        currentMovePoint = Random.Range(0, movePoints.Length);
        SetNextDestination();
    }

    void Update()
    {

        /*if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            SetNextDestination();
        }*/
        if (pray.checkBookFull && !EnemyState.instance.isDie)
        {
            FollowOrAttackPlayer();
        }
        else
        {
            if (PlayerInVisionRange() || EnemyState.instance.playerAttack)
            {
                EnemyState.instance.playerAttack = false;
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

            if (navMeshAgent.velocity.magnitude > 0.1f)
            {
                if (!playerInSight)
                {
                    if (_audioSource.clip != soundMove)
                    {
                        _audioSource.enabled = false;
                        _audioSource.clip = soundMove;
                    }
                    _audioSource.enabled = true;
                    
                }
                else
                {

                    if (_audioSource.clip != soundMoveToPlayer)
                    {
                        _audioSource.enabled = false;
                        _audioSource.clip = soundMoveToPlayer;
                    }

                    _audioSource.enabled = true;

                }
            }
        }

        if (EnemyState.instance.isDie)
            navMeshAgent.speed = 0;
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

        if (nearestPlayer != null && !EnemyState.instance.isDie)
        {
            timeDelayAttack -= Time.deltaTime;
            if (timeDelayAttack <= 0)
            {
                checkAttackSkill = true;
                timeDelayAttack = 5f;
            }

            if (!isComboAttacking && canRandomAttack && minDistance <= attackRange && Time.time - lastAttackTime > attackCooldown && !isAttacking && !checkAttackSkill)
            {

                int randomAttack = Random.Range(1, 5);
                Attack(randomAttack, nearestPlayer.transform);
                timeDelayAttack = 5f;

            }
            else if (/*minDistance > attackRange &&*/ Time.time - lastAttackSkillTime > attackSkillCooldown && !isAttacking && checkAttackSkill)
            {
                Debug.Log("Da danh tam xa");
                StartCoroutine(UseAttackSkill());
                checkAttackSkill = false;
                
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
            StartCoroutine(AttackAnimationCoroutine(attackType, playerTransform));
        }
        // navMeshAgent.SetDestination(playerTransform.position);
    }
    private IEnumerator UseAttackSkill()
    {
        animator.SetBool("AttackSkill", true);
        GameObject vfxSkill = Instantiate(vfxEnemySkill, transform.position, Quaternion.identity);
        Destroy(vfxSkill, 5);
        yield return new WaitForSeconds(2.5f);
        GameObject attackVFX = Instantiate(vfxAttackPrefab, vfxTarget.position, Quaternion.identity);
        attackVFX.transform.forward = transform.forward;
        //GameObject attackVFX = Instantiate(vfxAttackPrefab, transform.position, Quaternion.identity);
        Destroy(attackVFX, 5);
        lastAttackSkillTime = Time.time;
        animator.SetBool("AttackSkill", false);
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
        }
        else if (attackType == 3)
        {
            animator.SetBool("Attack3", true);
        }
        else if (attackType == 4)
        {
            animator.SetBool("AttackCombo", true);
        }



        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

        // Dat lai animation va tiep tuc di chuyen theo diem
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        animator.SetBool("AttackCombo",false);
        isAttacking = false;
        navMeshAgent.isStopped = false;
        lastAttackTime = Time.time;
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
