using UnityEngine;
using Photon.Pun;

public class DestructableWall : MonoBehaviourPun
{
    public GameObject destructableWall;
    public GameObject fallingWall;

    public bool destroyed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destructableWall.SetActive(true);
        fallingWall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (destroyed) return;

        if (other.gameObject.CompareTag("Explosive") || other.gameObject.name == "Dynamite")
        {
            Debug.Log("An explosive ented the wall");

            WickExplosive wick = other.gameObject.GetComponent<WickExplosive>();
            if (wick.isOn)
            {
                Debug.Log("Explode wall");

                WickExplosive explosive = other.gameObject.GetComponent<WickExplosive>();
                explosive.Explode();

                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPC_DestroyWall", RpcTarget.All);
                }
                else
                {
                    RPC_DestroyWall();
                }
            }
        }
    }

    [PunRPC]
    public void RPC_DestroyWall()
    {
        destroyed = true;
        GameObject.Destroy(destructableWall);
        fallingWall.SetActive(true);
    }
}
