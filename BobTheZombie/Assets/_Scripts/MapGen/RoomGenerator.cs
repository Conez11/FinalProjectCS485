using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour {

	public Transform wallPrefab;
	public Transform passPrefab;
	public Transform floorPrefab;
	public Transform tilePrefab;
	public Transform obsticalPrefab;
	public Vector2 mapSize;
	public Vector2 mapLocation;
	public float tileSize;

	[Range(0,1)]
	public float tileOutlinePercent;

	List<Coord> tileCoordinates;
	Queue<Coord> shuffledTileCoordinates;

	public int obsticleCount;
	public int seed;

	[Range(0,1)]
	public int north;
	[Range(0,1)]
	public int east;
	[Range(0,1)]
	public int south;
	[Range(0,1)]
	public int west;


	void Start () {
		GenerateRoom();
	}

	public void GenerateRoom() {
		
		tileCoordinates = new List<Coord> ();

		for (int x = 0; x < mapSize.x; x++) {
			for (int y = 0; y < mapSize.y; y++) {
				tileCoordinates.Add (new Coord (x, y));
			}
		}

		shuffledTileCoordinates = new Queue<Coord> (Utility.RandomlyShuffledArray (tileCoordinates.ToArray (), seed));

		string holder = "Generated";

		if (transform.Find (holder)) {
			DestroyImmediate (transform.Find (holder).gameObject);
		}

		Transform obsticalHolder = new GameObject (holder).transform;
		obsticalHolder.parent = transform;


		Transform North=wallPrefab;
		if (north == 0) {
			North = passPrefab;
		}
		Vector3 nv = new Vector3 (0+mapLocation.x * tileSize, 1f, (tileSize/2 - 0.5f)*tileSize+mapLocation.y * tileSize);
		Transform northWall = Instantiate (North, nv, Quaternion.identity) as Transform;
		northWall.parent = obsticalHolder;
		northWall.localScale = Vector3.one*tileSize;

		Transform East=wallPrefab;
		if (east == 0) {
			East = passPrefab;
		}
		Vector3 ev = new Vector3 ((tileSize/2 - 0.5f)*tileSize+mapLocation.x * tileSize, 1f, 0+mapLocation.y * tileSize);
		Transform eastWall = Instantiate (East, ev, Quaternion.Euler (Vector3.up * 90)) as Transform;
		eastWall.parent = obsticalHolder;
		eastWall.localScale = Vector3.one*tileSize;

		Transform South=wallPrefab;
		if (south == 0) {
			South = passPrefab;
		}
		Vector3 sv = new Vector3 (0+mapLocation.x * tileSize, 1f, -(tileSize/2 - 0.5f)*tileSize+mapLocation.y * tileSize);
		Transform southWall = Instantiate (South, sv, Quaternion.identity) as Transform;
		southWall.parent = obsticalHolder;
		southWall.localScale = Vector3.one*tileSize;


		Transform West=wallPrefab;
		if (west == 0) {
			West = passPrefab;
		}
		Vector3 wv = new Vector3 (-(tileSize/2 - 0.5f)*tileSize+mapLocation.x * tileSize, 1f, 0+mapLocation.y * tileSize);
		Transform westWall = Instantiate (West, wv, Quaternion.Euler (Vector3.up * 90)) as Transform;
		westWall.parent = obsticalHolder;
		westWall.localScale = Vector3.one*tileSize;

		Vector3 fl = new Vector3 (mapLocation.x * tileSize, 0f, mapLocation.y * tileSize);
		Transform floor = Instantiate (floorPrefab, fl, Quaternion.identity) as Transform;
		floor.parent = obsticalHolder;
		floor.localScale = Vector3.one*tileSize;

		for (int x = 0; x < mapSize.x; x++) {
			for (int y = 0; y < mapSize.y; y++) {
				Vector3 tilePosition = CoordinateToPosition (x, y);
				Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.Euler (Vector3.right * 90)) as Transform;
				newTile.localScale = Vector3.one * (1 - tileOutlinePercent) * tileSize;	
				newTile.parent = obsticalHolder;

			}
		}

		for (int i = 0; i < obsticleCount; i++) {
			Coord randomCoordinate = GetRandomCoordinate ();
			Vector3 obsticlePosition = CoordinateToPosition (randomCoordinate.x, randomCoordinate.y);
			Transform newObsticle = Instantiate (obsticalPrefab, obsticlePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;	
			newObsticle.parent = obsticalHolder;
			newObsticle.localScale = Vector3.one * tileSize;	

		}
	}

	public Vector3 CoordinateToPosition (int x, int y) {
		return new Vector3 (-mapSize.x / 2 + 0.5f + x+mapLocation.x, 0f, -mapSize.y / 2 + 0.5f + y+mapLocation.y) * tileSize;
	}

	public Coord GetRandomCoordinate (){
		Coord tempCoord = shuffledTileCoordinates.Dequeue ();
		shuffledTileCoordinates.Enqueue (tempCoord);
		return tempCoord;
	}

	public struct Coord {
		public int x;
		public int y;

		public Coord (int _x, int _y) {
			x = _x;
			y = _y;

		}
	}

}
