using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public Transform wallPrefab;
	public Transform passPrefab;
	public Transform floorPrefab;
	public Transform tilePrefab;
	public Transform obsticalPrefab;
	public Transform exitPrefab;
	public Vector2 mapSize;
	public int RoomNumber;
	public float roomSize;
	public bool isRandom;

	public Transform enemyPrefab;
	public Transform enemySPPrefab;
	public Transform wayPoint;
	public int enemyCount;
	public int enemySPCount;

	Doors[,] rooms;
	Transform newRoom;

	public int obsticleCount;
	public int seed;
	System.Random prng;

	void Start() {
		GenerateMap ();
	}
	public void GenerateMap (){
		if (isRandom) {
			prng = new System.Random ();
		}
		else
			prng = new System.Random (seed);
		roomInit ();
	}

	public void roomInit(){
		int totalRoomNumberLine = RoomNumber * 2 - 1;
		rooms=new Doors[totalRoomNumberLine,totalRoomNumberLine];
		for (int x = 0; x < totalRoomNumberLine; x++) {
			for (int y = 0; y < totalRoomNumberLine; y++) {
				rooms[x,y]= new Doors(1,1,1,1);
			}
		}
			
		string holder = "Rooms";

		if (transform.Find (holder)) {
			DestroyImmediate (transform.Find (holder).gameObject);
		}

		newRoom = new GameObject (holder).transform;
		newRoom.parent = transform;

		//gernerate rooms
		DibsTheRoom (RoomNumber - 1, RoomNumber - 1);
		DoorsGenerator (RoomNumber - 1, RoomNumber - 1, prng.Next(0,4), RoomNumber);
		int dx = 0, dy = 0,d=0;
		for (int x = 0; x < totalRoomNumberLine; x++)
			for (int y = 0; y < totalRoomNumberLine; y++)
				if (CheckRoomEx (x, y)) {
					if (rooms [x - 1, y].S == 0)
						rooms [x, y].N = 0;
					if (rooms [x + 1, y].N == 0)
						rooms [x, y].S = 0;
					if (rooms [x, y + 1].W == 0)
						rooms [x, y].E = 0;
					if (rooms [x, y - 1].E == 0)
						rooms [x, y].W = 0;
					if (System.Math.Abs( x-RoomNumber+1) > d) {
						dx = x; dy = y;
						d = System.Math.Abs (x - RoomNumber+1);
					}
					if (System.Math.Abs( y-RoomNumber+1) > d) {
						dx = x; dy = y;
						d = System.Math.Abs (y - RoomNumber+1);
					}
				}

		//assign exit
		bool flag=true;
		int rdm;
		while (flag) {
			rdm = prng.Next (0, 4);
			switch (rdm) {
			case 0:
				if (rooms [dx, dy].N == 1) {
					rooms [dx, dy].N = -1;
					flag = false;
				}
					
				break;
			case 1:
				if (rooms [dx, dy].E == 1){
					rooms [dx, dy].E = -1;
					flag = false;
				}
				break;
			case 2:
				if (rooms [dx, dy].S == 1){
					rooms [dx, dy].S = -1;
					flag = false;
				}
				break;
			case 3:
				if (rooms [dx, dy].W == 1){
					rooms [dx, dy].W = -1;
					flag = false;
				}
				break;
			default:
				break;
			}

		}

		for (int x = 0; x < totalRoomNumberLine; x++)
			for (int y = 0; y < totalRoomNumberLine; y++)
				if (CheckRoomEx (x, y)) {
					CreateRoom (rooms [x, y].N, rooms [x, y].E, rooms [x, y].S, rooms [x, y].W, x, y);
				}
	}

	public void DoorsGenerator(int x, int y,int comingdir,int numbersOfRooms){
		if (numbersOfRooms == 0)
			return;
		if (CheckRoomEx (x + 1, y) && CheckRoomEx (x - 1, y) && CheckRoomEx (x, y + 1) && CheckRoomEx (x, y - 1)) {
			return;
		}
		else {
			int s=0, n=0, e=0, w=0;
			int assignRooms = numbersOfRooms - 1;
			int opsdir = comingdir - 2;
			if (comingdir < 2)
				opsdir = comingdir + 2;
			while (assignRooms > 0) {
				int rdm = prng.Next (0, 7);
				if (rdm == comingdir) {
					continue;
				}
				if (rdm > 3) {
					rdm = opsdir;
				}

				switch (rdm) {
				case 0:
					if (CheckRoomEx (x - 1, y))
						continue;
					n++;
					break;
				case 1:
					if (CheckRoomEx (x, y + 1))
						continue;
					e++;
					break;
				case 2:
					if (CheckRoomEx (x + 1, y))
						continue;
					s++;
					break;
				case 3:
					if (CheckRoomEx (x, y - 1))
						continue;
					w++;
					break;
				default:
					break;
				}
				assignRooms--;

			}

			if (!CheckRoomEx (x + 1, y)) {
				if(s == 0)
					rooms [x, y].S = 1;
				else
					DibsTheRoom (x + 1, y);
			}
			if (!CheckRoomEx (x - 1, y)) {
				if(n == 0)
					rooms [x, y].N = 1;
				else
					DibsTheRoom (x - 1, y);
			}
			if (!CheckRoomEx (x , y + 1)) {
				
				if(e == 0)
					rooms [x, y].E = 1;
				else
					DibsTheRoom (x , y + 1);
			}
			if (!CheckRoomEx (x, y - 1)) {
				if(w == 0)
					rooms [x, y].W = 1;
				else
					DibsTheRoom (x, y - 1);
			}


			DoorsGenerator (x - 1, y, 2, n);
			DoorsGenerator (x, y + 1, 3, e);
			DoorsGenerator (x + 1, y, 0, s);
			DoorsGenerator (x, y - 1, 1, w);

		}
	}

	public bool CheckRoomEx(int x,int y)
	{
		if (rooms [x,y].E==0 || rooms [x,y].W ==0|| rooms [x,y].S ==0|| rooms [x,y].N==0)
			return true;
		return false;
	}

	public void DibsTheRoom(int x,int y)
	{
		rooms [x, y].E = 0;
		rooms [x, y].W = 0;
		rooms [x, y].S = 0;
		rooms [x, y].N = 0;
	}

	public void CreateRoom(int n, int e, int s,int w,int x,int y){
		Vector2 roomPosition = CoordinateToPosition (x, y);


		string roomName = "Room" + x + y;
		GameObject nr = new GameObject (roomName);
		nr.AddComponent <RoomGenerator>();
		RoomGenerator rg = nr.GetComponent ("RoomGenerator")as RoomGenerator;
		rg.east = e;
		rg.west = w;
		rg.south = s;
		rg.north = n;
		rg.wallPrefab = wallPrefab;
		rg.passPrefab = passPrefab;
		rg.floorPrefab = floorPrefab;
		rg.tilePrefab = tilePrefab;
		rg.obsticalPrefab = obsticalPrefab;
		rg.exitPrefab = exitPrefab;
		rg.mapSize = mapSize;
		rg.tileSize = mapSize.x + 4;
		rg.obsticleCount = obsticleCount;
		rg.seed = prng.Next ();
		rg.mapLocation = roomPosition;
		rg.tileOutlinePercent = 1;
		rg.transform.parent = newRoom;
		rg.transform.localScale =Vector3.one;
		rg.enemyPrefab=enemyPrefab;
		rg.enemySPPrefab=enemySPPrefab;
		rg.enemyCount=enemyCount;
		rg.enemySPCount=enemySPCount;
		rg.wayPoint = wayPoint;
	}

	public Vector2 CoordinateToPosition (int x, int y) {
		return new Vector2 (-RoomNumber+1+y,RoomNumber-1-x) * roomSize;
	}


	public struct Doors {
		private int n;
		private int e;
		private int s;
		private int w;
		public int N
		{
			get { return n; }
			set { n = value; }
		}
		public int E
		{
			get { return e; }
			set { e = value; }
		}
		public int S
		{
			get { return s; }
			set { s = value; }
		}
		public int W
		{
			get { return w; }
			set { w = value; }
		}
		public Doors (int _n, int _e,int _s, int _w) {
			n = _n;
			e = _e;
			s = _s;
			w = _w;
		}

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
