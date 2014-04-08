using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileBehaviour : MonoBehaviour {
	
    public float gas = 0.0f;
	private WorldController worldcontroller;
	private int coord_x = 0;
	private int coord_y = 0;
	bool draw = false;
	private float transferPerTick = 50.0f;
	public enum TileType { Empty, Solid };
	public TileType tiletype;
	public bool open = false;

	public float interpolate(float a, float b, float c)
	{
		return a * (1.0f - c) + c * b;
	}

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		if (draw == true)
			gameObject.renderer.enabled = true;
	}

	public void setController(WorldController c)
	{
		if (c == null) throw new UnityException();
		worldcontroller = c;
	}

	public void setcoords(int x, int y)
	{
		coord_x = x;
		coord_y = y;
		transform.position = new Vector3(x*15, 0, y*15);
	}

	public void setdraw(bool b)
	{
		gameObject.renderer.enabled = b;
		draw = b;
		Color c = renderer.material.color;
		c.r = gas/100.0f;
		c.g = gas/100.0f;
		c.b = gas/100.0f;
		c.a = 1.0f;
		gameObject.renderer.material.color = c;
	}

	public void setgas(float g)
	{
		gas = g;
	}

	public void repressurize()
	{
		if (tiletype == TileType.Solid && open == false)
			return;
		if (worldcontroller == null)
		{
			Debug.Log ("Worldcontroller Empty");
		}
	
		List<GameObject> dirs = new List<GameObject>();
		if (coord_x > 0)
		{
			GameObject t1 = worldcontroller.getNeighbor(coord_x-1, coord_y);
			if (t1.GetComponent<TileBehaviour>().tiletype == TileType.Empty ||
			    t1.GetComponent<TileBehaviour>().open == true &&
			    t1.GetComponent<TileBehaviour>().gas < this.gas)
				dirs.Add(t1);
		}
		if (coord_x < 31)
		{
			GameObject t2 = worldcontroller.getNeighbor(coord_x+1, coord_y);
			if (t2.GetComponent<TileBehaviour>().tiletype == TileType.Empty ||
			    t2.GetComponent<TileBehaviour>().open == true &&
			    t2.GetComponent<TileBehaviour>().gas < this.gas)
				dirs.Add(t2);
		}
		if (coord_y > 0)
		{
			GameObject t3 = worldcontroller.getNeighbor(coord_x, coord_y-1);
			if (t3.GetComponent<TileBehaviour>().tiletype == TileType.Empty ||
			    t3.GetComponent<TileBehaviour>().open == true &&
			    t3.GetComponent<TileBehaviour>().gas < this.gas)
				dirs.Add(t3);
		}
		if (coord_y < 31)
		{	
			GameObject t4 = worldcontroller.getNeighbor(coord_x, coord_y+1);
			if (t4.GetComponent<TileBehaviour>().tiletype == TileType.Empty ||
			    t4.GetComponent<TileBehaviour>().open == true &&
			    t4.GetComponent<TileBehaviour>().gas < this.gas)
				dirs.Add(t4);
		}

		if (dirs.Count > 0)
		{
			GameObject selected = dirs[Random.Range(0, dirs.Count)];
			float difference = this.gas - selected.GetComponent<TileBehaviour>().gas;
			float transfer = difference / 2.0f;
			this.gas -= transfer;
			selected.GetComponent<TileBehaviour>().transferGas(transfer);
			
		}
		Color c = renderer.material.color;
		c.r = gas/100.0f;//interpolate(0.0f, 1.0f, gas/100.0f);
		c.g = gas/100.0f;//interpolate(0.0f, 1.0f, gas/100.0f);
		c.b = gas/100.0f;//interpolate(0.0f, 1.0f, gas/100.0f);
		c.a = 1.0f;
		gameObject.renderer.material.color = c;
	}

	public void transferGas(float g)
	{
		this.gas += g;
		//Debug.Log("Gas at " + coord_x + ", " + coord_y + ": " + gas);
		Color c = renderer.material.color;
		c.r = gas/100.0f;//interpolate(0.0f, 1.0f, gas/100.0f);
		c.g = gas/100.0f;//interpolate(0.0f, 1.0f, gas/100.0f);
		c.b = gas/100.0f;//interpolate(0.0f, 1.0f, gas/100.0f);
		c.a = 1.0f;
		gameObject.renderer.material.color = c;
	}

	public void setsolid()
	{
		Color c = Color.red;
		gameObject.renderer.material.color = c;
		tiletype = TileType.Solid;
		open = false;
	}

	public void toggleOpen()
	{
		if (open == false)
			open = true;
		else
		{
			open = false;
			Color c = Color.red;
			gameObject.renderer.material.color = c;
		}
	}

}
