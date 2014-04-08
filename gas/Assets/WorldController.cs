using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public GameObject tilebase;
	public GameObject[][] world;
	private Ray ray;
	private RaycastHit hit;


	// Use this for initialization
	void Start () {
		Random.seed = (int)System.DateTime.Now.Ticks;
		world = new GameObject[32][];
		for (int i = 0; i < 32; i++)
		{
			world[i] = new GameObject[32];
			for (int j = 0; j < 32; j++)
			{
				world[i][j] = (GameObject) Instantiate(tilebase);
				world[i][j].GetComponent<TileBehaviour>().setController(this);
				world[i][j].GetComponent<TileBehaviour>().setcoords(i, j);
				world[i][j].GetComponent<TileBehaviour>().setdraw(true);
				world[i][j].GetComponent<TileBehaviour>().tiletype = TileBehaviour.TileType.Empty;
			}
		}

		for (int i = 10; i < 14; i++)
		{
			for (int j = 10; j < 14; j++)
			{
				world[i][j].GetComponent<TileBehaviour>().setgas(1000.0f);
			}
		}

		for (int i = 9; i < 15; i++)
		{
			world[i][9].GetComponent<TileBehaviour>().setsolid();
			world[i][14].GetComponent<TileBehaviour>().setsolid();
		}
		for (int j = 9; j < 15; j++)
		{
			world[9][j].GetComponent<TileBehaviour>().setsolid();
			world[14][j].GetComponent<TileBehaviour>().setsolid();
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				GameObject g = world[i][j];

				if (g.GetComponent<TileBehaviour>().tiletype == TileBehaviour.TileType.Solid &&
				    g.GetComponent<TileBehaviour>().open == false)
					continue;
				if (g.GetComponent<TileBehaviour>().gas > 10.0f)
				{
					g.GetComponent<TileBehaviour>().repressurize();
				}
			}
		}

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 1000.0f) && Input.GetMouseButtonDown(0))
		{
			TileBehaviour t = hit.collider.gameObject.GetComponent<TileBehaviour>();
			if (t != null)
			{
				if (t.tiletype == TileBehaviour.TileType.Solid)
				{
					t.toggleOpen();
				}
			}
		}
	}

	public GameObject getNeighbor(int x, int y)
	{
		if (x < 0 || y < 0 || x > 31 || y > 31)
			return null;
		return world[x][y];
	}

	public int getrandom(int a, int b)
	{
		return Random.Range (a, b);
	}
}
