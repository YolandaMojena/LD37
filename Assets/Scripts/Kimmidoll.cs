using UnityEngine;
using System.Collections;

public class Kimmidoll : MonoBehaviour {

    [SerializeField]
    Transform LArm;
    [SerializeField]
    Transform RArm;
    [SerializeField]
    Transform LHand;
    [SerializeField]
    Transform RHand;
    [SerializeField]
    MeshRenderer Hair;

    public bool gender;
    public Color hairColor;
    public bool excited = false;

    Quaternion LInitialRotation;
    Quaternion RInitialRotation;
    Quaternion initialRotation;
    private Vector3 ENVELOPE_OFFSET_LEFT = new Vector3(-0.038f, -0.17f, 0);
    private Vector3 ENVELOPE_OFFSET_RIGHT = new Vector3(0.036f, -0.17f, 0);

    const float AMPLITUDE = 10f;

    bool LArm_excited = true;
    bool excitement_boost = false;
    float timer = 0f;
    const float BOOST_INTERVAL = 3f;

    //Kimmidoll -> 25
    //Arm       -> [-25, -40]


    // Use this for initialization
    void Start () {
        LInitialRotation = LArm.rotation;
        RInitialRotation = RArm.rotation;
        initialRotation = transform.rotation;
        //BecomeExcited();
        //StopExcitement();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (excited)
        {
            // Initial Steep
            if (LArm_excited)
            {
                /*Vector3 targetRotation = new Vector3(0f,0f,335f);
                transform.eulerAngles = new Vector3(Mathf.Lerp(transform.eulerAngles.x, targetRotation.x, Time.deltaTime),
                                                    Mathf.Lerp(transform.eulerAngles.y, targetRotation.y, Time.deltaTime),
                                                    Mathf.Lerp(transform.eulerAngles.z, targetRotation.z, Time.deltaTime));*/
                Quaternion currentRotation = transform.rotation;
                transform.eulerAngles = new Vector3(0f, 0f, 335f);
                Quaternion targetRotation = transform.rotation;
                transform.rotation = currentRotation;
                transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation, Time.deltaTime);
            }
            else
            {
                Vector3 targetRotation = new Vector3(0f, 0f, 25f);
                transform.eulerAngles = new Vector3(Mathf.Lerp(transform.eulerAngles.x, targetRotation.x, Time.deltaTime),
                                                    Mathf.Lerp(transform.eulerAngles.y, targetRotation.y, Time.deltaTime),
                                                    Mathf.Lerp(transform.eulerAngles.z, targetRotation.z, Time.deltaTime));
            }


            //Boost enabler
            if (timer < BOOST_INTERVAL)
                timer += Time.deltaTime;
            else
            {
                if (excitement_boost)
                {
                    excitement_boost = false;
                    timer = 0f;
                }
                else if (Random.value > 0.5f)
                {
                    excitement_boost = true;
                    timer = BOOST_INTERVAL - 1f;
                }
                else
                    timer = 0f;
            }

            // ARM WAVING
            float freq_booster = 5f;
            if (excitement_boost) freq_booster = 50f;

            if (LArm_excited)
            {
                LArm.eulerAngles = new Vector3(-40f + Mathf.Sin(Time.time * freq_booster) * AMPLITUDE, -90, 90);
            }
            else
            {
                RArm.eulerAngles = new Vector3(-40f + Mathf.Sin(Time.time * freq_booster) * AMPLITUDE, 90, -90);
            }
        }
        // BACK TO IDLE POSITION
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime);
            LArm.rotation = Quaternion.Lerp(LArm.rotation, LInitialRotation, Time.deltaTime);
            RArm.rotation = Quaternion.Lerp(RArm.rotation, RInitialRotation, Time.deltaTime);
        }
	}

    public void BecomeExcited()
    {
        excited = true;

        if (Random.value > 0.5f)
            LArm_excited = true;
        else
            LArm_excited = false;
    }
    public void StopExcitement()
    {
        excited = false;
    }

    public void SetHairColor(Color c)
    {
        hairColor = c;
        Hair.material.SetColor("_EmissionColor", c);
    }

    public void HoldMessage(GameObject envelope)
    {
        if (LArm_excited)
        {
            envelope.transform.position = LHand.position + ENVELOPE_OFFSET_LEFT;
            envelope.transform.Rotate(new Vector3(0, 180, 180));
            envelope.transform.SetParent(LHand);
            LHand.eulerAngles += new Vector3(0, -60, 0);
        }

        else
        {
            envelope.transform.position = RHand.position + ENVELOPE_OFFSET_RIGHT;
            envelope.transform.Rotate(new Vector3(0, 0, 180));
            envelope.transform.SetParent(RHand);
            RHand.eulerAngles += new Vector3(0, 60, 0);
        }    
    }
}
