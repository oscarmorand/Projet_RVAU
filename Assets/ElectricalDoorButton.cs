using UnityEngine;
using Photon.Pun; // Importer Photon
using UnityEngine.XR.Interaction.Toolkit;

public class ElectricalDoorButton : MonoBehaviourPun
{
    public int index;
    public GameObject door;

    public void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (door != null)
        {
            door.GetComponent<ElectricalDoor>().OnButtonPressed(index);
        }
    }

    public void OnButtonUnpressed(SelectExitEventArgs args)
    {
        if (door != null)
        {
            door.GetComponent<ElectricalDoor>().OnButtonUnpressed(index);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (door != null)
            {
                door.GetComponent<ElectricalDoor>().OnButtonPressed(index);
            }
        }
    }
}
