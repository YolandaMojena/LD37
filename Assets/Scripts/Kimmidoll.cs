using UnityEngine;
using System.Collections;

public class Kimmidoll : MonoBehaviour {

    [SerializeField]
    Transform LArm;
    [SerializeField]
    Transform RArm;

    bool excited = false;
    bool LArm_excited = true;

    //Kimmidoll -> 25
    //Arm       -> [-25, -40]


    // Use this for initialization
    void Start () {
        BecomeExcited();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (excited)
        {
            /*if (LArm_excited)
            {

            }
            else
            {

            }*/
            RArm.eulerAngles = new Vector3(-35f + Mathf.Sin(Time.time*5f) * 15f, 90, -90);
        }
	}

    void BecomeExcited()
    {
        excited = true;

        if (Random.value > 0.5f)
            LArm_excited = true;
        LArm_excited = false;

        if (LArm_excited)
            transform.eulerAngles = new Vector3(0, 0, -25);
        else
            transform.eulerAngles = new Vector3(0, 0, 25);
    }
    void StopExcitement()
    {
        excited = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

}
