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
        "disasters upon your will. These divine instructions can be turned off by clicking the (i) button in the bottom right corner of the menu.\n" +
        "<b><color=red>Trigger</color></b> -> Teleport to laser point\n" +
        "<b><color=red>Grip</color></b> -> Drag to move\n" +
        "<b><color=red>Joystick</color></b> -> Move up/down and rotate";

    private string defaultLeft = "This is the orb of destruction. It can be used to select disasters from the " +
        "sacred wand in your right hand. Use the <b>white</b> tip on the orb to select a disaster from the wands menu\n" +
        "<b><color=red>Y</color></b> -> reset world";

    private string tornadoRight = "Unleash a tornado by pressing <b><color=red>A</color></b>.\n" +
        "Control the tornado's movement by pointing where you want it to go.\n " +
        "Press <b><color=red>A</color></b> again to remove it.";

    private string cometRight = "Point where you want a comet to land. \n Call upon the fury of the cosmos by pressing <color=red><b>A</b></color>.";

    private string magnetRight = "";

    private string magnetLeft = "";

    private string hammerRight = "You just summoned Thor's Hammer, Mjölner!\n" +
        "Press <b><color=red>A</color></b> to activate the thunder.";

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
