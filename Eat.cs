using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour {

    public GameObject meter;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject table;
    [SerializeField] GameObject mouth;
    [SerializeField] GameObject manager;
    [SerializeField] AudioClip bravo;

    public float fullness;
    float eatSpeed = 0.3f;
    float lerpSpeed = 0.6f;
    float maxFull = 100.0f;

	// Use this for initialization
	void Start () {
        fullness = 0.0f;
        meter.transform.localScale = new Vector3(meter.transform.localScale.x, 0.0f, meter.transform.localScale.z);
    }

    // Update is called once per frame
    void Update () { }

    void OnTriggerStay(Collider other)
    {
        if (mouth.activeSelf == false)
        {
            mouth.SetActive(true);
        }

        if (other.tag == "Food")
        {
            fullness += eatSpeed;
        }

        if (fullness >= maxFull)
        {
            manager.GetComponent<RoundController>().IncrementScore(gameObject.tag == "Blue");
            winText.SetActive(true);
            gameObject.SetActive(false);
            Lift.canMove = false;
            other.gameObject.SetActive(false);
            mouth.SetActive(false);

            // Play the "Bravo" track
            table.GetComponent<AudioSource>().PlayOneShot(bravo, 4.0f);

            Time.timeScale = 0.25f;
        }

        meter.transform.localScale = new Vector3(meter.transform.localScale.x, Mathf.Lerp(meter.transform.localScale.y, fullness / maxFull, lerpSpeed), meter.transform.localScale.z);
    }

    private void OnTriggerExit(Collider other)
    {
        mouth.SetActive(false);
    }
}
