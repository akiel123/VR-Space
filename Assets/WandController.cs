using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {

	public GameObject cameraRig;
	public GameObject indicator;

	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	public bool gripButtonDown = false;
	public bool gripButtonUp = false;
	public bool gripButtonPressed = false;

	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	public bool triggerButtonDown = false;
	public bool triggerButtonUp = false;
	public bool triggerButtonPressed = false;

	private SteamVR_Controller.Device c { get { return SteamVR_Controller.Input ((int)trackedObj.index); } }
	private SteamVR_TrackedObject trackedObj;

	bool draggingSpace = false;

	public Vector3 lastPos;

	public float dragSpeed = 2;
	public float dragDrag = 0.9f;
	private Vector3 dragVelocity;


	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!draggingSpace)
			lastPos = transform.position;
		ButtonDetection();
		if(draggingSpace) 
			lastPos = transform.position;

		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) {
			indicator.transform.position = hit.point;
		}

	}

	void FixedUpdate(){
		if (!(c.GetPress (Valve.VR.EVRButtonId.k_EButton_Grip))) {
			draggingSpace = false;
			dragVelocity *= (dragDrag);
		}


	}

	void ButtonDetection(){
		if (c == null) {
			return;
		}



		//Triggering
		if (c.GetPress(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			
		}

		//Dragging
		if (c.GetPress (Valve.VR.EVRButtonId.k_EButton_Grip)) {
			draggingSpace = true;
			dragVelocity = (lastPos - transform.position) * dragSpeed / Time.deltaTime;
		} 
		cameraRig.transform.position += dragVelocity * Time.deltaTime;
	}
		
}
