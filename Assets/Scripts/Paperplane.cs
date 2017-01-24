﻿using UnityEngine;
using System.Collections;

public class Paperplane : MonoBehaviour {

    bool collided = false;

    Vector3 lift_vector;
    float lift_magnitude = 9.81f;

    Vector3 drag_vector;
    float drag_magnitude = 0f;
    bool stall = false;
    const float MAX_SPEED = 6f;
    const float MIN_SPEED = 0.8f;
    const float BASE_LIFT = 9.31f;
    const float NOOB_FRIENDLY = 1.27f;

    public Rigidbody _rigidbody;

    [SerializeField]
    Transform paper;
    float turbulence = 0;

    AudioSource audioSource;

    [SerializeField]
    AudioSource deathAudio;

    //Controls
    Vector3 mouse_lastPosition;
    float tilt_magnitude;
    float pitch_magnitude;

    [SerializeField]
    GameObject mainCamera;
    [SerializeField]
    GameObject reset;
    [SerializeField]
    GameObject tutorial;
    [SerializeField]
    GameObject proTips;

    public bool delivering;
    public Kimmidoll destinatary;
    public int load = 0;

    // Use this for initialization
    void Start () {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward /* MAX_SPEED/1.5f*/, ForceMode.VelocityChange);
        //paper = GetComponentInChildren<Transform>();
        if (!mainCamera)
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (!collided)
        {
            Quaternion lastRotation = transform.rotation;

            audioSource.pitch = 0.5f + 2f * _rigidbody.velocity.magnitude / MAX_SPEED;
            audioSource.volume = 0.75f * _rigidbody.velocity.magnitude / MAX_SPEED;

            tilt_magnitude = Input.mousePosition.x / (Screen.width / 2f) - 1f;// / 50f;
            pitch_magnitude = Input.mousePosition.y / (Screen.height / 2f) - 1f;// / 50f;

            lift_magnitude = BASE_LIFT + Mathf.Sqrt(_rigidbody.velocity.magnitude) / 6f;
            lift_vector = (Vector3.up + transform.up) / 2f * lift_magnitude;
            //lift_magnitude = Mathf.Pow(_rigidbody.velocity.magnitude, 2) / MAX_SPEED;
            //lift_vector = transform.up * lift_magnitude;
            
            drag_magnitude = Mathf.Pow(_rigidbody.velocity.magnitude, 9) / Mathf.Pow(MAX_SPEED * 20, 3f) + (Mathf.Sqrt(Mathf.Pow(tilt_magnitude, 2) + Mathf.Pow(pitch_magnitude, 2)) * 2f * _rigidbody.velocity.magnitude / MAX_SPEED);
            drag_vector = -_rigidbody.velocity.normalized * drag_magnitude;
            //drag_vector = -_rigidbody.velocity.normalized * Mathf.Pow(_rigidbody.velocity.magnitude, 2)/MAX_SPEED;
            /*drag_magnitude = Mathf.Pow(_rigidbody.velocity.magnitude, 4) / 125f - 1f;
            if (drag_magnitude < 0f)
                drag_magnitude = 0;*/
            
            
            //lift_magnitude = (9.81f - 1f) + _rigidbody.velocity.magnitude;
            //_rigidbody.velocity += lift_vector * lift_magnitude * Time.deltaTime;


            // LIFT & DRAG
            _rigidbody.AddForce(lift_vector, ForceMode.Acceleration);
            _rigidbody.AddForce(drag_vector / 2f, ForceMode.Acceleration);

            // TILT
            _rigidbody.AddForce(transform.right * tilt_magnitude * (GameManager.NoobFriendly ? NOOB_FRIENDLY : 1f) * _rigidbody.velocity.magnitude, ForceMode.Acceleration);
            _rigidbody.AddTorque(transform.forward * -tilt_magnitude * _rigidbody.velocity.magnitude * 2f, ForceMode.Acceleration);
            //Vector3 fixedRigthVector = -Vector3.Cross(_rigidbody.velocity, transform.up);
            //_rigidbody.AddForce(fixedRigthVector * tilt_magnitude * _rigidbody.velocity.magnitude / MAX_SPEED / 4f, ForceMode.Acceleration);
            //_rigidbody.AddTorque(transform.up * tilt_magnitude / 10f, ForceMode.Acceleration);

            // PITCH
            _rigidbody.AddForce(transform.up * pitch_magnitude * (GameManager.NoobFriendly ? NOOB_FRIENDLY : 1f) * _rigidbody.velocity.magnitude, ForceMode.Acceleration);
            _rigidbody.AddTorque(transform.right * -pitch_magnitude * _rigidbody.velocity.magnitude / MAX_SPEED, ForceMode.Acceleration);


            //if (_rigidbody.velocity.magnitude < MIN_SPEED)
            //    _rigidbody.AddForce(-transform.up / Mathf.Pow(_rigidbody.velocity.magnitude, 2), ForceMode.Acceleration);
            //_rigidbody.AddForce(-transform.up * 2f * (lift_magnitude - BASE_LIFT), ForceMode.Acceleration);
            //transform.LookAt(Vector3.Lerp(transform.position + transform.forward, transform.position + Vector3.down, Time.deltaTime * 5));
            //else

            //transform.LookAt(Vector3.Lerp(transform.position + transform.forward, transform.position + _rigidbody.velocity, Time.deltaTime * 200));

            
            //transform.LookAt(Vector3.Lerp(transform.position + transform.forward, transform.position + _rigidbody.velocity, Time.deltaTime + _rigidbody.velocity.magnitude));
            
            if (transform.eulerAngles.x > 265 && transform.eulerAngles.x < 285) {
                if (_rigidbody.velocity.magnitude < MIN_SPEED)
                {
                    stall = true;
                }
                //transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90f * Time.deltaTime, transform.eulerAngles.y, transform.eulerAngles.z);
                
            }

            if (stall)
            {
                _rigidbody.angularVelocity = Vector3.zero;
                //transform.LookAt(transform.position + Vector3.Lerp(transform.forward, -transform.up, Time.deltaTime));
                //transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90f*Time.deltaTime, transform.eulerAngles.y, transform.eulerAngles.z);
                _rigidbody.velocity = Vector3.zero;
                transform.Rotate(transform.right, -10f);
                if (Vector3.Angle(transform.forward, Vector3.down) < 45f)
                    stall = false;
            }
            else
            {
                transform.LookAt(transform.position + _rigidbody.velocity);
            }

            VisualTilt();
        }
        else
        {
            mainCamera.transform.LookAt(transform);
            if (transform.position.y < 0f)
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        
        Logging();   
	}


