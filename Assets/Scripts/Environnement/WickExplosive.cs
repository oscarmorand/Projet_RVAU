using UnityEngine;
using System.Collections;
using Photon.Pun;

public class WickExplosive : MonoBehaviourPun
{
    public GameObject flame;
    public GameObject explosionPrefab;

    public bool isOn = false;
    public bool hasExploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator WaitForExplosion()
    {
        isOn = true;

        flame.SetActive(true);
        flame.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(5);

        Explode();
    }

    [PunRPC]
    public void RPC_Explode()
    {
        hasExploded = true;

        GameObject explosiveObj = gameObject.transform.parent.gameObject;

        GameObject explosion = Instantiate(explosionPrefab, explosiveObj.transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();

        GameObject.Destroy(explosiveObj);
    }

    [PunRPC]
    public void RPC_StartExplosion()
    {
        StartCoroutine(WaitForExplosion());
    }

    public void StartExplosion()
    {
        if (isOn) return;

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_StartExplosion", RpcTarget.All);
        }
        else
        {
            RPC_StartExplosion();
        }
    }

    public void Explode()
    {
        if (hasExploded) return;

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_Explode", RpcTarget.All);
        }
        else
        {
            RPC_Explode();
        }
    }
}
