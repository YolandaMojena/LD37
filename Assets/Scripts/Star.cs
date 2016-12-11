using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    private float velocity = 0;
    private Vector3 targetDir = new Vector3();
    [SerializeField]
    GameObject target;

    // Use this for initialization
    void Start () {

        velocity = Random.Range(2.5f, 4.0f);
        target = GameObject.Find("paperplane");
        targetDir = target.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        targetDir = target.transform.position - transform.position;
        transform.position += targetDir.normalized * velocity * Time.deltaTime;
        transform.LookAt(target.transform.position);
	}

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject, 0.2f);
    }
}