    void VisualTilt()
    {
        float smoothness = 4f;

        //paper.rotation = transform.rotation;
        Vector3 currentRotation = paper.rotation.eulerAngles;
        if (currentRotation.z > 180f)
        {
            currentRotation.z = Mathf.Lerp(currentRotation.z, 360f+(-tilt_magnitude * 60f), Time.deltaTime * smoothness);
        }
        else
            currentRotation.z = Mathf.Lerp(currentRotation.z, -tilt_magnitude * 60f, Time.deltaTime * smoothness);



       /* float turbulent_threshold = MAX_SPEED - 1f;

        if (_rigidbody.velocity.magnitude > turbulent_threshold)
        {
            if (turbulence > 0)
                turbulence = -(_rigidbody.velocity.magnitude - turbulent_threshold);
            else
                turbulence = (_rigidbody.velocity.magnitude - turbulent_threshold);

            currentRotation.z = currentRotation.z + Mathf.Sqrt(turbulence);
        }
        else
            turbulence = 0;*/


        paper.eulerAngles = currentRotation;
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (!collided)
        {
			if(!other.gameObject.name.Contains("envelope") && other.gameObject.tag != "DeliveryArea" && other.gameObject.tag != "Star") {
                Death();
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Star")
        {
            //THIS IS OK
            float star_force = 100f;
            _rigidbody.AddForce(transform.forward * star_force * Time.deltaTime, ForceMode.Acceleration);

            //Vector3 accelerationDirection = (other.transform.forward + transform.forward).normalized;
            //star_force /= 10f*(Vector3.Dot(other.transform.forward, transform.forward)+1f)/2f;
            //_rigidbody.AddForce(accelerationDirection * star_force * Time.deltaTime, ForceMode.Acceleration);
            /*float star_force = 400f;
            //Vector3 accelerationDirection = (other.transform.forward + transform.forward).normalized;
            //star_force /= 10f * (Vector3.Dot(other.transform.forward, transform.forward) + 1f) / 2f;
            _rigidbody.AddForce(transform.forward * star_force * Time.deltaTime, ForceMode.Acceleration);*/
        }

        if (transform.position.y < 0)
            transform.position += new Vector3(0f, -transform.position.y, 0f);
    }
    /*void OnCollisionEnter(Collision collision)
    {
        if (!collided)
        {
            collided = true;
            //_rigidbody.velocity = -_rigidbody.velocity * 0.8f;
        }
    }*/

    void Logging()
    {
        //Debug.Log("-----------------------------------");
        //Debug.Log("Velocity: " + _rigidbody.velocity.magnitude + " Lift: " + lift_magnitude + " Drag: " + drag_magnitude);
        Debug.Log(transform.up);
        //Debug.Log("Tilt: " + tilt_magnitude + " Pitch: " + pitch_magnitude);
        //Debug.Log(paper.rotation.eulerAngles.z + ", " + -tilt_magnitude * 60f);
    }

    IEnumerator DisplayReset()
    {
        yield return new WaitForSecondsRealtime(4.0f);
        tutorial.SetActive(false);
        reset.SetActive(true);
        proTips.SetActive(true);
    }

    public void Death()
    {
        collided = true;
        _rigidbody.velocity = -_rigidbody.velocity * 0.5f;
        mainCamera.transform.parent = transform.parent;
        Time.timeScale = 0.5f;
        GetComponentInChildren<MeshCollider>().isTrigger = false;
        foreach (TrailRenderer tr in GetComponentsInChildren<TrailRenderer>())
            tr.enabled = false;

        audioSource.pitch = 0.5f * 2f * _rigidbody.velocity.magnitude / MAX_SPEED;
        audioSource.volume = 0.25f * 2f * _rigidbody.velocity.magnitude / MAX_SPEED;

        deathAudio.Play();

        StartCoroutine(DisplayReset());
    }
}
