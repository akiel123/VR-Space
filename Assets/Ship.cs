using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

	float flightHeight = 0.1f;
	float moveVelocity = 0.3f;
	float shipSize = 0.4f;

	public GameObject cosmetic;

	public enum LocationType {Planet, Warp};
	public LocationType locationType = LocationType.Planet;
	public LocationType destinationType;
	public Planet lPlanet;
	public Planet dPlanet;
	public WarpRoute lWarp;
	public WarpRoute dWarp;

	public Vector3 destination;
	public Vector3 pos;

	bool hasDestination = false;

	int count = 0;

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate(){
		move (Time.fixedDeltaTime);
		count++;
		followRandomPath ();
	}

	// Update is called once per frame
	void Update () {
		pos = transform.position;
	}

	void followRandomPath(){
		float velocityModifier = 1;
		if (locationType == LocationType.Warp)
			velocityModifier = lWarp.velocityModifier;


		if ((transform.position - destination).magnitude < 0.2f) {
			Debug.Log ("Reached goal: Destnation type: " + destinationType);
			Vector3 direction;
			switch (destinationType) {
			case LocationType.Planet:
				setLocation (dPlanet.gameObject, LocationType.Planet);
				//hasDestination = false;
				destinationType = LocationType.Warp;
				dWarp = lPlanet.destinations [Random.Range(0, lPlanet.destinations.Count)];
				direction = (dWarp.destination.transform.position - dWarp.location.transform.position).normalized;
				if (locationType == LocationType.Planet)
					destination = lPlanet.transform.position + direction * (lPlanet.size + flightHeight);
				else {
					Debug.Log ("Navigation Error");
					destination = Vector3.zero;
				}
				break;
			case LocationType.Warp:
				setLocation (dWarp.gameObject, LocationType.Warp);
				direction = (dWarp.destination.transform.position - transform.position).normalized;
				destination = dWarp.destination.transform.position - (direction * (dWarp.destination.size + flightHeight));
				destinationType = LocationType.Planet;
				dPlanet = dWarp.destination;
				break;
			default:
				break;
			}
		}
	}
		
		

	

	public void move(float timeStep){
		switch (locationType) {
		case LocationType.Planet:
			transform.position += transform.forward * moveVelocity * timeStep;
			Vector3 direction = (transform.position - lPlanet.transform.position).normalized * (lPlanet.size / 2 + flightHeight + shipSize / 2);
			transform.position = lPlanet.transform.position + direction;

			if (hasDestination) {
				Vector3 norm1 = Vector3.Cross (transform.position - lPlanet.transform.position, transform.position - destination);
				Vector3 norm2 = Vector3.Cross (norm1, transform.position - lPlanet.transform.position);
				transform.rotation = Quaternion.LookRotation (norm2, transform.position - lPlanet.transform.position);
				transform.up = transform.position - lPlanet.transform.position;
			} else {
				Debug.Log ("No destination");
				Vector3 norm1 = Vector3.Cross (transform.position - lPlanet.transform.position, transform.forward);
				Vector3 norm2 = Vector3.Cross (norm1, transform.position - lPlanet.transform.position);
				transform.rotation = Quaternion.LookRotation (norm2, transform.position - lPlanet.transform.position);
				transform.up = transform.position - lPlanet.transform.position;
			}

			break;
		case LocationType.Warp:
			transform.position += transform.forward * moveVelocity * lWarp.velocityModifier * timeStep;
			transform.LookAt (lWarp.destination.transform);
			break;
		default:
			break;
		}


	}

	public void setLocation(GameObject newParent, LocationType type){
		transform.SetParent (newParent.transform);
		updateLocation (type);
	}

	public void updateLocation(LocationType newType){
		switch (locationType) {
		case LocationType.Planet:
			if(lPlanet != null)
				lPlanet.unRegisterShip (this);
			break;
		case LocationType.Warp:
			if(lWarp != null)
				lWarp.unRegisterShip (this);
			break;
		default:
			break;
		}
		locationType = newType;
		switch (locationType) {
		case LocationType.Planet:
			lPlanet = transform.parent.GetComponent<Planet> ();
			lPlanet.registerShip (this);
			Debug.Log ("Set Location, planet");
			break;
		case LocationType.Warp:
			Debug.Log ("Set Location, warp");
			lWarp = transform.parent.GetComponent<WarpRoute> ();
			lWarp.registerShip (this);
			break;
		default:
			break;
		}
	}

	public static Ship CreateShip(GameObject shipPrefab, GameObject parent, LocationType parentType){
		Ship ship = (Instantiate (shipPrefab, parent.transform) as GameObject).GetComponent<Ship> ();
		ship.transform.localPosition = parent.transform.position;
		ship.cosmetic.transform.localScale = Vector3.one * ship.shipSize;

		Vector3 direction;
		switch (parentType) {
		case LocationType.Planet:
			ship.setLocation (parent, LocationType.Planet);
			//hasDestination = false;
			ship.destinationType = LocationType.Warp;
			ship.dWarp = ship.lPlanet.destinations [Random.Range (0, ship.lPlanet.destinations.Count)];
			direction = (ship.dWarp.destination.transform.position - ship.dWarp.location.transform.position).normalized;
			ship.hasDestination = true;
			if (ship.locationType == LocationType.Planet)
				ship.destination = ship.lPlanet.transform.position + direction * (ship.lPlanet.size + ship.flightHeight);
			else {
				Debug.Log ("Navigation Error");
				ship.destination = Vector3.zero;
			}
			break;
		case LocationType.Warp:
			ship.setLocation (ship.dWarp.gameObject, LocationType.Warp);
			direction = (ship.dWarp.destination.transform.position - ship.transform.position).normalized;
			ship.destination = ship.dWarp.destination.transform.position - (direction * (ship.dWarp.destination.size + ship.flightHeight));
			ship.hasDestination = true;
			break;
		default:
			break;
		}
		//ship.transform.up = Vector3.left;
		ship.move (Time.fixedTime);
		return ship;
	}
}
