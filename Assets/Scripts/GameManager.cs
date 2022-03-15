using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    List<int> userSeq = new List<int> { };
    List<int> simonSeq = new List<int> { };
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
    void StartGame () {

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

    }

    void SimonSequence () {

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
        int rInt = Random.Range (0, 4);
        simonSeq.Add (rInt);
    }

    IEnumerator TimesUp () {
        yield return new WaitForSeconds (timer);
        DisplayError ();
    }

    IEnumerator PlaySimonSequence () {
        int colorValue = simonSeq[simonCurrentStep];
        pads[colorValue].GetComponent<Pad> ().AddClassSound ();
        simonCurrentStep++;
        yield return new WaitForSeconds (1.0f);
        if (simonCurrentStep >= simonSeq.Count) {
            simonsTurn = false;
            simonCurrentStep = 0;
            StopCoroutine (PlaySimonSequence ());
            StartCoroutine (TimesUp ());
        } else {
            StartCoroutine (PlaySimonSequence ());
        }
    }

}