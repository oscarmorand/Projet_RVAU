using UnityEngine;
using Photon.Pun;


public class KeyDoor : MonoBehaviourPun
{
    public GameObject closedDoor;
    public GameObject openDoor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPC_OpenDoor", RpcTarget.All);
            }
            else
            {
                RPC_OpenDoor();
            }
        }
    }

    [PunRPC]
    void RPC_OpenDoor()
    {
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
    }
}
