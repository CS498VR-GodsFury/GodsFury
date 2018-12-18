using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeftHandSelector : MonoBehaviour {

    // Use this for initialization
    public GameObject GameplayMenu;
    public GameObject IngamePanel;
    private bool isMenuActive;
    private bool isPanelActive;
    private float timer;
    private int frameCounter;
    public string selectedWeapon;
    string[] weaponNames;
    void Start()
    {
        timer = 0f;
        frameCounter = 0;
        isMenuActive = false;
        isPanelActive = false;
        GameplayMenu.SetActive(false);
        IngamePanel.SetActive(false);

        var curRotation = this.transform.localRotation;
        curRotation.eulerAngles = new Vector3(135, 230, 180);
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = curRotation;

        
        selectedWeapon = "None";
        //weaponNames = new string[3];
        weaponNames = new[]{ "Tornado", "Comet", "Magnet", "Hammer" };
    }

    private void OnCollisionEnter(Collision collision)
    {
        var name = collision.collider.name;
        if (isPanelActive)
            GameObject.Find("Weapon").GetComponent<Text>().text = selectedWeapon;
        if (name == "Menu")
        {
            isMenuActive ^= true;
            GameplayMenu.SetActive(isMenuActive);
        }
        else if(Array.IndexOf(weaponNames, name)!=-1)
        {
            if (name == selectedWeapon)
            {
                GameObject.Find(selectedWeapon).transform.localScale /= 1.35f;
                GameObject.Find(selectedWeapon + "BG").GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
                selectedWeapon = "None";
                
                return;
            }
            else if (selectedWeapon != "None")
            {
                GameObject.Find(selectedWeapon).transform.localScale /= 1.35f;
                GameObject.Find(selectedWeapon + "BG").GetComponent<RawImage>().color = new Color(255, 255, 255, 0);
            }
                
            selectedWeapon = name;
            //if(isPanelActive)
            //GameObject.Find("Weapon").GetComponent<Text>().text = name;
            if (selectedWeapon != "None")
            {
                GameObject.Find(selectedWeapon).transform.localScale *= 1.35f;
            }
        }
        else if(name=="Quit")
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
        else if (name == "IngamePanelControl")
        {
            isPanelActive ^= true;
            IngamePanel.SetActive(isPanelActive);
        }

    }
    // Update is called once per frame
    void Update () {
        var deltaTime = Time.deltaTime;
        timer += deltaTime;
        frameCounter += 1;
        if(selectedWeapon!="None")
            GameObject.Find(selectedWeapon + "BG").GetComponent<RawImage>().color = new Color(255, 167, 0, 0.5f);
        if (timer > 0.3f)
        {
            timer = 0;
            if (isPanelActive)
                GameObject.Find("FPS").GetComponent<Text>().text = ((int)(frameCounter / 0.3f)).ToString();
            frameCounter = 0;
        }
	}
}
