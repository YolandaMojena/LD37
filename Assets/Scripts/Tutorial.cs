using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

    [SerializeField]
    private GameObject[] tutorialText;
    private int i = 0;

	// Use this for initialization
	void Start () {

        if (GameManager.firstTime)
        {
            GameManager.firstTime = false;
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
        yield return new WaitForSecondsRealtime(10f);
        tutorialText[i].SetActive(false);
        i++;
        if (i < tutorialText.Length)
        {
            tutorialText[i].SetActive(true);
            StartCoroutine(switchText());
        }
        else gameObject.SetActive(false);
    }
}
