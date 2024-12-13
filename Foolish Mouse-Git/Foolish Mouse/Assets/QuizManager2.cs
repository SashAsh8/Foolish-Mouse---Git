using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public class Question
    {
        public string text;
        public string[] answers;

        public Question(string text, string[] answers)
        {
            this.text = text;
            this.answers = answers;
        }
    }

    public TMPro.TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TMPro.TextMeshProUGUI resultText;

    private int currentIndex = 0;
    private List<Question> questions = new List<Question>(); // Use a List to store questions
    private Dictionary<string, int> personalities;

    void Start()
    {
        // Initialize personality scores
        personalities = new Dictionary<string, int> { { "A", 0 }, { "B", 0 }, { "C", 0 }, { "D", 0 } };

        // Add questions manually
        questions.Add(new Question("What is your favorite color?", new string[] { "A. Blue", "B. Red", "C. Green", "D. Yellow" }));
        questions.Add(new Question("What is your favorite food?", new string[] { "A. Pizza", "B. Sushi", "C. Burger", "D. Salad" }));
        questions.Add(new Question("What is your favorite hobby?", new string[] { "A. Reading", "B. Sports", "C. Traveling", "D. Gaming" }));

        ShowQuestion();

        resultText.gameObject.SetActive(false);

        // Add listeners to buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => Answer(index));
        }
    }

    void ShowQuestion()
    {
        if (currentIndex < questions.Count)
        {
            questionText.text = questions[currentIndex].text;

            string[] shuffledAnswers = ShuffleArray(questions[currentIndex].answers);

            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<Text>().text = shuffledAnswers[i];
                answerButtons[i].name = shuffledAnswers[i].Substring(0, 1); // Set button name (e.g., "A", "B")
            }
        }
        else
        {
            ShowResult();
        }
    }

    string[] ShuffleArray(string[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            string temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
    }

    void Answer(int buttonIndex)
    {
        personalities[answerButtons[buttonIndex].name]++;

        currentIndex++;
        ShowQuestion();
    }

    void ShowResult()
    {
        questionText.gameObject.SetActive(false);
        foreach (Button button in answerButtons)
        {
            button.gameObject.SetActive(false);
        }

        string dominantPersonality = "";
        int maxCount = 0;

        foreach (var pair in personalities)
        {
            if (pair.Value > maxCount)
            {
                dominantPersonality = pair.Key;
                maxCount = pair.Value;
            }
        }

        resultText.gameObject.SetActive(true);
        resultText.text = $"Your personality is: {dominantPersonality}";
    }
}
