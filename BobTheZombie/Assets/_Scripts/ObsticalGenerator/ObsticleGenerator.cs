using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleGenerator : MonoBehaviour {

	public Transform tilePrefab;
	public Transform obsticalPrefab;
	public Vector2 mapSize;
	public float tileSize;

	[Range(0,1)]
	public float tileOutlinePercent;

	List<Coord> tileCoordinates;
	Queue<Coord> shuffledTileCoordinates;

	public int obsticleCount;
	public int seed;

	void Start() {
		GenerateObsticles ();
	}

	public void GenerateObsticles() {

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
		return new Vector3 (-mapSize.x / 2 + 0.5f + x, 0f, -mapSize.y / 2 + 0.5f + y) * tileSize;
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
