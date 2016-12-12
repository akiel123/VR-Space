using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpRoute : MonoBehaviour {

	public LineRenderer lr;

	public float velocityModifier = 1;
	public float routeLength;

	public Planet destination;
	public Planet location;

	public List<Ship> ships = new List<Ship>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static WarpRoute CreateWarpRoute(Planet destination, Planet location, GameObject prefab){
		WarpRoute route = (Instantiate (prefab, location.transform) as GameObject).GetComponent<WarpRoute>();
		route.location = location;
		route.destination = destination;
		route.lr.SetPosition (0, route.location.transform.position);
		route.lr.SetPosition (1, route.destination.transform.position);
		//route.lr.enabled = false;
		route.routeLength = (route.location.transform.position - route.destination.transform.position).magnitude - route.location.size / 2 - route.destination.size / 2;
		return route;
	}


	public void registerShip(Ship s){
		ships.Add (s);
	}
	public void unRegisterShip(Ship s){
		ships.Remove (s);
	}
}
