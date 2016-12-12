using UnityEngine;
using System.Collections;

public class Sensei : MonoBehaviour {

    [SerializeField]
    private GameObject starPrefab;
    [SerializeField]
    private Transform LeftEye;
    [SerializeField]
    private Transform RightEye;
    [SerializeField]
    private Transform Head;

    private bool turn = true;
    private int messagesSent = 0;

    private const float INITIAL_FREQ = 10.0f;
    [SerializeField]
    private float cooldown = 5f;

    private const float WINDUP = 2f;
    private float windup = 0f;

    private Vector3 FIXED_FORWARD;
    private Vector3 FIXED_RIGHT;
    private Quaternion initialHeadRotation;
    private Quaternion winkingHeadRotation;

    // Use this for initialization
    void Start () {
        //waveFrequency = INITIAL_FREQ;

        //FIXED_FORWARD = transform.up;
        //FIXED_RIGHT = transform.
        initialHeadRotation = Head.rotation;
        Head.Rotate(Vector3.forward, -70f);
        winkingHeadRotation = Head.rotation;
        Head.rotation = initialHeadRotation;
	}
	
	// Update is called once per frame
	void Update () {

        /*if (cooldown > 0)
            cooldown -= Time.deltaTime;
        else
        {
            windup = WINDUP;
            cooldown = INITIAL_FREQ + WINDUP;
        }*/

        if (windup > 0)
        {
            windup -= Time.deltaTime;
            if(windup <= 0)
            {
                //LaunchStar(turn ? LeftEye.position : RightEye.position);
                //turn = !turn;
                LaunchStar(LeftEye.position);
            }

            Head.rotation = Quaternion.Lerp(Head.rotation, winkingHeadRotation, Time.deltaTime);
        }
        else
        {
            Head.rotation = Quaternion.Lerp(Head.rotation, initialHeadRotation, Time.deltaTime);
        }
	}

    void FixedUpdate()
    {
        Head.transform.eulerAngles = new Vector3(Head.transform.eulerAngles.x, Head.transform.eulerAngles.y + Mathf.Cos(Time.time*5f), Head.transform.eulerAngles.z);
    }

    void LaunchStar(Vector3 origin){
        GameObject newStar = Instantiate(starPrefab, origin, Quaternion.identity) as GameObject;
    }


    void SmoothLookTowards(Vector3 direction)
    {
        direction.Normalize();
        Quaternion currentRotation = Head.transform.rotation;
        Head.transform.LookAt(Head.transform.position + direction);
        Quaternion targetRotation = Head.transform.rotation;
        Head.transform.rotation = currentRotation;
        Head.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime);
    }

    public void Wink()
    {
        if (windup > 0f)
            LaunchStar(LeftEye.transform.position);
        windup = WINDUP;
    }
}
