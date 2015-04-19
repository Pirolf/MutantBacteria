﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Bacteria : MonoBehaviour {
	public float amount;
	public int row ;
	public int col ;
	public List<GameObject> neighbours;
	public float ticks;
	public bool inited = false;
	void Awake(){
		row = -1;
		col = -1;
		neighbours = new List<GameObject>();
	}
	// Use this for initialization
	void Start () {
		ticks = 0.0f;
		//amount = 1f;
		amount = 5f*Mathf.PerlinNoise(transform.position.x, transform.position.y);
		float emptyRnd = Random.Range(0f,1.0f);
		if(emptyRnd < 0.5f){
			amount = 0f;
		}
		UpdateGridColor();
	}
	
	// Update is called once per frame
	void Update () {
	}
	public IEnumerator RunLifeCycle(){
		
		while(true){
			yield return new WaitForSeconds(3f);
			//Debug.Log("started life");
			//count total neighgour's bacteria amount
			float totalBacteria = 0f;
			if(amount < 0.0011f)continue;
			foreach(GameObject neighbour in neighbours){
				if(float.IsNaN(neighbour.GetComponent<Bacteria>().amount)){
					continue;
				}
				totalBacteria += neighbour.GetComponent<Bacteria>().amount;
			}
			//plus self
			totalBacteria += amount;
			if(totalBacteria <= 0.0001f)continue;
			float newBacteriaAmount = amount;
			float randNeighbour = Random.Range(0, neighbours.Count);
			Bacteria neighbourBacteria = neighbours[Mathf.FloorToInt(randNeighbour)].GetComponent<Bacteria>();
			neighbourBacteria.amount
				+= newBacteriaAmount;
				/*
			foreach(GameObject neighbour in neighbours){
				if(float.IsNaN(neighbour.GetComponent<Bacteria>().amount))continue;
				float randNeighbour = Random.Range(0, neighbours.Count());

				neighbour.GetComponent<Bacteria>().amount 
					+= (neighbour.GetComponent<Bacteria>().amount/totalBacteria) * newBacteriaAmount;
			}
			amount += (amount/totalBacteria) * newBacteriaAmount;
			*/
			//update my color
			UpdateGridColor();
		}

		
	
	}
	public void UpdateGridColor(){
		Color oldColor = GetComponent<SpriteRenderer>().color;
		GetComponent<SpriteRenderer>().color = new Color(
			oldColor.r,
			oldColor.g,
			oldColor.b,
			10f * Mathf.CeilToInt(amount/50f)/255f
			);  
	}
	public void Init(int r, int c){
		row = r;
		col = c;
		/*
		amount = Mathf.PerlinNoise(transform.position.x, transform.position.y);
		//Debug.Log(amount);
		float emptyRnd = Random.Range(0f,1.0f);
		if(emptyRnd < 0.9f){
			amount = 0f;
		}
		UpdateGridColor();	
		
		*/
		inited = true;
	}
	public void SetNeighbours(){
		//add neighbours
		int totalRow = GameControl.self.grid.GetLength(0);
		int totalCol = GameControl.self.grid.GetLength(1);
		if(row == 0){
			neighbours.Add(GameControl.self.grid[row+1,col]);
			if(col == 0){
				//right, bottom
				neighbours.Add(GameControl.self.grid[row,col+1]);
			}else if(col == totalCol -1){
				//left, bottom
				neighbours.Add(GameControl.self.grid[row,col-1]);
			}else{
				//left, bottom, right
				neighbours.Add(GameControl.self.grid[row,col+1]);
				neighbours.Add(GameControl.self.grid[row,col-1]);
			}
		}else if(row == totalRow-1){
			//up
			neighbours.Add(GameControl.self.grid[row-1,col]);
			if(col == 0){
				//up, right
				neighbours.Add( GameControl.self.grid[row,col+1]);
			}else if(col == totalCol-1){
				//up, left
				neighbours.Add(GameControl.self.grid[row,col-1]);
			}else{
				//up, left, right
				neighbours.Add( GameControl.self.grid[row,col+1]);
				neighbours.Add(GameControl.self.grid[row,col-1]);
			}
		}else{
			//bottom
			neighbours.Add(GameControl.self.grid[row+1,col]);
			//up
			neighbours.Add(GameControl.self.grid[row-1,col]);
			if(col == 0){
				//up, right, bottom
				neighbours.Add(GameControl.self.grid[row,col+1]);
			}else if(col == totalCol-1){
				//up,left, bottom
				neighbours.Add(GameControl.self.grid[row,col-1]);
			}else{
				//up,left,right,bottom
				neighbours.Add(GameControl.self.grid[row,col+1]);
				neighbours.Add(GameControl.self.grid[row,col-1]);
			}
		}
	}
}
