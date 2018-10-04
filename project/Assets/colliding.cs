using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliding : MonoBehaviour {

	
	void Awake ()
	{
		StartCoroutine(cols());
	}


	IEnumerator cols()
	{
		yield return new WaitForSeconds(4);
		gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
	}
}
