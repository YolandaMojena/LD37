using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

    private float velY = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(new Vector3(0, velY * Time.deltaTime, 0));
	}

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Classroom", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
