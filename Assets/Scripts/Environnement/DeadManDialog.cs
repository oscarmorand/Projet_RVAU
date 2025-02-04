using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class DeadManDialog : MonoBehaviour
{
    public GameObject deadManDialog;
    public GameObject canvas;

    public TextMeshProUGUI textComponent;
    [SerializeField] private string[] lines;
    public float textSpeed;

    private int index;
    private bool isTyping;
    private bool isDone;
    private bool hasStarted;

    InputAction nextLineAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deadManDialog.SetActive(false);
        canvas.SetActive(true);

        nextLineAction = InputSystem.actions.FindAction("Pick");

        isDone = false;
        hasStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTyping || !hasStarted || isDone)
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
        hasStarted = true;
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
            textComponent.text = string.Empty;
            deadManDialog.SetActive(false);
            canvas.SetActive(false);
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
            if (isDone || hasStarted)
            {
                return;
            }
            canvas.SetActive(true);
            deadManDialog.SetActive(true);
            StartDialogue();
        }
    }
}
