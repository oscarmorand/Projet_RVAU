using UnityEngine;

public class CollapseBridge : MonoBehaviour
{
    public GameObject bridge;
    public GameObject brokenBridge;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bridge.SetActive(true);
        brokenBridge.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BreakBridge()
    {
        bridge.SetActive(false);
        brokenBridge.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BreakBridge();
        }
    }
}
