using UnityEngine;
using Photon.Pun;

public class ElectricalDoor : MonoBehaviourPun
{
    public GameObject closedDoor;
    public GameObject openDoor;

    public bool isOpen = false;

    public GameObject[] buttons;
    public bool[] pressedButtons;

    public BeginChaseEvent beginChaseEvent = null;

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
            RPC_OpenDoor();
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
        foreach (bool pressed in pressedButtons)
        {
            if (!pressed)
            {
                return;
            }
        }
        OpenDoor();
    }

    void OpenDoor()
    {
        photonView.RPC("RPC_OpenDoor", RpcTarget.All);
    }

    [PunRPC]
    void RPC_OpenDoor()
    {
        closedDoor.SetActive(false);
        openDoor.SetActive(true);
        isOpen = true;

        if (beginChaseEvent != null)
        {
            beginChaseEvent.OnEventStarted();
        }
    }
}
