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

    public GameObject buttonObj;
    public GameObject baseObj;

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
        Hide();
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
            Hide();
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

        Show();

        StartCoroutine(PlayBuzzingSound());
    }

    IEnumerator PlayBuzzingSound()
    {
        yield return new WaitForSeconds(0.4f);
        buzzingSound.enabled = true;
        buzzingSound.Play();
    }

    private void StopBuzzing()
    {
        buzzingSound.Stop();
        buzzingSound.enabled = false;
    }

    public void Show()
    {
        buttonObj.GetComponent<MeshRenderer>().enabled = true;
        baseObj.GetComponent<MeshRenderer>().enabled = true;
    }

    public void Hide()
    {
        buttonObj.GetComponent<MeshRenderer>().enabled = false;
        baseObj.GetComponent<MeshRenderer>().enabled = false;
    }
}