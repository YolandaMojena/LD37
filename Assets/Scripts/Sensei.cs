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

    private int turn = 1;
    private int messagesSent = 0;

    private const float INITIAL_FREQ = 10.0f;
    private float waveFrequency = 0f;
    [SerializeField]
    private float waveTimer = 0f;
    private float showtime = 0f;
    private float cadency = 0.5f;
    [SerializeField]
    private float cooldown = 0f;

    private Vector3 FIXED_FORWARD;
    private Vector3 FIXED_RIGHT;

    // Use this for initialization
    void Start () {
        waveFrequency = INITIAL_FREQ;

        FIXED_FORWARD = transform.up;
        //FIXED_RIGHT = transform.
	}
	
	// Update is called once per frame
	void Update () {

        if (waveTimer >= waveFrequency)
        {
            //LaunchStar(turn < 0 ? LeftEye.position : RightEye.position);
            showtime = messagesSent + 1;

            waveFrequency = INITIAL_FREQ - Mathf.Sqrt(messagesSent);
            if (waveFrequency < 1f)
                waveFrequency = 1f;
            waveFrequency += showtime;

            turn *= -1;
            waveTimer = 0;
        }
        else
            waveTimer += Time.deltaTime;

        if(showtime > 0f)
        {
            SmoothLookTowards(-transform.right);
            if (cooldown >= cadency)
            {
                LaunchStar(turn < 0 ? LeftEye.position : RightEye.position);
                cooldown = 0;
            }
            else
                cooldown += Time.deltaTime;

            showtime -= Time.deltaTime;
        }
        else
        {
            //SmoothLookTowards(FIXED_FORWARD);
        }
	}

    void FixedUpdate()
    {
        Head.transform.eulerAngles = new Vector3(Head.transform.eulerAngles.x, Head.transform.eulerAngles.y + Mathf.Sin(Time.time*5f)/7f, Head.transform.eulerAngles.z);
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
}
