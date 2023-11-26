using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class questionsUI : MonoBehaviour
{
    public Question[] questions;
    public Answer[] answers;

    private Question currentQuestion;
    private Answer currentAnswer;

    private bool guessClicked;
    private string input;


    [SerializeField]
    private TextMeshProUGUI questionText;
    private Button guessButton;

    private void Start()
    {
        guessClicked = false;
        getRandomQuestion();
    }

    void getRandomQuestion()
    {
        int randomQuestionIndex = Random.Range(0, questions.Length);
        currentQuestion = questions[randomQuestionIndex];
        currentAnswer = answers[randomQuestionIndex];

        questionText.text = currentQuestion.questions;
    }

    public void userSelectedGuess()
    {
        guessClicked = true;
        checkAnswer();
    }

    public void getAnswer(string ans)
    {
        input = ans;
        Debug.Log(input);
    }

    void checkAnswer()
    {
        if (guessClicked)
        {
            if(input.ToLower()==currentAnswer.answers.ToLower())
            {
                //int++
            }
            else
            {
                //close UI
            }
        }
    }
}
