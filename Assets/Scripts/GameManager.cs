using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<int> userSeq = {};
    List<int> simonSeq = {};
    int level = 0;
    int userCurrentStep = 0;
    int highScore = 0;
    bool simonsTurn = false;

    // TODO set up timesUp

    // 5 seconds for the timer
    int timer = 5000;
    bool isGameBoardOn = false;

    public Text scoreDisplay;

    public Text highScoreDisplay;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // TODO js startButton function
    void StartGame() {
        
    }

    // TODO js outerSwtich function
    void HandleOnOffSwitch() {

    }

    void DisplayError() {

    }

    void SimonSequence() {

    }

    void ResetGame() {

    }

    void ResetUserInput() {
        userCurrentStep = 0;
        userSeq.Clear();
    }

    void GetRandomNum() {
        Random r = new Random();
        int rInt = r.Next(0,4);
        simonSeq.Add(rInt);
    }

    void ChangePadColor() {

    }
}
