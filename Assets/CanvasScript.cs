using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetButtonFalse(Button button) {
        button.interactable = false;
    }

    public void SetButtonTrue(Button button)
    {
        button.interactable = true;
    }


}
