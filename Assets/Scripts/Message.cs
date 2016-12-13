using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer smallIcon;
    [SerializeField]
    private SpriteRenderer largeIcon;
    [SerializeField]
    private MeshRenderer hairColor;
    [SerializeField]
    private AudioClip pickingClip;
    [SerializeField]
    private AudioClip deliveringClip;

    AudioSource audio;

    private bool inTransit = false;
    private Kimmidoll sender;
    private Kimmidoll destinatary;
    private Vector3 DELIVERY_OFFSET = new Vector3(0, 0.3f, 0);

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void SetToradoreo(Sprite icon, Color color, Kimmidoll sender, Kimmidoll destinatary)
    {
        smallIcon.sprite = icon;
        largeIcon.sprite = icon;
        if (color.r == 0 && color.g == 0 && color.b == 0)
            color = Color.grey * 0.4f;
        hairColor.material.SetColor("_EmissionColor", color);
        this.sender = sender;
        this.destinatary = destinatary;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(!inTransit)
        {
            if (other.gameObject.name.Contains("paper") && (!GameManager.Plane.delivering || GameManager.Plane.destinatary == destinatary))
            {
                transform.localScale *= 0.5f;
                transform.rotation = other.transform.rotation;
                transform.parent = other.transform;
                transform.position = other.transform.position;
                transform.position += -transform.forward * 0.3f + transform.up*0.074f;
                transform.eulerAngles += new Vector3(-5f, 0f, 0f);

                inTransit = true;
                sender.StopExcitement();
                GameManager.Plane.delivering = true;
                GameManager.Plane.destinatary = destinatary;
                GameManager.Plane.load++;

                audio.clip = pickingClip;
                audio.Play();
            }
        }
        else if(other.gameObject.tag == "DeliveryArea" && other.transform.root.gameObject == destinatary.gameObject)
        {
            GameManager.LettersHandedIn++;
            GameManager.Score += GameManager.Plane.load * 10;
            GameManager.UpdateScoreText();
            GameManager.Plane.load--;
            GameManager.Plane.delivering = false;
            GameManager.Plane.destinatary = null;
            GameManager.Sensei.Wink();
            foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                mr.enabled = false;
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            audio.clip = deliveringClip;
            audio.Play();
            Destroy(gameObject, 0.6f);
        }
    }
}
