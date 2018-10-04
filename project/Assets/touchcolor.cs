using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchcolor : MonoBehaviour {

	public Material sel, trans;


	void Start ()
	{
		gameObject.GetComponent<Renderer>().material = sel;
	}
	

	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		gameObject.GetComponent<Renderer>().material = sel;
	}

	void OnCollisionExit(Collision collision)
	{
		gameObject.GetComponent<Renderer>().material = trans;
	}
}
