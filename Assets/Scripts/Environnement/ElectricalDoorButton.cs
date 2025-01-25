using UnityEngine;
using Photon.Pun; // Importer Photon
using UnityEngine.XR.Interaction.Toolkit;

public class ElectricalDoorButton : MonoBehaviourPun
{
    public int index;
    public GameObject door;
    public bool isPushed = false;

    public void OnButtonPressed(SelectEnterEventArgs args)
    {
        isPushed = true;
        if (door != null)
        {
            door.GetComponent<ElectricalDoor>().OnButtonPressed(index);
        }
    }

    public void OnButtonUnpressed(SelectExitEventArgs args)
    {
        isPushed = false;
        if (door != null)
        {
            door.GetComponent<ElectricalDoor>().OnButtonUnpressed(index);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPushed = true;
            if (door != null)
            {
                door.GetComponent<ElectricalDoor>().OnButtonPressed(index);
            }
        }
    }

    public void OnTiggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPushed = true;
            if (door != null)
            {
                door.GetComponent<ElectricalDoor>().OnButtonPressed(index);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPushed = false;
            if (door != null)
            {
                door.GetComponent<ElectricalDoor>().OnButtonUnpressed(index);
            }
        }
    }
}
