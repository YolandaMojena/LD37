using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer smallIcon;
    [SerializeField]
    private SpriteRenderer largeIcon;
    [SerializeField]
    private MeshRenderer hairColor;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetToradoreo(Sprite icon, Color color)
    {
        smallIcon.sprite = icon;
        largeIcon.sprite = icon;
        hairColor.material.SetColor("_EmissionColor", color);
    }
}
