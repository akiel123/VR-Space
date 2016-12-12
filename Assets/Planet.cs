using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

	public float size = 1;
	private float reach{ get { return size * reachModifier; } }
	public float reachModifier = 10;
	public Constellation constellation;
	public List<WarpRoute> destinations = new List<WarpRoute> ();
	public List<Ship> ships = new List<Ship>();

	public GameObject cosmetic;

	public GameObject warpRoutePrefab;
	public GameObject shipPrefab;

	bool hasSpawned = false;


	// Use this for initialization
	void Start () {
		findDestinations ();
		cosmetic.transform.localScale = Vector3.one * size;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasSpawned) {
			hasSpawned = true;
			spawnShip ();
		}
	}

	public void findDestinations(){
		foreach (Planet p in constellation.planets) {
			float d = (transform.position - p.transform.position).magnitude;
			if(d < reach){
				destinations.Add(WarpRoute.CreateWarpRoute (p, this,warpRoutePrefab));
			}
		}
	}

	public void spawnShip(){
		Ship ship = Ship.CreateShip (shipPrefab, gameObject, Ship.LocationType.Planet);
	}

	public void registerShip(Ship s){
		ships.Add (s);
	}
	public void unRegisterShip(Ship s){
		ships.Remove (s);
	}
}
