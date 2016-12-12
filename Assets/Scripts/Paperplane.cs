using UnityEngine;
using System.Collections;

public class Paperplane : MonoBehaviour {

    bool collided = false;

    Vector3 lift_vector;
    float lift_magnitude = 9.81f;

    Vector3 drag_vector;
    float drag_magnitude = 0f;
    const float MAX_SPEED = 6f;
    const float MIN_SPEED = 0.2f;

    Rigidbody _rigidbody;

    [SerializeField]
    Transform paper;
    float turbulence = 0;

    //Controls
    Vector3 mouse_lastPosition;
    float tilt_magnitude;
    float pitch_magnitude;

    [SerializeField]
    GameObject mainCamera;

    public bool delivering;
    public Kimmidoll destinatary;

    // Use this for initialization
    void Start () {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * MAX_SPEED/1.5f, ForceMode.VelocityChange);
        //paper = GetComponentInChildren<Transform>();
        if (!mainCamera)
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (!collided)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _rigidbody.AddForce(_rigidbody.velocity.normalized * MAX_SPEED / 1.5f, ForceMode.VelocityChange);
            if (Input.GetKeyDown(KeyCode.LeftControl))
                collided = true;

            tilt_magnitude = Input.mousePosition.x / (Screen.width / 2f) - 1f;// / 50f;
            pitch_magnitude = Input.mousePosition.y / (Screen.height / 2f) - 1f;// / 50f;



            //lift_magnitude = Mathf.Pow(_rigidbody.velocity.magnitude, 2) / MAX_SPEED;
            lift_magnitude = 9.31f + Mathf.Sqrt(_rigidbody.velocity.magnitude) / 6f;
            //lift_vector = transform.up * lift_magnitude;
            lift_vector = (Vector3.up + transform.up) / 2f * lift_magnitude;
            //drag_vector = -_rigidbody.velocity.normalized * Mathf.Pow(_rigidbody.velocity.magnitude, 2)/MAX_SPEED;
            /*drag_magnitude = Mathf.Pow(_rigidbody.velocity.magnitude, 4) / 125f - 1f;
            if (drag_magnitude < 0f)
                drag_magnitude = 0;*/
            drag_magnitude = Mathf.Pow(_rigidbody.velocity.magnitude, 9) / Mathf.Pow(MAX_SPEED * 20, 3f) + (Mathf.Sqrt(Mathf.Pow(tilt_magnitude, 2) + Mathf.Pow(pitch_magnitude, 2)) * 2f * _rigidbody.velocity.magnitude / MAX_SPEED);
            drag_vector = -_rigidbody.velocity.normalized * drag_magnitude;
            //lift_magnitude = (9.81f - 1f) + _rigidbody.velocity.magnitude;
            //_rigidbody.velocity += lift_vector * lift_magnitude * Time.deltaTime;


            // LIFT & DRAG
            _rigidbody.AddForce(lift_vector, ForceMode.Acceleration);
            _rigidbody.AddForce(drag_vector / 2f, ForceMode.Acceleration);

            // TILT
            //Vector3 fixedRigthVector = -Vector3.Cross(_rigidbody.velocity, transform.up);
            _rigidbody.AddForce(transform.right * tilt_magnitude * _rigidbody.velocity.magnitude, ForceMode.Acceleration);
            //_rigidbody.AddForce(fixedRigthVector * tilt_magnitude * _rigidbody.velocity.magnitude / MAX_SPEED / 4f, ForceMode.Acceleration);
            _rigidbody.AddTorque(transform.forward * -tilt_magnitude * _rigidbody.velocity.magnitude * 2f, ForceMode.Acceleration);
            //_rigidbody.AddTorque(transform.up * tilt_magnitude / 10f, ForceMode.Acceleration);

            // PITCH
            _rigidbody.AddForce(transform.up * pitch_magnitude * _rigidbody.velocity.magnitude, ForceMode.Acceleration);
            _rigidbody.AddTorque(transform.right * -pitch_magnitude * _rigidbody.velocity.magnitude / MAX_SPEED, ForceMode.Acceleration);

            if (_rigidbody.velocity.magnitude < MIN_SPEED)
                transform.LookAt(Vector3.Lerp(transform.position + transform.forward, transform.position + Vector3.down, Time.deltaTime * 5));
            else
                transform.LookAt(transform.position + _rigidbody.velocity);
            //transform.LookAt(Vector3.Lerp(transform.position + transform.forward, transform.position + _rigidbody.velocity, Time.deltaTime * 200));

            VisualTilt();
        }
        else
        {
            mainCamera.transform.LookAt(transform);
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
			if(!other.gameObject.name.Contains("envelope") && other.gameObject.tag != "DeliveryArea") {
				collided = true;
				_rigidbody.velocity = -_rigidbody.velocity * 0.5f;
				mainCamera.transform.parent = transform.parent;
				Time.timeScale = 0.5f;
				GetComponentInChildren<MeshCollider>().isTrigger = false;
				foreach (TrailRenderer tr in GetComponentsInChildren<TrailRenderer>())
					tr.enabled = false;
            }
        }
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
        //Debug.Log("Tilt: " + tilt_magnitude + " Pitch: " + pitch_magnitude);
        //Debug.Log(paper.rotation.eulerAngles.z + ", " + -tilt_magnitude * 60f);
    }
}
