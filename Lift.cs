using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {

    [SerializeField] GameObject blueMeter;
    [SerializeField] GameObject orangeMeter;
    [SerializeField] GameObject ReturnUI;
    [SerializeField] float strength = 0.3f;
    [SerializeField] float minHeight = 0.5f;
    [SerializeField] float maxHeight = 2.5f;
    [SerializeField] bool spamEnabled = false;

    public static bool canMove = false;

    float currLeft, currRight, targetLeft, targetRight, heightDiff, angle, blueStamina, orangeStamina;
    float lerpSpeed = 0.6f;
    float maxBar = 20.0f;
    float rechargeFactor = 0.25f;
    float spamMult = 3.0f;

    // Use this for initialization
    void Start () {
        targetLeft = minHeight;
        targetRight = minHeight;
        currLeft = minHeight;
        currRight = minHeight;
        blueStamina = 100.0f;
        orangeStamina = 100.0f;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        // Check for game quitting
        if (canMove && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0.0f;
            ReturnUI.SetActive(true);
        }

        // Even out the table under a certain rotation threshold
        if (Mathf.Abs(transform.rotation.z) <= 0.1f)
        {
            transform.rotation = Quaternion.identity;
        }

        // Prevent the table from falling below a certain height
        if (transform.position.y < 0.5f)
        {
            transform.position = new Vector3(0, 0.5f, 0.0f);
        }

        // Update the left side's target height and stamina
        if (canMove && (spamEnabled? Input.GetKeyDown(KeyCode.W) : Input.GetKey(KeyCode.W)))
        {
            if (Mathf.Max(orangeStamina - (targetLeft / maxHeight + 1) * (spamEnabled ? strength * spamMult : strength), 0.0f) > 0.0f)
            {
                targetLeft = Mathf.Min(targetLeft + (spamEnabled ? strength * spamMult : strength), maxHeight);
                orangeStamina = Mathf.Max(orangeStamina - (targetLeft / maxHeight + 1.5f) * (spamEnabled ? strength * spamMult : strength), 0.0f);
            }
            else
            {
                targetLeft = Mathf.Max(targetLeft - rechargeFactor * strength, minHeight);
            }
        }
        else
        {
            targetLeft = Mathf.Max(targetLeft - rechargeFactor * strength, minHeight);
            orangeStamina = Mathf.Min(orangeStamina + rechargeFactor * strength, maxBar);
        }

        // Update the right side's target height and stamina
        if (canMove && (spamEnabled? Input.GetKeyDown(KeyCode.UpArrow) : Input.GetKey(KeyCode.UpArrow)))
        {
            if (Mathf.Max(blueStamina - (targetLeft / maxHeight + 1) * (spamEnabled ? strength * spamMult : strength), 0.0f) > 0.0f)
            {
                targetRight = Mathf.Min(targetRight + (spamEnabled ? strength * spamMult : strength), maxHeight);
                blueStamina = Mathf.Max(blueStamina - (targetLeft / maxHeight + 1.5f) * (spamEnabled ? strength * spamMult : strength), 0.0f);
            }
            else
            {
                targetRight = Mathf.Max(targetRight - rechargeFactor * strength, minHeight);
            }
        }
        else
        {
            targetRight = Mathf.Max(targetRight - rechargeFactor * strength, minHeight);
            blueStamina = Mathf.Min(blueStamina + rechargeFactor * strength, maxBar);
        }

        // Update the left and right stamina meters
        Transform blueTransform = blueMeter.transform;
        blueTransform.localScale = new Vector3(blueTransform.localScale.x, Mathf.Lerp(blueTransform.localScale.y, blueStamina / maxBar, lerpSpeed), blueTransform.localScale.z);

        Transform orangeTransform = orangeMeter.transform;
        orangeTransform.localScale = new Vector3(orangeTransform.localScale.x, Mathf.Lerp(orangeTransform.localScale.y, orangeStamina / maxBar, lerpSpeed), orangeTransform.localScale.z);

        // Update the left and right positions
        currLeft = Mathf.Lerp(currLeft, targetLeft, lerpSpeed);
        currRight = Mathf.Lerp(currRight, targetRight, lerpSpeed);

        heightDiff = Mathf.Abs(currRight - currLeft);

        // Updates the table's position
        transform.position = new Vector3(transform.position.x, Mathf.Min(currLeft, currRight) + heightDiff / 2);

        // Updates the table's rotation
        angle = Mathf.Asin(heightDiff / transform.localScale.x) * 180 / Mathf.PI;

        // Negative rotation
        if (currLeft > currRight)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, -angle);
        }
        // Positive rotation
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
    }
}
