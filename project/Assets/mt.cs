using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class mt : MonoBehaviour {


	public string destination;
	public GameObject g1;


	// Use this for initialization
	void Start () {

	}

	void Awake()
	{
		// save the parent GO-s pos+rot
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;

		// move to the origin for combining
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;

		MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
		List<CombineInstance> combine = new List<CombineInstance>();

		for (int i = 0; i < filters.Length; i++)
		{
			// skip the empty parent GO
			if (filters[i].sharedMesh == null)
				continue;

			// combine submeshes
			for (int j = 0; j < filters[i].sharedMesh.subMeshCount; j++)
			{
				CombineInstance ci = new CombineInstance();

				ci.mesh = filters[i].sharedMesh;
				ci.subMeshIndex = j;
				ci.transform = filters[i].transform.localToWorldMatrix;

				combine.Add(ci);
			}

			// disable child mesh GO-s
			filters[i].gameObject.SetActive(false);
		}

		MeshFilter filter = GetComponent<MeshFilter>();
		filter.mesh = new Mesh();
		filter.mesh.CombineMeshes(combine.ToArray(), true, true);

		// restore the parent GO-s pos+rot
		transform.position = position;
		transform.rotation = rotation;

		ObjExporter.MeshToFile(filter, Application.persistentDataPath + "testobj.obj");



		Mesh holderMesh = new Mesh();
		ObjImporter newMesh = new ObjImporter();
		holderMesh = newMesh.ImportFile(Application.persistentDataPath + "testobj.obj");

		MeshRenderer renderer = g1.GetComponent<MeshRenderer>();
		MeshFilter filter1 = g1.GetComponent<MeshFilter>();
		filter1.mesh = holderMesh;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
