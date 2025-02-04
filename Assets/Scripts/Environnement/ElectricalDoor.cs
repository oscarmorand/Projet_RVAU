using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ElectricalDoor : MonoBehaviourPun
{
    public GameObject closedDoor;
    public GameObject openDoor;

    public bool isOpen = false;

    public GameObject[] buttons;
    public bool[] pressedButtons;

    public BeginChaseEvent beginChaseEvent = null;

    public AudioSource openSound;

    void Start()
    {
        closedDoor.SetActive(true);
        openDoor.SetActive(false);

        pressedButtons = new bool[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<ElectricalDoorButton>().index = i;
            buttons[i].GetComponent<ElectricalDoorButton>().door = gameObject;
        }
    }

    public void OnButtonPressed(int index)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_OnButtonPressed", RpcTarget.All, index);
        }
        else
        {
            if (!isOpen)
            {
                RPC_OpenDoor();
            }
        }
    }

    [PunRPC]
    public void RPC_OnButtonPressed(int index)
    {
        pressedButtons[index] = true;
        CheckDoor();
    }

    public void OnButtonUnpressed(int index)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_OnButtonUnpressed", RpcTarget.All, index);
        }
    }

    [PunRPC]
    public void RPC_OnButtonUnpressed(int index)
    {
        pressedButtons[index] = false;
    }

    void CheckDoor()
    {
        if (isOpen)
        {
            return;
        }
        int count = 0;
        foreach (bool pressed in pressedButtons)
        {
            if (pressed)
            {
                count++;
            }
        }
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (count == buttons.Length || count >= playerCount)
        {
            photonView.RPC("RPC_OpenDoor", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_OpenDoor()
    {
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
        isOpen = true;

        StartCoroutine(StartChase());

        openSound.Play();
    }

    IEnumerator StartChase()
    {
        yield return new WaitForSeconds(3f);

        if (beginChaseEvent != null)
        {
            beginChaseEvent.OnEventStarted();
        }
    }
}
