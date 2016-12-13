using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour {

    [SerializeField]
    Image img;
	
	// Update is called once per frame
	void Update () {
        img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - Time.deltaTime);
        if (img.color.a <= 0)
            Destroy(gameObject);
	}
}
