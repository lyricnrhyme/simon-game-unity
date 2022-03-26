using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    List<int> userSeq = new List<int> { };

    public List<int> simonSeq = new List<int> { };

    int level = 0;

    int userCurrentStep = 0;

    int simonCurrentStep = 0;

    int highScore = 0;

    bool simonsTurn = false;

    // 5 seconds for the timer
    float timer = 5.0f;

    bool isGameBoardOn = false;

    public Text scoreDisplay;

    public Text highScoreDisplay;

    public GameObject[] pads;

    public GameObject onOffSwitch;

    Animator onOffSwitchAnimator;

    Coroutine timesUp;

    bool timerTriggered = false;

    bool timerStopped = false;

    bool isGameInSession = false;

    // Start is called before the first frame update
    void Start()
    {
        onOffSwitchAnimator = onOffSwitch.GetComponent<Animator>();
    }

    void Update()
    {
        if (timerTriggered)
        {
            if (timesUp != null) StopCoroutine(timesUp);
            timesUp = StartCoroutine(TimesUp());
            timerTriggered = false;
        }

        if (timerStopped)
        {
            if (timesUp != null) StopCoroutine(timesUp);
            timerStopped = false;
        }
    }

    public void StartGame()
    {
        if (isGameBoardOn)
        {
            ResetGame();
            SimonSequence();
        }
    }

    public void HandleOnOffSwitch()
    {
        isGameBoardOn = !isGameBoardOn;
        scoreDisplay.text = isGameBoardOn ? "00" : "";

        foreach (GameObject pad in pads)
        {
            Pad padScript = pad.GetComponent<Pad>();
            if (isGameBoardOn)
            {
                padScript.ChangePadColor(padScript.lightColor);
            }
            else
            {
                padScript.ChangePadColor(padScript.darkColor);
            }
        }

        if (isGameBoardOn)
        {
            onOffSwitchAnimator.Play("TurnOn");
            highScoreDisplay.text =
                highScore >= 10
                    ? highScore.ToString()
                    : "0" + highScore.ToString();
        }
        else
        {
            ResetGame();
            onOffSwitchAnimator.Play("TurnOff");
            highScoreDisplay.text = "";
            timerStopped = true;
            isGameInSession = false;
        }
    }

    void DisplayError()
    {
        if (isGameBoardOn)
        {
            scoreDisplay.text = "XX";
            isGameInSession = false;
            foreach (GameObject pad in pads)
            {
                Pad padScript = pad.GetComponent<Pad>();
                padScript.PlaySound();
            }

            if (level > highScore)
            {
                highScore = level;
                highScoreDisplay.text =
                    highScore >= 10
                        ? highScore.ToString()
                        : "0" + highScore.ToString();
            }
            ResetGame();

            StartCoroutine(ResetScoreDisplay());
        }
    }

    void SimonSequence()
    {
        if (isGameBoardOn)
        {
            isGameInSession = true;
            simonsTurn = true;
            scoreDisplay.text =
                level >= 10 ? level.ToString() : "0" + level.ToString();
            GetRandomNum();
            StartCoroutine(PlaySimonSequence());
            timerTriggered = true;
        }
        else
        {
            simonsTurn = false;
            isGameInSession = false;
        }
    }

    public void CheckSimonSequence(GameObject pad)
    {
        if (!simonsTurn && isGameBoardOn)
        {
            Pad padScript = pad.GetComponent<Pad>();
            userSeq.Add(padScript.id);
            padScript.AddClassSound();
            timerStopped = true;

            if (isGameInSession)
            {
                if (padScript.id != simonSeq[userCurrentStep])
                {
                    DisplayError();
                }
                else
                {
                    timerTriggered = true;
                    userCurrentStep++;

                    if (userSeq.Count == simonSeq.Count)
                    {
                        level++;
                        StartCoroutine(NextLevelSimonSequence());
                    }
                }
            }
        }
    }

    void ResetGame()
    {
        isGameInSession = false;
        level = 0;
        simonSeq.Clear();
        ResetUserInput();
        timerStopped = true;
    }

    void ResetUserInput()
    {
        userCurrentStep = 0;
        userSeq.Clear();
    }

    void GetRandomNum()
    {
        int rInt = UnityEngine.Random.Range(0, 4);
        simonSeq.Add (rInt);
    }

    IEnumerator TimesUp()
    {
        yield return new WaitForSeconds(timer);
        DisplayError();
    }

    IEnumerator NextLevelSimonSequence()
    {
        simonsTurn = true;
        yield return new WaitForSeconds(1.0f);
        ResetUserInput();
        SimonSequence();
        timerStopped = true;
    }

    IEnumerator PlaySimonSequence()
    {
        int colorValue = simonSeq[simonCurrentStep];
        pads[colorValue].GetComponent<Pad>().AddClassSound();
        simonCurrentStep++;
        yield return new WaitForSeconds(1.0f);
        if (simonCurrentStep >= simonSeq.Count)
        {
            simonsTurn = false;
            simonCurrentStep = 0;
            StopCoroutine(PlaySimonSequence());
            timerTriggered = true;
        }
        else
        {
            StartCoroutine(PlaySimonSequence());
        }
    }

    IEnumerator ResetScoreDisplay()
    {
        yield return new WaitForSeconds(1.5f);
        scoreDisplay.text = "00";
    }
}
