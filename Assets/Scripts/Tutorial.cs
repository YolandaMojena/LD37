using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

    [SerializeField]
    private GameObject[] tutorialText;
    private int i = 0;

    const float LESSON_TIME = 6f;

	// Use this for initialization
	void Start () {

        if (GameManager.firstTime)
        {
            tutorialText[0].SetActive(true);
            StartCoroutine(switchText());
        }
        else gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator switchText()
    {
        yield return new WaitForSecondsRealtime(LESSON_TIME+i/2f);
        tutorialText[i].SetActive(false);
        i++;
        if (i < tutorialText.Length)
        {
            tutorialText[i].SetActive(true);
            StartCoroutine(switchText());
        }
        else
        {
            GameManager.firstTime = false;
            gameObject.SetActive(false);
        }
    }
}
