using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorSphere : MonoBehaviour {

	public GameObject camera;
	public float size = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.one * (camera.transform.position - transform.position).magnitude * size;
	}
}
