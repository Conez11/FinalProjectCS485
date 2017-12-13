using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour {

	public Transform wallPrefab;
	public Transform passPrefab;
	public Transform floorPrefab;
	public Transform tilePrefab;
	public Transform obsticalPrefab;
	public Transform exitPrefab;
	public Vector2 mapSize;
	public Vector2 mapLocation;
	public float tileSize;

	public Transform wayPoint;
	public Transform enemyPrefab;
	public Transform enemySPPrefab;
	public int enemyCount;
	public int enemySPCount;


	[Range(0,1)]
	public float tileOutlinePercent;

	List<Coord> tileCoordinates;
	Queue<Coord> shuffledTileCoordinates;
	List<Coord> obsticleCorrd;

	public int obsticleCount;
	public int seed;

	[Range(-1,1)]
	public int north;
	[Range(-1,1)]
	public int east;
	[Range(-1,1)]
	public int south;
	[Range(-1,1)]
	public int west;


	void Start () {
		GenerateRoom();
	}

	public void GenerateRoom() {
		
		tileCoordinates = new List<Coord> ();
		obsticleCorrd = new List<Coord> ();

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
		if(north==-1){
			North = exitPrefab;
		}
		Vector3 nv = new Vector3 (0+mapLocation.x * tileSize, 1f, (tileSize/2 - 0.5f)*tileSize+mapLocation.y * tileSize);
		Transform northWall = Instantiate (North, nv, Quaternion.identity) as Transform;
		northWall.parent = obsticalHolder;
		northWall.localScale = Vector3.one*tileSize;

		Transform East=wallPrefab;
		if (east == 0) {
			East = passPrefab;
		}
		if(east==-1){
			East = exitPrefab;
		}
		Vector3 ev = new Vector3 ((tileSize/2 - 0.5f)*tileSize+mapLocation.x * tileSize, 1f, 0+mapLocation.y * tileSize);
		Transform eastWall = Instantiate (East, ev, Quaternion.Euler (Vector3.up * 90)) as Transform;
		eastWall.parent = obsticalHolder;
		eastWall.localScale = Vector3.one*tileSize;

		Transform South=wallPrefab;
		if (south == 0) {
			South = passPrefab;
		}
		if(south==-1){
			North = exitPrefab;
		}
		Vector3 sv = new Vector3 (0+mapLocation.x * tileSize, 1f, -(tileSize/2 - 0.5f)*tileSize+mapLocation.y * tileSize);
		Transform southWall = Instantiate (South, sv, Quaternion.identity) as Transform;
		southWall.parent = obsticalHolder;
		southWall.localScale = Vector3.one*tileSize;


		Transform West=wallPrefab;
		if (west == 0) {
			West = passPrefab;
		}
		if(west==-1){
			West = exitPrefab;
		}
		Vector3 wv = new Vector3 (-(tileSize/2 - 0.5f)*tileSize+mapLocation.x * tileSize, 1f, 0+mapLocation.y * tileSize);
		Transform westWall = Instantiate (West, wv, Quaternion.Euler (Vector3.up * 90)) as Transform;
		westWall.parent = obsticalHolder;
		westWall.localScale = Vector3.one*tileSize;

		Vector3 fl = new Vector3 (mapLocation.x * tileSize, 1f, mapLocation.y * tileSize);
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
			obsticleCorrd.Add (randomCoordinate);
			Vector3 obsticlePosition = CoordinateToPosition (randomCoordinate.x, randomCoordinate.y);
			Transform newObsticle = Instantiate (obsticalPrefab, obsticlePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;	
			newObsticle.parent = obsticalHolder;
			newObsticle.localScale = Vector3.one * tileSize;	

		}


		//generate enemies
		EnemyGeneration();


	}


	public void EnemyGeneration()
	{
		int x, y;
		System.Random prng = new System.Random (seed);
		string holder = "Enemies";

		if (transform.Find (holder)) {
			DestroyImmediate (transform.Find (holder).gameObject);
		}

		Transform enemyHolder = new GameObject (holder).transform;
		enemyHolder.parent = transform;


		for (int i = 0; i < enemyCount; i++) {
			int r = prng.Next (0, obsticleCorrd.Count);
			x=obsticleCorrd.ToArray () [r].x-1+prng.Next(0,3);
			y=obsticleCorrd.ToArray () [r].y-1+prng.Next(0,3);
			if (isObstical (new Coord (x, y))) {
				i--;
				continue;
			}
			Vector3 pos = CoordinateToPosition (x, y);
			obsticleCorrd.Add (new Coord (x, y));
			Transform enemy = Instantiate (enemyPrefab, pos, Quaternion.identity)as Transform;
			enemy.parent = enemyHolder;

			EnemyPathGeneration (x, y, enemy);
		}

		for (int i = 0; i < enemySPCount; i++) {
			int r = prng.Next (0, obsticleCorrd.Count);
			x=obsticleCorrd.ToArray () [r].x-1+prng.Next(0,3);
			y=obsticleCorrd.ToArray () [r].y-1+prng.Next(0,3);
			if (isObstical (new Coord (x, y))) {
				i--;
				continue;
			}
			Vector3 pos = CoordinateToPosition (x, y);
			obsticleCorrd.Add (new Coord (x, y));
			Transform enemy = Instantiate (enemySPPrefab, pos, Quaternion.identity)as Transform;
			enemy.parent = enemyHolder;
			EnemyPathGeneration (x, y, enemy);

		}
	}

	public void EnemyPathGeneration(int x, int y, Transform Enemy)
	{
		string holder = "Path";

		Transform pathHolder = Enemy.Find(holder).transform;

		Vector3 pos = CoordinateToPosition (x, y);
		Transform waypoint = Instantiate (wayPoint, pos, Quaternion.identity)as Transform;
		waypoint.parent = pathHolder;


		Vector3 pos1 = CoordinateToPosition (x, y-1);
		Transform waypoint1 = Instantiate (wayPoint, pos1, Quaternion.identity)as Transform;
		waypoint1.parent = pathHolder;


		Vector3 pos2 = CoordinateToPosition (x+1, y-1);
		Transform waypoint2 = Instantiate (wayPoint, pos2, Quaternion.identity)as Transform;
		waypoint2.parent = pathHolder;

		Vector3 pos3 = CoordinateToPosition (x+1, y);
		Transform waypoint3 = Instantiate (wayPoint, pos3, Quaternion.identity)as Transform;
		waypoint3.parent = pathHolder;

		Vector3 pos4 = CoordinateToPosition (x, y);
		Transform waypoint4 = Instantiate (wayPoint, pos4, Quaternion.identity)as Transform;
		waypoint4.parent = pathHolder;
	}

	public bool isObstical(Coord c)
	{
		return obsticleCorrd.Contains (c);
	}


	public Vector3 CoordinateToPosition (int x, int y) {
		return new Vector3 (-mapSize.x / 2 + 0.5f + x+mapLocation.x, 0.5f, -mapSize.y / 2 + 0.5f + y+mapLocation.y) * tileSize;
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
