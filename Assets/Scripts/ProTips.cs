using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProTips : MonoBehaviour {

    [SerializeField]
    private Text textField;

    void OnEnable()
    {
        textField.text = tips[Mathf.FloorToInt(Random.value * tips.Length * 0.99999999f)];
    }

    string[] tips = new string[] {
        "Once you have a controlled trajectory, plan for your next movement and time your turnings",
        "Don't attempt overclosed turns; sometimes it's better to fly-by, turn around, and make a propper approach",
        "You may gain height before a twist to slow a bit and perform a smaller curve; then lose height to regain speed",
        "Try to learn the stars' movement pattern; don't get surprised by their arrival and use them to your advantage!",
        "Taking a look at the destinatary's gender before picking the letter will make the delivery easier",

        "Let's put salt on it if it's too sweet."
    };

}