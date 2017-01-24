using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Sprite female;
    [SerializeField]
    private Sprite male;
    [SerializeField]
    private GameObject envelope;
    [SerializeField]
    private GameObject boyDoll;
    [SerializeField]
    private GameObject girlDoll;

    
    private static AudioSource audioSource;
    private static bool muted = false;
    public static bool NoobFriendly = false;
    //[SerializeField]
    //private AudioClip audioClip;
    
    [SerializeField]
    private Color[] hairColors;
    

    public static Sensei Sensei;

    public static Paperplane Plane;
    public static int LettersHandedIn = 0;
    public static int Score = 0;
    private static Text scoreText;
    public static bool firstTime = true;
    private float timer = 0;
    private float messageFrequency = 0;
    private const float BASE_FREQ = 12f;
    private const float FREQ_DECAY = 0.5f;

    private List<Color> boyColors;
    private List<Color> girlColors;

    /*const*/
    Vector3 DOLL_OFFSET = new Vector3(0,1.27f, -0.25f);

    [SerializeField]
    private List<Kimmidoll> pupils;
    private Color destColor;

    int boys = 8;
    int girls = 8;

    void Awake()
    {
        pupils = new List<Kimmidoll>();
        boyColors = new List<Color>(hairColors);
        girlColors = new List<Color>(hairColors);
        audioSource = GetComponent<AudioSource>();
        audioSource.mute = muted;

        foreach (GameObject chair in GameObject.FindGameObjectsWithTag("Chair"))
        {
            if (boys == 0)
            {
                SpawnGirl(chair.transform.position);
            }
            else if (girls == 0)
            {
                SpawnBoy(chair.transform.position);
            }
            else
            {
                if (Random.value > 0.5f)
                    SpawnBoy(chair.transform.position);
                else
                    SpawnGirl(chair.transform.position);
            }
        }
    }

    // Use this for initialization
    void Start () {

        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Classroom")
        {
            Plane = GameObject.Find("paperplane").GetComponent<Paperplane>();
            Sensei = GameObject.Find("Sensei").GetComponent<Sensei>();
            //messageFrequency = 10 - Mathf.Sqrt(LettersHandedIn);
            if (firstTime)
                messageFrequency = BASE_FREQ;
            else
                messageFrequency = BASE_FREQ/2f;
            scoreText = GameObject.Find("Score").GetComponent<Text>();
        }
        else
        {
            if(muted)
                GameObject.Find("MuteBtn").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/silentNote");
        }
        LettersHandedIn = 0;
        Score = 0;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update () {

        if (timer >= messageFrequency)
        {
            GenerateMessage();
            timer = 0;
            //messageFrequency = 10 - Mathf.Sqrt(LettersHandedIn);
            messageFrequency = BASE_FREQ - Mathf.Sqrt(LettersHandedIn) * FREQ_DECAY;
        }
        else
            timer += Time.deltaTime;
	}

    void GenerateMessage()
    {
        /*Kimmidoll sender = pupils[Random.Range(0, pupils.Count - 1)];
        while(sender.excited)
            sender = pupils[Random.Range(0, pupils.Count - 1)];*/

        List<Kimmidoll> potentialSenders = new List<Kimmidoll>();
        foreach (Kimmidoll ps in pupils)
            if (!ps.excited)
                potentialSenders.Add(ps);

        if (potentialSenders.Count > 0)
        {
            Kimmidoll sender = potentialSenders[Random.Range(0, potentialSenders.Count - 1)];
            Kimmidoll destinatary = pupils[Random.Range(0, pupils.Count - 1)];
            while (destinatary == sender)
                destinatary = pupils[Random.Range(0, pupils.Count - 1)];

            // Place envelope in hand
            GameObject newMessage = Instantiate(envelope) as GameObject;
            newMessage.GetComponent<Message>().SetToradoreo(!destinatary.gender ? male : female, destinatary.hairColor, sender, destinatary);
            sender.BecomeExcited();
            sender.HoldMessage(newMessage);
        }
        else
        {
            Plane.Death();
            Plane._rigidbody.velocity = -Plane._rigidbody.velocity; // If you are seeing this, don't take this into consideration. I know it's wrong. But it's a jam. And I'm hungry :D
        }
    }

    void SpawnBoy(Vector3 position)
    {
        boys--;
        GameObject newPupil = GameObject.Instantiate(boyDoll, position + DOLL_OFFSET, Quaternion.identity) as GameObject;
        Kimmidoll newKimmidoll = newPupil.GetComponent<Kimmidoll>();
        int i = Mathf.FloorToInt(Random.value * boyColors.Count * 0.99999999f);
        newKimmidoll.SetHairColor(boyColors[i]);
        boyColors.RemoveAt(i);
        AddKimmidoll(newKimmidoll);
    }
    void SpawnGirl(Vector3 position)
    {
        girls--;
        GameObject newPupil = GameObject.Instantiate(girlDoll, position + DOLL_OFFSET, Quaternion.identity) as GameObject;
        Kimmidoll newKimmidoll = newPupil.GetComponent<Kimmidoll>();
        int i = Mathf.FloorToInt(Random.value * girlColors.Count * 0.99999999f);
        newKimmidoll.SetHairColor(girlColors[i]);
        girlColors.RemoveAt(i);
        AddKimmidoll(newKimmidoll);
    }
    void AddKimmidoll(Kimmidoll newPupil)
    {
        pupils.Add(newPupil);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Classroom", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void BackToMainMenuBtn()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void MuteBtn(GameObject caller)
    {
        audioSource.mute = !audioSource.mute;
        muted = !muted;
        Image img = caller.GetComponent<Image>();
        if (img.sprite.name == "note")
            img.sprite = Resources.Load<Sprite>("Sprites/silentNote");
        else
            img.sprite = Resources.Load<Sprite>("Sprites/note");
    }

    public static void UpdateScoreText()
    {
        scoreText.text = Score.ToString();
    }
}
