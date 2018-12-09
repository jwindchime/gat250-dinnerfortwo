using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundController : MonoBehaviour {

    [SerializeField] GameObject food;
    [SerializeField] GameObject sauce;
    [SerializeField] GameObject table;
    [SerializeField] GameObject blueEat;
    [SerializeField] GameObject orangeEat;
    [SerializeField] Text blueText;
    [SerializeField] Text orangeText;
    [SerializeField] GameObject blueWin;
    [SerializeField] GameObject orangeWin;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject countdownUI;

    public static int blueScore = 0;
    public static int orangeScore = 0;
    public static int roundNum = 1;

    bool nextRound;

    public void Start()
    {
        // Reset time scale at the start of the scene
        Time.timeScale = 1.0f;
    }

    public static bool GameOver()
    {
        if (blueScore > roundNum / 2.0f || orangeScore > roundNum / 2.0f)
        {
            return true;
        }

        return false;
    }

    public void IncrementScore(bool isBlue)
    {
        if (isBlue)
            blueText.text = "" + ++blueScore;
        else
            orangeText.text = "" + ++orangeScore;

        Invoke("EndRound", 2.0f);
    }

    public static int GetBlueScore()
    {
        return blueScore;
    }

    public static int GetOrangeScore()
    {
        return orangeScore;
    }

    public static void ResetScore()
    {
        blueScore = orangeScore = 0;
    }

    void ResetLevel()
    {
        // Reset time scale
        Time.timeScale = 1.0f;

        // Reset victory text
        blueWin.SetActive(false);
        orangeWin.SetActive(false);

        // Reset fullness values
        blueEat.GetComponent<Eat>().fullness = 0.0f;
        orangeEat.GetComponent<Eat>().fullness = 0.0f;

        // Reset fullness meters
        GameObject meter = blueEat.GetComponent<Eat>().meter;
        meter.transform.localScale = new Vector3(meter.transform.localScale.x, 0.0f, meter.transform.localScale.z);
        meter = orangeEat.GetComponent<Eat>().meter;
        meter.transform.localScale = new Vector3(meter.transform.localScale.x, 0.0f, meter.transform.localScale.z);

        // Reset eating colliders
        blueEat.SetActive(true);
        orangeEat.SetActive(true);

        // Reset body colliders
        Score.canScore = true;

        // Reset the table position
        table.transform.position = new Vector3(0.0f, 0.5f, 0.0f);

        // Reset the food position
        food.SetActive(true);
        food.transform.position = new Vector3(0.0f, 1.1f, 0.0f);
        food.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        food.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        sauce.SetActive(false);

        // Show the countdown
        gameUI.SetActive(false);
        countdownUI.SetActive(true);
    }

    void EndRound()
    {
        if (GameOver())
        {
            // Show a victory thingy
            Debug.Log("You won!");

            // Switch to the victory screen
            SceneManager.LoadScene("VictoryScene");
        }

        // Reset the round
        ResetLevel();
    }
}
