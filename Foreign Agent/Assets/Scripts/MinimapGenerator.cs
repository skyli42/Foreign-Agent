using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapGenerator : MonoBehaviour
{
	public GameObject wallParent;
	public Material mat;
	// Start is called before the first frame update
    void Start()
    {
		for (int i = 0; i < wallParent.transform.childCount; i++)
		{
			GameObject wall = Instantiate(wallParent.transform.GetChild(i).gameObject);
			wall.layer = 12;
			wall.GetComponent<Renderer>().material = mat;
			MeshFilter mF = wall.GetComponent<MeshFilter>();
			Mesh mesh = mF.mesh;
			LineRenderer lRender = wall.AddComponent<LineRenderer>();
			lRender.material = new Material(Shader.Find("Standard"));
			lRender.material.color = new Color(0.255f, 1.0f, 0.0f);
			SortedSet<float> yvals = new SortedSet<float>();
			foreach(Vector3 point in mesh.vertices)
			{
				Vector3 truePos = wall.transform.TransformPoint(point);
				yvals.Add(truePos.y);
			}
			List<Vector3> points = new List<Vector3>();
			foreach(Vector3 vertex in mesh.vertices)
			{
				Vector3 truePos = wall.transform.TransformPoint(vertex);

				if (truePos.y == yvals.Max) {
					truePos.y += 0.5f;
					points.Add(truePos);
				}
			}
			lRender.startWidth = 0.1f;
			lRender.endWidth = 0.1f;
			lRender.positionCount = points.Count;
			lRender.SetPositions(points.ToArray());
			lRender.startColor = new Color(0.255f, 1.0f, 0.0f);
			lRender.endColor = new Color(0.255f, 1.0f, 0.0f);
		}

	}

}
