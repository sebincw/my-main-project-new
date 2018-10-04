using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class erased : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col)
	{
		if (Input.GetKeyDown(KeyCode.Mouse1))
			Destroy(col.gameObject);
	}
}
