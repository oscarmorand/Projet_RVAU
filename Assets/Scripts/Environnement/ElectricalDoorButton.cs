using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ElectricalDoorButton : MonoBehaviourPun
{
    public int index;
    public GameObject door;
    public bool isPushed = false;

    public AudioSource pushSound;
    public AudioSource buzzingSound;

    public void OnButtonPressed(SelectEnterEventArgs args)
    {
        StartPush();
    }

    public void OnButtonUnpressed(SelectExitEventArgs args)
    {
        isPushed = false;
        if (door != null)
        {
            door.GetComponent<ElectricalDoor>().OnButtonUnpressed(index);
        }
        StopBuzzing();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartPush();
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
            StopBuzzing();
        }
    }

    private void StartPush()
    {
        isPushed = true;
        pushSound.Play();
        if (door != null)
        {
            door.GetComponent<ElectricalDoor>().OnButtonPressed(index);
        }

        StartCoroutine(PlayBuzzingSound());
    }

    IEnumerator PlayBuzzingSound()
    {
        yield return new WaitForSeconds(0.4f);
        buzzingSound.Play();
    }

    private void StopBuzzing()
    {
        buzzingSound.Stop();
    }
}