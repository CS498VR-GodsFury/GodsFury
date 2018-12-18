using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour {

    public GameObject rightTutorialScreen;
    public GameObject leftTutorialScreen;
    Text rightTutorialScreenText;
    Text leftTutorialScreenText;

    private string defaultRight = "This is the sacred wand that controls the elements of the earth and can unleash " +
        "disasters upon your will. These divine instructions can be turned off by clicking the button to the right in the wand menu.\n" + 
        "Trigger -> Teleport to laser point\n" +
        "Grip -> Drag to move\n" +
        "Joystick -> Move up/down and rotate";

    private string defaultLeft = "This is the orb of destruction. It can be used to select disasters from the " +
        "sacred wand in your right hand. Use the white tip on the orb to select a disaster from the wands menu\n" +
        "Y -> reset world";

    private string tornadoRight = "Unleash a tornado by pressing A.\n" +
        "Control the tornado's movement by pointing where you want it to go.\n " +
        "Press A again to remove it.";

    private string cometRight = "Point where you want a comet to land. \n Call upon the fury of the cosmos by pressing A.";

    private string magnetRight = "";

    private string magnetLeft = "";

    private string hammerRight = "You just summoned Thor's Hammer!\n" +
        "Press A to activate the thunder.";

    private string hammerLeft = "Grip to call back the hammer. \n" +
        "Release to grip to throw the hammer.";


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setTutorialScreensActive(bool toggle)
    {
        rightTutorialScreen.SetActive(toggle);
        leftTutorialScreen.SetActive(toggle);
    }

    public void selectInstructionMessage(string menuState)
    {
        rightTutorialScreenText = rightTutorialScreen.transform.Find("RightTutorialScreenText").GetComponent<Text>();
        leftTutorialScreenText = leftTutorialScreen.transform.Find("LeftTutorialScreenText").GetComponent<Text>();

        rightTutorialScreenText.fontSize = 230;
        leftTutorialScreenText.fontSize = 230;
        setTutorialScreensActive(true);

        if (menuState == "None")
        {
            rightTutorialScreenText.text = defaultRight;
            leftTutorialScreenText.text = defaultLeft;
            rightTutorialScreenText.fontSize = 200;
        }
        else if (menuState == "Tornado")
        {
            rightTutorialScreenText.text = tornadoRight;
            leftTutorialScreen.SetActive(false);
        }
        else if (menuState == "Comet")
        {
            rightTutorialScreenText.text = cometRight;
            leftTutorialScreen.SetActive(false);
        }
        else if (menuState == "Magnet")
        {
            rightTutorialScreenText.text = magnetRight;
            leftTutorialScreenText.text = magnetLeft;
        }
        else if (menuState == "Hammer")
        {
            rightTutorialScreenText.text = hammerRight;
            leftTutorialScreenText.text = hammerLeft;
        }
        

    }


}
