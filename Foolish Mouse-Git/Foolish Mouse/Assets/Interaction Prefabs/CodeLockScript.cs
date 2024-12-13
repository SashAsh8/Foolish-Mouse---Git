using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Text;

public class CodeLockWithTrigger : MonoBehaviour
{
    // UnityEvents for correct and wrong answers
    public UnityEvent<string> onRightAnswer;
    public UnityEvent<string> onWrongAnswer;

    // Configuration for the code lock
    public int CodeLength = 4;
    public string CorrectCode = "1234";

    // Display and code variables
    private TextMeshProUGUI displayText;
    [HideInInspector]
    public string code = "";

    public UnityEvent onRightAnswerAnimationEvent;
    public GameObject objectToAnimate;

    void Start()
    {
        // Find the display TextMeshPro component
        displayText = transform.Find("Display").GetComponentInChildren<TextMeshProUGUI>();

        // Add button listeners
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(
                button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
            ));
        }
    }

    void OnButtonClick(string name)
    {
        if (name == "<")
        {
            // Remove the last character from the code
            if (code.Length > 0)
            {
                code = code.Substring(0, code.Length - 1);
                UpdateDisplay();
            }
        }
        else if (name == "Ok")
        {
            // Check the code and invoke the correct callback
            CloseCodeLock();
            HandleCallback((code == CorrectCode), code);
        }
        else if (name == "Close")
        {
            CloseCodeLock();
        }
        else if (code.Length < CodeLength)
        {
            // Add the pressed button's value to the code
            code += name;
            UpdateDisplay();
        }
    }

    void HandleCallback(bool answerWasRight, string enteredCode)
    {
        if (answerWasRight)
        {
            // Invoke the correct answer UnityEvent
            onRightAnswer.Invoke(enteredCode);

            onRightAnswerAnimationEvent?.Invoke();
            TriggerAnimation(true);
        }
        else
        {
            // Invoke the wrong answer UnityEvent
            onWrongAnswer.Invoke(enteredCode);
        }
    }

    void CloseCodeLock()
    {
        // Deactivate the parent canvas
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            parentCanvas.gameObject.SetActive(false);
        }
    }

    void UpdateDisplay()
    {
        // Format the code with underscores for empty slots
        string paddedCode = code.PadRight(CodeLength, '_');
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < paddedCode.Length; i++)
        {
            stringBuilder.Append(paddedCode[i]);
            if (i != paddedCode.Length - 1)
                stringBuilder.Append(' ');
        }

        displayText.text = stringBuilder.ToString();
    }

    void TriggerAnimation(bool isCorrect)
    {
        if (objectToAnimate != null)
        {
            Animator animator = objectToAnimate.GetComponent<Animator>();

            if (animator != null)
            {
                string triggerName = isCorrect ? "CorrectAnswer" : "WrongAnswer";
                animator.SetTrigger(triggerName);
            }
        }
    }
}
