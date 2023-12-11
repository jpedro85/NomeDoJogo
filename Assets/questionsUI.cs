using TMPro;
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

    public TMP_InputField inputField;

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
        chances = 3;
        
        guessClicked = false;
        getRandomQuestion();
        
        dialog.SetActive(true);

        TxtErro.text = " ";
        input = " ";
        inputField.Select();
        inputField.text="";
        
        closeDelay = 0;
    }

    void getRandomQuestion()
    {
        var randomQuestionIndex = Random.Range(0, questions.Length);
        currentQuestion = questions[randomQuestionIndex];
        currentAnswer = answers[randomQuestionIndex];

        questionText.text = currentQuestion.questions;
    }

    public void skipQuestion()
    {
        answer?.Invoke(false);
        dialog.SetActive(false);
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
        if (!guessClicked || input == "") return;

        if(chances > 0){

            if (input.ToLower() == currentAnswer.answers.ToLower())
            {
                TxtErro.color = Color.green;
                TxtErro.text = "Correct";
                guessClicked = false;
                closeDelay = 0.01f;
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
            guessClicked = false;
            closeDelay = 0.01f;
            answer?.Invoke(false);
        }
    }

    public void backFuntion()
    {
        backEvent?.Invoke();
    }
    
}
