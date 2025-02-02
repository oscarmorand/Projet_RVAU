using UnityEngine;
using System.Collections;

public class StepsSounds : MonoBehaviour
{
    public float stepsVolume = 0.5f;

    public AudioSource[] audioSources;
    public float stepDelayFactor = 1f;

    private Vector3 lastPos;
    private float speed;
    private bool isPlaying = false;
    private int lastIndex = -1;

    public bool constant = false;
    public float constantDelay = 0.5f;

    void Start()
    {
        lastPos = transform.position;
    }

    IEnumerator playStep()
    {
        isPlaying = true;

        int index = Random.Range(0, audioSources.Length);
        if (index == lastIndex)
        {
            index = (index + 1) % audioSources.Length;
        }
        lastIndex = index;
        var audioSource = audioSources[index];

        if (!audioSource.isPlaying)
        {
            audioSource.volume = stepsVolume;
            audioSource.Play();
        }

        if (constant)
        {
            yield return new WaitForSeconds(constantDelay);
        }
        else
        {
            float delay = stepDelayFactor / Mathf.Max(speed, 0.1f);
            yield return new WaitForSeconds(delay);
        }

        isPlaying = false;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        speed = (currentPos - lastPos).magnitude / Time.deltaTime;

        if (speed > 1f && !isPlaying)
        {
            StartCoroutine(playStep());
        }

        lastPos = currentPos;
    }
}
