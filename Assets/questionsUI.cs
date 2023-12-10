using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class questionsUI : MonoBehaviour
{
    public Question[] questions;
    public Answer[] answers;

    private Question currentQuestion;
    private Answer currentAnswer;
    public GameObject dialog;

    private bool guessClicked;
    private string input;
    private int chances;
    public TextMeshProUGUI TxtErro;


    public TextMeshProUGUI questionText;
    private float closeDelay;


    public bool isOpen
    {
        get { return dialog.activeSelf; }
    }


    public delegate void answerDelegate(bool result);
    public event answerDelegate answer;


    public delegate void back();
    public event back backEvent;

    private void Start()
    {
        dialog.SetActive(false);
        guessClicked = false;
        chances = 3;
        getRandomQuestion();
        TxtErro.text = " ";
        input = "";
        closeDelay = 0;
    }

    public void Update()
    {
        if (closeDelay != 0 && !guessClicked && closeDelay  <= 1)
        {
            closeDelay += Time.deltaTime;
        }
        else if(!guessClicked && closeDelay >= 1)
        {
            dialog.SetActive(false);
        }
    }

    public void open()
    {
        guessClicked = false;
        chances = 3;
        getRandomQuestion();
        dialog.SetActive(true);
        TxtErro.text = " ";
        input = " ";
        closeDelay = 0;
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
    }

    void checkAnswer()
    {


        if (guessClicked && input != "" )
        {
            if(chances > 0){

                if (input.ToLower() == currentAnswer.answers.ToLower())
                {
                    TxtErro.color = Color.green;
                    TxtErro.text = "Correct";
                    guessClicked = false;
                    closeDelay = 0.01f;
                    Debug.Log(answer.Target);
                    answer?.Invoke(true);
                }
                else
                {
                    chances--;
                    TxtErro.color = Color.red;
                    TxtErro.text = "Remaining attempts: " + chances.ToString();

                }
            }
            else
            {
                Debug.Log(answer.Target);
                guessClicked = false;
                closeDelay = 0.01f;
                answer?.Invoke(false);
            }
        }
        else
        {
            

        }
    
            
    }

    public void backFuntion()
    {
        backEvent?.Invoke();
    }
    
}
