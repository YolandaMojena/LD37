using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer smallIcon;
    [SerializeField]
    private SpriteRenderer largeIcon;
    [SerializeField]
    private MeshRenderer hairColor;

    private bool inTransit = false;
    private Kimmidoll sender;
    private Kimmidoll destinatary;
    private Vector3 DELIVERY_OFFSET = new Vector3(0, 0.3f, 0);

    // Use this for initialization

    public void SetToradoreo(Sprite icon, Color color, Kimmidoll sender, Kimmidoll destinatary)
    {
        smallIcon.sprite = icon;
        largeIcon.sprite = icon;
        BlackHairCheat(color);
        hairColor.material.SetColor("_EmissionColor", color);
        this.sender = sender;
        this.destinatary = destinatary;
    }

    private void BlackHairCheat(Color color)
    {
        if(color == Color.black){
            foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                mr.material.SetColor("_EmissionColor", color);
            hairColor.material.SetColor("_EmissionColor", Color.white);
        }
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
            }
        }
        else if(other.gameObject.tag == "DeliveryArea" && other.transform.root.gameObject == destinatary.gameObject)
        {
            GameManager.LettersHandedIn++;
            GameManager.Plane.delivering = false;
            GameManager.Plane.destinatary = null;
            GameManager.Sensei.Wink();
            Destroy(gameObject, 0.2f);
        }
    }
}
