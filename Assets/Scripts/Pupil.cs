using UnityEngine;
using System.Collections;

public class Pupil : MonoBehaviour {

    [SerializeField]
    private int gender;
    [SerializeField]
    private Color hairColor;

    public int Gender
    {
        get { return gender; }
    }

    public Color HairColor
    {
        get { return hairColor; }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
