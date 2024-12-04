using UnityEngine;

public class EnableVolume : MonoBehaviour
{
    public GameObject globalVolume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        globalVolume.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
