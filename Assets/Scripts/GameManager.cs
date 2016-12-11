using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField]
    private Color[] hairColors;

    private List<Color> boyColors;
    private List<Color> girlColors;


    /*const*/
    Vector3 DOLL_OFFSET = new Vector3(0,1.27f, -0.25f);

    [SerializeField]
    private List<Kimmidoll> pupils;
    [SerializeField]
    private Kimmidoll destinatary;
    private Color destColor;
    [SerializeField]
    private Kimmidoll sender;

    

    int boys = 8;
    int girls = 8;

    // Use this for initialization
    void Start () {

        //GameObject newMessage = Instantiate(envelope, transform.position, Quaternion.identity) as GameObject;
        //newMessage.GetComponent<Message>().SetToradoreo(male, Color.blue);

        boyColors = new List<Color>(hairColors);
        girlColors = new List<Color>(hairColors);

        foreach (GameObject chair in GameObject.FindGameObjectsWithTag("Chair"))
        {
            if(boys == 0)
            {
                SpawnGirl(chair.transform.position);
            }
            else if(girls == 0)
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
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateNewMessage()
    {
        sender = pupils[Random.Range(0, pupils.Count - 1)];

        destinatary = pupils[Random.Range(0, pupils.Count - 1)];
        while(destinatary == sender)
            destinatary = pupils[Random.Range(0, pupils.Count - 1)];

        // Place envelope in hand
        GameObject newMessage = Instantiate(envelope, sender.transform.position, Quaternion.identity) as GameObject;

        Pupil pupilDest = destinatary.GetComponent<Pupil>();
        newMessage.GetComponent<Message>().SetToradoreo(pupilDest.Gender == 0 ? male : female, pupilDest.HairColor);
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
}
