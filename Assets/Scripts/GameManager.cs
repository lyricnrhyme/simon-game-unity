using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    List<int> userSeq = new List<int> { };
    public List<int> simonSeq = new List<int> { };
    int level = 0;
    int userCurrentStep = 0;
    int simonCurrentStep = 0;
    int highScore = 0;
    bool simonsTurn = false;

    // TODO set up timesUp

    // 5 seconds for the timer
    float timer = 5.0f;
    bool isGameBoardOn = false;

    public Text scoreDisplay;
    public Text highScoreDisplay;
    public GameObject[] pads;
    public GameObject onOffSwitch;
    Animator onOffSwitchAnimator;

    // Start is called before the first frame update
    void Start () {
        onOffSwitchAnimator = onOffSwitch.GetComponent<Animator> ();
    }

    // TODO js startButton function
    public void StartGame () {
        if (isGameBoardOn) {
            ResetGame ();
            SimonSequence ();
        }
    }

    // TODO js outerSwtich function
    public void HandleOnOffSwitch () {
        isGameBoardOn = !isGameBoardOn;
        scoreDisplay.text = isGameBoardOn ? "00" : "";

        foreach (GameObject pad in pads) {
            Pad padScript = pad.GetComponent<Pad> ();
            if (isGameBoardOn) {
                padScript.ChangePadColor (padScript.lightColor);
            } else {
                padScript.ChangePadColor (padScript.darkColor);
            }
        }

        if (isGameBoardOn) {
            onOffSwitchAnimator.Play ("TurnOn");
            highScoreDisplay.text = highScore >= 10 ? highScore.ToString () : "0" + highScore.ToString ();
        } else {
            ResetGame ();
            onOffSwitchAnimator.Play ("TurnOff");
            highScoreDisplay.text = "";
        }
    }

    void DisplayError () {
        scoreDisplay.text = "XX";
        foreach (GameObject pad in pads) {
            Pad padScript = pad.GetComponent<Pad> ();
            padScript.PlaySound ();
        }

        if (level > highScore) {
            highScore = level;
            highScoreDisplay.text = highScore >= 10 ? highScore.ToString () : "0" + highScore.ToString ();
        }
        ResetGame ();

        StartCoroutine (ResetScoreDisplay ());
    }

    void SimonSequence () {
        if (isGameBoardOn) {
            simonsTurn = true;
            scoreDisplay.text = level >= 10 ? level.ToString () : "0" + level.ToString ();
            GetRandomNum ();
            foreach (int colorId in simonSeq) {
                StartCoroutine (PlaySimonSequence (colorId));
            }
            simonsTurn = false;
            StartCoroutine (TimesUp ());
        }
    }

    public void CheckSimonSequence (GameObject pad) {
        if (!simonsTurn && isGameBoardOn) {
            Pad padScript = pad.GetComponent<Pad> ();
            userSeq.Add (padScript.id);
            padScript.AddClassSound ();
            StopCoroutine (TimesUp ());

            if (padScript.id != simonSeq[userCurrentStep]) {
                DisplayError ();
            } else {
                StartCoroutine (TimesUp ());
                userCurrentStep++;

                if (userSeq.Count == simonSeq.Count) {
                    level++;
                    ResetUserInput ();
                    SimonSequence ();
                    StopCoroutine (TimesUp ());
                }
            }
        }
    }

    void ResetGame () {
        level = 0;
        simonSeq.Clear ();
        ResetUserInput ();
        StopCoroutine (TimesUp ());
    }

    void ResetUserInput () {
        userCurrentStep = 0;
        userSeq.Clear ();
    }

    void GetRandomNum () {
        int rInt = UnityEngine.Random.Range (0, 4);
        simonSeq.Add (rInt);
    }

    IEnumerator TimesUp () {
        yield return new WaitForSeconds (timer);
        DisplayError ();
    }

    // IEnumerator PlaySimonSequence () {
    //     int colorValue = simonSeq[simonCurrentStep];
    //     pads[colorValue].GetComponent<Pad> ().AddClassSound ();
    //     simonCurrentStep++;
    //     yield return new WaitForSeconds (1.0f);
    //     if (simonCurrentStep >= simonSeq.Count) {
    //         simonsTurn = false;
    //         simonCurrentStep = 0;
    //         StopCoroutine (PlaySimonSequence ());
    //         StartCoroutine (TimesUp ());
    //     } else {
    //         StartCoroutine (PlaySimonSequence ());
    //     }
    // }

    IEnumerator ResetScoreDisplay () {
        yield return new WaitForSeconds (1.5f);
        scoreDisplay.text = "00";
    }

    IEnumerator PlaySimonSequence (int id) {
        GameObject colorPad = Array.Find (pads, pad => pad.GetComponent<Pad> ().id == id);
        colorPad.GetComponent<Pad> ().AddClassSound ();
        yield return new WaitForSeconds (1.0f);
    }

}