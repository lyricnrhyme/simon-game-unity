using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pad : MonoBehaviour {
    public int id;

    public AudioSource padSound;
    public Color lightColor;
    public Color darkColor;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void AddClassSound () {
        ChangePadColor (darkColor);
        PlaySound ();
        StartCoroutine (RevertPadColor ());
    }

    public void PlaySound () {
        padSound.Play ();
    }

    public void ChangePadColor (Color color) {
        gameObject.GetComponent<Image> ().color = color;
    }

    IEnumerator RevertPadColor () {
        yield return new WaitForSeconds (0.5f);
        if (GameManager.isGameBoardOn) ChangePadColor (lightColor);
    }
}