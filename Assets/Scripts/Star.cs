using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    private float velocity = 0;
    //private Vector3 targetDir = new Vector3();
    GameObject target;
    Rigidbody targetRigidbody;
    TrailRenderer trail;

    // Use this for initialization
    void Start () {

        velocity = Random.Range(2.5f, 3.5f);
        target = GameObject.Find("paperplane");
        targetRigidbody = target.GetComponent<Rigidbody>();
        transform.LookAt(target.transform.position);
        trail = GetComponentInChildren<TrailRenderer>();
        //targetDir = target.transform.position - transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        /*targetDir = target.transform.position - transform.position;
        transform.position += targetDir.normalized * velocity * Time.deltaTime;
        transform.LookAt(target.transform.position);*/
        SmoothLookAt(target.transform.position + targetRigidbody.velocity/3f);
        transform.position += transform.forward * velocity * Time.deltaTime;
        //transform.eulerAngles += new Vector3(0f, 0f, 90f * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent && other.transform.parent.name != "Sensei")
        {
            trail.transform.parent = null;
            Destroy(gameObject, 0.2f);
        }
    }

    void SmoothLookAt(Vector3 target)
    {
        const float SMOOTHNESS = 0.75f;
        Quaternion currentRotation = transform.rotation;
        transform.LookAt(target);
        Quaternion targetRotation = transform.rotation;
        transform.rotation = currentRotation;
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime / SMOOTHNESS);
    }
}
