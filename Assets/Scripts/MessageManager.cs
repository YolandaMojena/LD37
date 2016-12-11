using UnityEngine;
using System.Collections;

public class MessageManager : MonoBehaviour {

    [SerializeField]
    private Sprite female;
    [SerializeField]
    private Sprite male;
    [SerializeField]
    private GameObject envelope;
    [SerializeField]
    private GameObject[] classroom;
    [SerializeField]
    private GameObject destinatary;
    private Color destColor;
    [SerializeField]
    private GameObject sender;

    // Use this for initialization
    void Start () {

        GameObject newMessage = Instantiate(envelope, transform.position, Quaternion.identity) as GameObject;
        newMessage.GetComponent<Message>().SetToradoreo(male, Color.blue);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateNewMessage()
    {
        sender = classroom[Random.Range(0, classroom.Length - 1)];

        destinatary = classroom[Random.Range(0, classroom.Length - 1)];
        while(destinatary == sender)
            destinatary = classroom[Random.Range(0, classroom.Length - 1)];

        // Place envelope in hand
        GameObject newMessage = Instantiate(envelope, sender.transform.position, Quaternion.identity) as GameObject;

        Pupil pupilDest = destinatary.GetComponent<Pupil>();
        newMessage.GetComponent<Message>().SetToradoreo(pupilDest.Gender == 0 ? male : female, pupilDest.HairColor);
    }
}
