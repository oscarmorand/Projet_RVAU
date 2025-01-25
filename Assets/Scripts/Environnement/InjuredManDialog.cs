using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class InjuredManDialog : MonoBehaviour
{
    public GameObject injuredManDialog;

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    private bool isTyping;
    private bool isDone;

    InputAction nextLineAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        injuredManDialog.SetActive(false);

        nextLineAction = InputSystem.actions.FindAction("Pick");
    }

    // Update is called once per frame
    void Update()
    {
        if (isTyping)
        {
            return;
        }
        if (nextLineAction.IsPressed())
        {
            NextLine();
        }
    }

    void StartDialogue()
    {
        index = 0;
        isDone = false;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            injuredManDialog.SetActive(false);
            isDone = true;
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isDone)
            {
                return;
            }
            injuredManDialog.SetActive(true);
            StartDialogue();
        }
    }
}
