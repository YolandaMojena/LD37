using UnityEngine;
using System.Collections;

public class StarManager : MonoBehaviour {

    [SerializeField]
    private GameObject starPrefab;
    [SerializeField]
    private Transform teacherLeftEye;
    [SerializeField]
    private Transform teacherRightEye;

    private int turn = 1;

    private float frequency = 0;
    private float timer = 0;

    private float INITIAL_FREQ = 10.0f;
    private float increment = 0.5f;

	// Use this for initialization
	void Start () {

        frequency = INITIAL_FREQ;	
	}
	
	// Update is called once per frame
	void Update () {

        if (timer >= frequency)
        {
            LaunchStar(turn < 0 ? teacherLeftEye.position : teacherRightEye.position);
            if (frequency > increment)
                frequency -= increment;
            turn *= -1;
            timer = 0;
        }
        else timer += Time.deltaTime;
	}

    void LaunchStar(Vector3 origin){

        GameObject newStar = Instantiate(starPrefab, origin, Quaternion.identity) as GameObject;
    }
}
