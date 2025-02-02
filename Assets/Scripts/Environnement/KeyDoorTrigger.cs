using UnityEngine;
using System.Collections.Generic;  // Import nécessaire pour utiliser List<GameObject>

public class KeyDoorTrigger : MonoBehaviour
{
    public KeyDoor keyDoor;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " has entered the trigger zone of the door. Checking if it is a key...");

        if (other.CompareTag("Key"))
        {
            Debug.Log("A key entered the lock directly.");
            keyDoor.OpenDoor();
            return;
        }

        List<GameObject> keys = GetChildrenWithTag(other.gameObject, "Key");

        foreach (GameObject key in keys)
        {
            if (key.activeSelf)
            {
                Debug.Log("A key entered the lock via a child object.");
                keyDoor.OpenDoor();
                break;
            }
        }
    }

    List<GameObject> GetChildrenWithTag(GameObject parent, string tag)
    {
        List<GameObject> taggedChildren = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                taggedChildren.Add(child.gameObject);
            }

            taggedChildren.AddRange(GetChildrenWithTag(child.gameObject, tag));
        }

        return taggedChildren;
    }
}

