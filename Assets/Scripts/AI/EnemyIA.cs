using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using System.Collections;

public class EnemyIA : MonoBehaviourPun
{
    public enum EnemyState
    {
        Base,
        Chase,
        Attack,
        Sleep
    }

    public float speed = 3.0f;
    public float attackDistance = 2.0f;

    public float recheckFrequency = 2.0f;
    private float recheckTimer = 0.0f;

    public float sleepDuration = 5.0f;
    private float sleepTimer = 0.0f;
    private bool isSleeping = false;

    public EnemyState state;
    private GameObject targetPlayer;
    private NavMeshAgent navMeshAgent;

    public MeshRenderer mesh;
    public MeshRenderer hairMesh;

    public AudioSource firstScreamAudio;
    public AudioSource jumpscareAudio;

    private bool isLooking = false;
    public Transform monsterEyes;

    void Start()
    {
        state = EnemyState.Base;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;

        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                enabled = false;
            }

            photonView.RPC("RPC_HideMesh", RpcTarget.All);
            photonView.RPC("RPC_PlayFirstScream", RpcTarget.All);
        }
        else
        {
            RPC_HideMesh();
            RPC_PlayFirstScream();
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

        switch (state)
        {
            case EnemyState.Base:
                DetectOrRecheckPlayers();
                break;

            case EnemyState.Chase:
                recheckTimer += Time.deltaTime;
                if (recheckTimer >= recheckFrequency)
                {
                    DetectOrRecheckPlayers();
                    recheckTimer = 0.0f;
                }
                ChasePlayer();
                break;

            case EnemyState.Attack:
                AttackPlayer();
                break;

            case EnemyState.Sleep:
                StartCoroutine(Sleep());
                break;
        }
    }

    void DetectOrRecheckPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log("Nombre de joueurs : " + players.Length);

        float closestPathDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (GameObject player in players)
        {
            if (player.GetComponent<PhotonView>() == null) continue;

            float pathDistance = CalculatePathDistance(player.transform.position);

            if (pathDistance < closestPathDistance)
            {
                closestPathDistance = pathDistance;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            if (targetPlayer != closestPlayer)
            {
                targetPlayer = closestPlayer;
                Debug.Log($"Cible trouvée : {targetPlayer.name}");
            }

            if (state == EnemyState.Base)
            {
                state = EnemyState.Chase;
            }
        }
    }

    void ChasePlayer()
    {
        Debug.Log("Chasing");
        if (targetPlayer == null)
        {
            state = EnemyState.Base;
            return;
        }

        navMeshAgent.SetDestination(targetPlayer.transform.position);

        float distance = Vector3.Distance(transform.position, targetPlayer.transform.position);
        if (distance < attackDistance)
        {
            Debug.Log("Attaque");
            state = EnemyState.Attack;
        }
    }

    void AttackPlayer()
    {
        if (targetPlayer == null)
        {
            state = EnemyState.Base;
            return;
        }

        navMeshAgent.ResetPath();

        if (PhotonNetwork.IsConnected)
        {
            PhotonView playerPhotonView = targetPlayer.GetComponent<PhotonView>();
            int playerId = playerPhotonView.Owner.ActorNumber;

            photonView.RPC("RPC_AttackPlayer", PhotonNetwork.CurrentRoom.GetPlayer(playerId));
        }
        else
        {
            RPC_AttackPlayer();
        }
        
        Debug.Log($"Attaque le joueur {targetPlayer.name}");

        state = EnemyState.Sleep;
    }

    IEnumerator Sleep()
    {
        if (isSleeping)
            yield break;

        isSleeping = true;
        yield return new WaitForSeconds(sleepDuration);
        isSleeping = false;

        state = EnemyState.Base;
    }

    float CalculatePathDistance(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(targetPosition, path);

        if (path.status != NavMeshPathStatus.PathComplete)
        {
            return Mathf.Infinity;
        }

        float distance = 0.0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return distance;
    }

    [PunRPC]
    public void RPC_HideMesh()
    {
        mesh.enabled = false;
        hairMesh.enabled = false;
    }

    [PunRPC]
    public void RPC_AttackPlayer()
    {
        ShowMesh();
        Jumpscare();
    }

    public void ShowMesh()
    {
        mesh.enabled = true;
        hairMesh.enabled = true;
    }

    public void Jumpscare()
    {
        jumpscareAudio.Play();

        Camera cam = targetPlayer.GetComponent<Camera>();
        if (cam == null)
        {
            cam = targetPlayer.GetComponentInChildren<Camera>();
        }

        Transform camTransform = cam.transform;

        if (!isLooking)
        {
            StartCoroutine(LookAtForOneSecond(monsterEyes, camTransform));
        }
    }

    private IEnumerator LookAtForOneSecond(Transform target, Transform cameraTransform)
    {
        isLooking = true;

        Quaternion initialRotation = cameraTransform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target.position - cameraTransform.position);

        float transition = 0.2f;
        float duration = 1.0f;

        float elapsed = 0f;
        while (elapsed < transition)
        {
            cameraTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsed / transition);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            cameraTransform.rotation = Quaternion.LookRotation(target.position - cameraTransform.position);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetRotation = Quaternion.LookRotation(target.position - cameraTransform.position);
        elapsed = 0f;
        while (elapsed < transition)
        {
            cameraTransform.rotation = Quaternion.Slerp(targetRotation, initialRotation, elapsed / transition);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.rotation = initialRotation; // Assure une rotation finale précise
        isLooking = false;

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_HideMesh", RpcTarget.All);
        }
        else
        {
            RPC_HideMesh();
        }
    }

    [PunRPC]
    public void RPC_PlayFirstScream()
    {
        firstScreamAudio.Play();
    }
}
