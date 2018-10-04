using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class touch : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
		VRSettings.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.touchCount > 1)
		{
			VRSettings.enabled = true;
			gameObject.SetActive(false);
		}
	}
}
