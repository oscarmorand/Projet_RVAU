using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string time_str = System.DateTime.UtcNow.ToString("HH:mm:ss");
        timerText.text = time_str;
    }
}
