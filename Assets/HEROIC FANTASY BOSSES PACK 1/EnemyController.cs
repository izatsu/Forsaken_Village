
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviourPunCallbacks
{
    [Header("Move Points")]
    [SerializeField] Transform[] movePoints;

    [Header("Vision Range")]
    [SerializeField] float visionRange = 5;
    [SerializeField] float returnTimeOutSize = 5;

    [Header("NavMeshAgent")]
    NavMeshAgent navMeshAgent;

    int currentMovePoint;
    bool playerInSight;
    float timePlayerLeftSight;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentMovePoint = Random.Range(0, movePoints.Length);
        SetNextDestination();
    }

     public void Update()
     {
       /* if(!photonView.IsMine)
        {
            return;
        }*/
        // Ki?m tra xem enemy ?ã ??n g?n ?i?m ??n hay ch?a
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            SetNextDestination();
        }
        if (PlayerInVisionRange())
        {
            FollowPlayer();
        }
        else if(playerInSight)
        {
            PlayerLeftSight();
        }
    }

    public void SetNextDestination()
    {
        currentMovePoint = GetRandomMovePoints();
        Vector3 targetPosition = movePoints[currentMovePoint].position;
        navMeshAgent.SetDestination(targetPosition);
    }

    private int GetRandomMovePoints()
    {
        int newPoint = currentMovePoint;
        while (newPoint == currentMovePoint)
        {
            newPoint = Random.Range(0, movePoints.Length);
        }
        return newPoint;
    }

    private bool PlayerInVisionRange()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Tìm t?t c? các ng??i ch?i trong scene

        foreach (GameObject player in players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= visionRange)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, player.transform.position, out hit))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        playerInSight = true;
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private void FollowPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Tìm t?t c? các ng??i ch?i trong scene
        GameObject nearestPlayer = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = player;
            }
        }

        if (nearestPlayer != null)
        {
            navMeshAgent.SetDestination(nearestPlayer.transform.position);
        }
    }
    private void PlayerLeftSight()
    {
        // N?u player r?i kh?i t?m nhìn, ??m th?i gian
        timePlayerLeftSight += Time.deltaTime;
        if (timePlayerLeftSight >= returnTimeOutSize)
        {
            playerInSight = false;
            timePlayerLeftSight = 0f;
            SetNextDestination();
        }
    }

}
