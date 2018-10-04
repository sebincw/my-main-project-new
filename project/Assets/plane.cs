using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class plane : MonoBehaviour
{

	public WebCamTexture wcd;
	public Color[] totpixels;
	public GameObject cube, paint, shape, newshape, basemesh, mesh1, mesh2, line3d, line3dinstance, hollow, dm1temp , s1,s2,s3,c1,c2,c3,c4,c5,c6;
	public GameObject[] objs, dm2temp;
	public float count, x1, y1, z1, bpscount, lerpcount, dist;
	public float hc,lc, starttime;
	public int w, h, drawmode, lrcount, colormode, shapemode, filenumber;
	public Vector2 mean;
	public int bptemp,ccc, objindex, dm2tempindex;
	public Vector3 brushpos, prevbrushpos, brushpos1, prevbrushpos1;
	public GameObject cam;
	public string keyname,aaa,currentbutton;
	public Texture2D tx;
	public AndroidJavaObject plugin;
	public byte[] mdata;
	public Renderer renderer1;
	public TextMesh tm;
	public float[] zarray;
	public List<Vector3> points;
	public Vector2[] points2d;
	public int[] indices;
	public LineRenderer lr1;
	public TextMesh b1, b2, b3, b4, b5, b6;
	public Material red,blue,green,trans;
	public bool spawned, drawmode2;

	void Start()
	{
		objs = new GameObject[100];
		dm2temp = new GameObject[30];
		wcd = new WebCamTexture();
		objindex = 0;
		dm2tempindex = 0;
		spawned = false;
		drawmode = 1;
		colormode = 1;
		shapemode = 1;
		filenumber = 1;
		x1 = 1;
		y1 = 1;
		//renderer1 = GetComponent<Renderer>();
		cam = GameObject.Find("Main Camera");
		//tm = GameObject.Find("tm").GetComponent<TextMesh>();
		//tm.text = "aaaaa";

		tx = new Texture2D(640, 480, TextureFormat.RGB565, false);
		plugin = new AndroidJavaObject("com.censwib.unityplugin.pluginclass");
		aaa = plugin.Call<int>("a").ToString();

		paint.GetComponent<Renderer>().material = green;
		line3d.GetComponent<Renderer>().material = green;
		hollow.GetComponent<Renderer>().material = green;

	}


	void Update()
	{


		if ((Input.GetKeyDown(KeyCode.Mouse0)) && (brushpos.y < -60))
		{
			if (16 < brushpos.z && brushpos.z < 24)
			{
				if (drawmode < 3)
				{
					drawmode++;
					b1.text = "DRAWMODE : " + drawmode;
				}
				else
				{
					drawmode = 1;
					b1.text = "DRAWMODE : " + drawmode;
				}
			}

			if (6 < brushpos.z && brushpos.z < 14)
			{

				if (colormode == 1)
				{
					colormode = 2;
					paint.GetComponent<Renderer>().material = red;
					line3d.GetComponent<Renderer>().material = red;
					hollow.GetComponent<Renderer>().material = red;
					b2.text = "COLOR : RED";
				}
				else if (colormode == 2)
				{
					colormode = 3;
					paint.GetComponent<Renderer>().material = blue;
					line3d.GetComponent<Renderer>().material = blue;
					hollow.GetComponent<Renderer>().material = blue;
					b2.text = "COLOR : BLUE";
				}
				else if (colormode == 3)
				{
					colormode = 1;
					paint.GetComponent<Renderer>().material = green;
					line3d.GetComponent<Renderer>().material = green;
					hollow.GetComponent<Renderer>().material = green;
					b2.text = "COLOR : GREEN";
				}

			}

			if (-4 < brushpos.z && brushpos.z < 4)
			{
				if (shapemode == 1)
				{
					shapemode = 2;
					paint.GetComponent<MeshFilter>().sharedMesh = s2.GetComponent<MeshFilter>().sharedMesh;
					b3.text = "SHAPE : SPHERE";
				}
				else if (shapemode == 2)
				{
					shapemode = 3;
					paint.GetComponent<MeshFilter>().sharedMesh = s3.GetComponent<MeshFilter>().sharedMesh;
					b3.text = "SHAPE : CYLINDER";
				}
				else if (shapemode == 3)
				{
					shapemode = 1;
					paint.GetComponent<MeshFilter>().sharedMesh = s1.GetComponent<MeshFilter>().sharedMesh;
					b3.text = "SHAPE : CUBE";
				}
			}

			if (-14 < brushpos.z && brushpos.z < -6)
			{
				if (filenumber == 3)
				{
					filenumber = 1;
					b4.text = "SAVE SLOT : " + filenumber;
				}
				else if (filenumber < 3)
				{
					filenumber++;
					b4.text = "SAVE SLOT : " + filenumber;
				}

			}

			if ((-24 < brushpos.z && brushpos.z < -16) && (brushpos.x < 0))
			{
				GameObject g1 = Instantiate(hollow, Vector3.zero, Quaternion.identity);
				GameObject g2 = Instantiate(hollow, Vector3.zero, Quaternion.identity);
				GameObject g3 = Instantiate(hollow, Vector3.zero, Quaternion.identity);



				for (int i = 1; i <= objindex; i++)
				{
					if (objs[i].GetComponent<Renderer>().material.color == red.color)
					{
						objs[i].transform.parent = g1.transform;
					}
					if (objs[i].GetComponent<Renderer>().material.color == blue.color)
					{
						objs[i].transform.parent = g2.transform;
					}
					if (objs[i].GetComponent<Renderer>().material.color == green.color)
					{
						objs[i].transform.parent = g3.transform;
					}
				}

				combiner(g1, true, 1, filenumber);
				combiner(g2, true, 2, filenumber);
				combiner(g3, true, 3, filenumber);



				objs[1] = g1;
				objs[2] = g2;
				objs[3] = g3;

				objs[1].GetComponent<Renderer>().material = red;
				objs[2].GetComponent<Renderer>().material = blue;
				objs[3].GetComponent<Renderer>().material = green;

				objindex = 3;
			}

			if ((-24 < brushpos.z && brushpos.z < -16) && (brushpos.x > 0))
			{

				for (int i = 1; i < 4; i++)
				{
					objindex++;
					objs[objindex] = Instantiate(hollow, Vector3.zero, Quaternion.identity);
					Mesh holderMesh = new Mesh();
					ObjImporter newMesh = new ObjImporter();
					holderMesh = newMesh.ImportFile(Application.persistentDataPath + "testobject" + i.ToString() + filenumber.ToString() + ".obj");

					MeshRenderer renderer = objs[objindex].GetComponent<MeshRenderer>();
					MeshFilter filter = objs[objindex].GetComponent<MeshFilter>();
					filter.mesh = holderMesh;
					if (i == 1)
						objs[objindex].GetComponent<Renderer>().material = red;
					if (i == 2)
						objs[objindex].GetComponent<Renderer>().material = blue;
					if (i == 3)
						objs[objindex].GetComponent<Renderer>().material = green;
				}
			}

		}



			if ((16 < brushpos.z && brushpos.z < 24)&& (brushpos.y < -60))

			{
			c1.GetComponent<Renderer>().material = green;
			}
			else
			{
				c1.GetComponent<Renderer>().material = trans;
			}

			if ((6 < brushpos.z && brushpos.z < 14) && (brushpos.y < -60))
		{
				c2.GetComponent<Renderer>().material = green;
			}
			else
			{
				c2.GetComponent<Renderer>().material = trans;
			}


			if ((-4 < brushpos.z && brushpos.z < 4) && (brushpos.y < -60))
		{
				c3.GetComponent<Renderer>().material = green;
			}
			else
			{
				c3.GetComponent<Renderer>().material = trans;
			}

			if ((-14 < brushpos.z && brushpos.z < -6) && (brushpos.y < -60))
		{
				c4.GetComponent<Renderer>().material = green;
			}
			else
			{
				c4.GetComponent<Renderer>().material = trans;
			}

			if (((-24 < brushpos.z && brushpos.z < -16) && (brushpos.x < 0)) && (brushpos.y < -60))
		{
				c5.GetComponent<Renderer>().material = green;
			}
			else
			{
				c5.GetComponent<Renderer>().material = trans;
			}

			if (((-24 < brushpos.z && brushpos.z < -16) && (brushpos.x > 0)) && (brushpos.y < -60))
		{
				c6.GetComponent<Renderer>().material = green;
			}
			else
			{
				c6.GetComponent<Renderer>().material = trans;
			}
		




			//detectPressedKeyOrButton();
			//tm.text = z1.ToString() + "//" + brushpos.ToString();

		mdata = plugin.Call<byte[]>("b");
		ccc = plugin.Call<int>("c");

		tx.LoadRawTextureData(mdata);
		tx.Apply();
		//renderer1.material.mainTexture = tx;

		totpixels = tx.GetPixels();
		w = tx.width;
		h = tx.height;




		//pixels 1d array to 2d array & bright pixels

		count = 0;
		mean = new Vector2(0, 0);
		for (int k = 0; k < totpixels.Length; k++)
		{
			if (totpixels[k].r >= 0.9 && totpixels[k].g <= 0.6 && totpixels[k].b <= 0.6)
			{
				mean.x += k % 640;
				mean.y += k / 640;
				count++;
			}
		}
		mean /= count;

		// pixels 1d array to 2d array








		// find centre of all bright pixels

		x1 = mean.x - 320;
		y1 = 240 - mean.y;
		z1 = count;
		Mathf.Clamp(z1, 0, 7000);
		// find centre of all bright pixels




		// default cube mesh is spawn at obtained points
		if (count > 10)
		{
			if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				if (objindex > 0)
				{
					objs[objindex].SetActive(false);
					objindex--;
				}
			}

			brushpos = cam.transform.forward * (80 - Mathf.Clamp(Mathf.Sqrt(z1), 0, 80));
			brushpos += cam.transform.right * x1 / 10;
			brushpos += cam.transform.up * y1 / 10;


			cube.transform.position = cam.transform.position + brushpos;
			cube.transform.rotation = cam.transform.rotation;

			if (drawmode == 1)
			{
				dist = Vector3.Distance(prevbrushpos, brushpos);

				if (Input.GetKeyDown(KeyCode.Joystick1Button0))
				{
					objindex++;
					objs[objindex] = Instantiate(hollow, Vector3.zero, Quaternion.identity);
					//objs[objindex] = Instantiate(line3d, brushpos, Quaternion.identity);
					//lr1 = objs[objindex].GetComponent<LineRenderer>();
					//lr1.positionCount = 0;

				}

				if (Input.GetKey(KeyCode.Joystick1Button0))
				{
					if ((dist > 0.1f))
					{
						//lr1.positionCount++;
						//lr1.SetPosition(lr1.positionCount - 1, brushpos);

						dm1temp = Instantiate(paint, (brushpos + prevbrushpos) / 2, Quaternion.identity);
						dm1temp.transform.LookAt(brushpos);
						dm1temp.transform.localScale = new Vector3(1, 1, dist * 1.1f);
						dm1temp.transform.parent = objs[objindex].transform;
					}
				}

				if (Input.GetKeyUp(KeyCode.Joystick1Button0))
				{
					combiner(objs[objindex],false,1,1);
				}
				prevbrushpos = brushpos;
			}

			if (drawmode == 2)
			{
				if (Input.GetKeyDown(KeyCode.Joystick1Button0))
				{
					points.Add(cube.transform.position);
					dm2tempindex++;
					dm2temp[dm2tempindex] = Instantiate(paint, brushpos, Quaternion.identity);
				}
				if (Input.GetKeyDown(KeyCode.Joystick1Button1))
				{
					points2d = twodgen(points);
					zarray = zgen(points);
					genmesh(points2d);
					points.Clear();
					while (dm2tempindex > 0)
					{
						dm2temp[dm2tempindex].SetActive(false);
						dm2tempindex--;
					}
				}
			}

			if (drawmode == 3)
			{
				if (Input.GetKeyDown(KeyCode.Joystick1Button0))
				{
					brushpos1 = brushpos;
					objindex++;
					objs[objindex] = Instantiate(paint, brushpos, Quaternion.identity);
					spawned = true;
					starttime = Time.time;
				}
				if (Input.GetKey(KeyCode.Joystick1Button0))
				{
					if (spawned)
					{
						prevbrushpos1 = brushpos;
						objs[objindex].transform.localScale = Vector3.one * Vector3.Distance(prevbrushpos1, brushpos1);
						objs[objindex].transform.LookAt(brushpos);
					}
				}
			}
			// default cube mesh is spawn at obtained points

		}

	}


	void genmesh(Vector2[] v2)
	{
		
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(v2);
		indices = tr.Triangulate();

		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[v2.Length];
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = new Vector3(v2[i].x, v2[i].y, zarray[i]);
		}

		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();

		objindex++;
		objs[objindex] = Instantiate(basemesh, Vector3.zero, Quaternion.identity);
		MeshFilter filter1 = objs[objindex].GetComponent<MeshFilter>();
		filter1.mesh = msh;

		Mesh msh1 = new Mesh();
		msh1.vertices = vertices;
		msh1.triangles = rev(indices);
		msh1.RecalculateNormals();
		msh1.RecalculateBounds();

		objindex++;
		objs[objindex] = Instantiate(basemesh, Vector3.zero, Quaternion.identity);
		MeshFilter filter2 = objs[objindex].GetComponent<MeshFilter>();
		filter2.mesh = msh1;
	}


	float[] zgen(List<Vector3> a)
	{
		float[] b = new float[a.Count];

		for (int i = 0; i < a.Count; i++)
		{
			b[i] = a[i].z;
		}

		return b;
	}

	Vector2[] twodgen(List<Vector3> a)
	{
		Vector2[] b = new Vector2[a.Count];

		for (int i = 0; i < a.Count; i++)
		{
			b[i].x = a[i].x;
			b[i].y = a[i].y;
		}

		return b;
	}

	public void detectPressedKeyOrButton()
	{
		foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(kcode))
			{
				//tm.text = "."+kcode.ToString()+".";
			}

		}
	}

	int[] rev(int[] a)
	{
		int[] b = new int[a.Length];

		for (int i = 0; i < a.Length; i++)
		{
			b[i] = a[a.Length - i - 1];
		}
		return b;
	}


	void combiner(GameObject go, Boolean save, int clr, int filenumber)
	{
		// save the parent GO-s pos+rot
		//Vector3 position = go.transform.position;
		//Quaternion rotation = go.transform.rotation;

		// move to the origin for combining
		//transform.position = Vector3.zero;
		//transform.rotation = Quaternion.identity;

		MeshFilter[] filters = go.GetComponentsInChildren<MeshFilter>();
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

		MeshFilter filter = go.GetComponent<MeshFilter>();
		filter.mesh = new Mesh();
		filter.mesh.CombineMeshes(combine.ToArray(), true, true);

		// restore the parent GO-s pos+rot
		//go.transform.position = position;
		//go.transform.rotation = rotation;

		if (save)
		{
			ObjExporter.MeshToFile(filter, Application.persistentDataPath + "testobject" + clr.ToString() + filenumber.ToString() + ".obj");
		}
	}


}


