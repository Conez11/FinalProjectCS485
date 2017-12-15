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
		nextStepGen ();
		int x, y;
		Coord c;
		enemyrespwaned = new List<Coord> ();
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
			c = new Coord (x, y);
			if (isObstical (c)||enemyrespwaned.Contains(c)) {
				i--;
				continue;
			}
			enemyrespwaned.Add (c);
			Vector3 pos = CoordinateToPositionEnemy (x, y);
			//obsticleCorrd.Add (new Coord (x, y));
			Transform enemy = Instantiate (enemyPrefab, pos, Quaternion.identity)as Transform;
			enemy.parent = enemyHolder;

			if (prng.Next (0, 2)==0) {
				EnemyPathGeneration (x, y, enemy,obsticleCorrd.ToArray () [r],true);
			}
			else
				EnemyPathGeneration (x, y, enemy,obsticleCorrd.ToArray () [r],false);
		}

		for (int i = 0; i < enemySPCount; i++) {
			int r = prng.Next (0, obsticleCorrd.Count);
			x=obsticleCorrd.ToArray () [r].x-1+prng.Next(0,3);
			y=obsticleCorrd.ToArray () [r].y-1+prng.Next(0,3);
			c = new Coord (x, y);
			if (isObstical (c)||enemyrespwaned.Contains(c)) {
				i--;
				continue;
			}
			enemyrespwaned.Add (c);
			Vector3 pos = CoordinateToPositionEnemy (x, y);
			//obsticleCorrd.Add (new Coord (x, y));
			Transform enemy = Instantiate (enemySPPrefab, pos, Quaternion.identity)as Transform;
			enemy.parent = enemyHolder;

			if (prng.Next (0, 2)==0) {
				EnemyPathGeneration (x, y, enemy,obsticleCorrd.ToArray () [r],true);
			}
			else
				EnemyPathGeneration (x, y, enemy,obsticleCorrd.ToArray () [r],false);

		}
	}
	Transform pathHolder;
	List<Coord> pathCoord;
	List<Coord> pathObstial;
	List<Coord> enemyrespwaned;
	public void EnemyPathGeneration(int x, int y, Transform Enemy,Coord c,bool clockwise)
	{
		string holder = "Path";
		pathHolder = Enemy.Find(holder).transform;
		pathCoord = new List<Coord> ();
		pathObstial = new List<Coord> ();
		//Coord current = new Coord (x, y);
		//addWayPoint(current);

		//Coord finish=nextStep(!clockwise,current,c);
		//pathStepGen (current, c, finish, clockwise);
		pathGen(new Coord(x,y),c,clockwise);

		foreach(Coord p in pathCoord)
		{
			addWayPoint(p);
		}
	}

	void pathGen(Coord current,Coord cube,bool clockwise){
		pathObstial.Add (cube);
		List<Coord> currentpath = new List<Coord> (); 
		Coord finish = current;
		 do{
			if (!(pathObstial.Contains (current)||pathCoord.Contains(current))){
				pathCoord.Add (current);
				currentpath.Add(current);
			}
			foreach (Coord c in currentpath) {
				if (isObstical (c)) {
					pathCoord.Remove (c);
					pathObstial.Add (c);
					pathGen ( nextStepForce (!clockwise, c, cube),c, clockwise);
				}
			}
			current = nextStepForce (clockwise, current, cube);
		}while (!finish.Equals (current));


	}

	/*
	public void pathStepGen(Coord current,Coord c,Coord finish,bool clockwise){
		//priority, go straight, check around, (0 path not being stepped, 1 path stepped, -1 can't walk,)


		//stuck
		if (current.Equals (finish))
			return;
		int i = 0;
		while(!finish.Equals(current)&&i<15){
			addWayPoint(current);
			current=nextStep(clockwise,current,c);
				i++;
		}

	}


	public Coord nextStep(bool clockwise, Coord currentPos, Coord cube){
		if (isSurround (currentPos)) {
			return currentPos;
		}
		Coord f = new Coord (currentPos.x - cube.x, currentPos.y - cube.y);
		if(clockwise)
			f=ns.ToArray()[(ns.IndexOf (f)+1)%ns.Count];
		else
			f=ns.ToArray()[(ns.IndexOf (f)-1+ns.Count)%ns.Count];
		f.x += cube.x;
		f.y += cube.y;
		if (isObstical (f)) {
			//double recursion
			pathStepGen(currentPos,f,nextStepForce(clockwise,f,cube),clockwise);
			return nextStepForce(clockwise,f,cube);
		}
		return f;
	}
	*/
	public Coord nextStepForce(bool clockwise, Coord currentPos, Coord cube){
		
		Coord f = new Coord (currentPos.x - cube.x, currentPos.y - cube.y);

		if(clockwise)
			f=ns.ToArray()[(ns.IndexOf (f)+1)%ns.Count];
		else
			f=ns.ToArray()[(ns.IndexOf (f)-1+ns.Count)%ns.Count];
		f.x += cube.x;
		f.y += cube.y;
		return f;
	}
	private List<Coord> ns;
	public void nextStepGen(){
		ns = new List<Coord> ();
		ns.Add (new Coord (-1,-1));
		ns.Add (new Coord (-1,0));
		ns.Add (new Coord (-1,1));
		ns.Add (new Coord (0,1));
		ns.Add (new Coord (1,1));
		ns.Add (new Coord (1,0));
		ns.Add (new Coord (1,-1));
		ns.Add (new Coord (0,-1));
	}

	public void addWayPoint(Coord c){
		
		Vector3 pos = CoordinateToPositionEnemy (c.x, c.y);
		Transform waypoint = Instantiate (wayPoint, pos, Quaternion.identity)as Transform;
		waypoint.parent = pathHolder;
	}

	public bool isSurround(Coord c){
		return (obsticleCorrd.Contains (new Coord(c.x-1,c.y))&&obsticleCorrd.Contains (new Coord(c.x,c.y-1))&&obsticleCorrd.Contains (new Coord(c.x+1,c.y))&&obsticleCorrd.Contains (new Coord(c.x,c.y+1)));
	}

	public bool isObstical(Coord c)
	{
		return obsticleCorrd.Contains (c);
	}


	public Vector3 CoordinateToPosition (int x, int y) {
		return new Vector3 (-mapSize.x / 2 + 0.5f + x+mapLocation.x, 0f, -mapSize.y / 2 + 0.5f + y+mapLocation.y) * tileSize;
	}

	public Vector3 CoordinateToPositionEnemy (int x, int y) {
		return new Vector3 (-mapSize.x / 2 + 0.5f + x+mapLocation.x, 0.1f, -mapSize.y / 2 + 0.5f + y+mapLocation.y) * tileSize;
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
