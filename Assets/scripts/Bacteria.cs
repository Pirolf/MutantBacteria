using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Bacteria : MonoBehaviour {
	public float amount;
	public int row ;
	public int col ;
	public List<GameObject> neighbours;
	public float ticks;
	public bool inited = false;
	public float baseMutationRate;
	public float remainingTimeUntilNextUpdate;
	public Color baseColor;
	public Color previousColor; //used for brush moving

	public BacteriaStrain strain;
	void Awake(){
		row = -1;
		col = -1;
		baseMutationRate = 0.05f;
		baseColor = Color.white;
		neighbours = new List<GameObject>();
	}
	// Use this for initialization
	void Start () {
		ticks = 0.0f;
		//amount = 1f;
		float perlinValue = Mathf.PerlinNoise(transform.position.x, transform.position.y);
		
		amount = 150f * perlinValue;
		GetComponent<SpriteRenderer>().color = baseColor;
		previousColor = GetComponent<SpriteRenderer>().color;
		if(perlinValue < 0.8f){
			amount = 0f;
		}
		UpdateGridColor();
		remainingTimeUntilNextUpdate = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(!GameControl.self.initFinished)return;
		if(row < 0 || col < 0)return;
		if(remainingTimeUntilNextUpdate >= 0f){
			remainingTimeUntilNextUpdate -= Time.deltaTime;
			return;
		}
		RunLifeCycle();
		remainingTimeUntilNextUpdate = 0.5f;
		*/
	}
	public void RunLifeCycle(){
			
			//Debug.Log("started life");
			//count total neighgour's bacteria amount
			float totalBacteria = 0f;
			if(amount < 0.0011f)return;
			foreach(GameObject neighbour in neighbours){
				if(float.IsNaN(neighbour.GetComponent<Bacteria>().amount)){
					return;
				}
				totalBacteria += neighbour.GetComponent<Bacteria>().amount;
			}
			//plus self
			totalBacteria += amount;
			if(totalBacteria <= 0.0001f)return;
			if(GetComponent<GridCell>().antibiotics.Count > 0){
				//die 100 or all (max(amount-100, 0))
				Debug.Log("killing bac");
				amount = Mathf.Max(amount-100.0f,0f);
				UpdateGridColor();
				previousColor = GetComponent<SpriteRenderer>().color;
				return;

			}
			float newBacteriaAmount = amount*1/1.414f;
			float mutationRnd = Random.Range(0f, 100f);
			if(mutationRnd < baseMutationRate * 100f){
				//mutate
				Mutate();
			}
			float randNeighbour = Random.Range(0, neighbours.Count-0.01f);
			Bacteria neighbourBacteria 
				= neighbours[Mathf.FloorToInt(randNeighbour)].GetComponent<Bacteria>();
			
			if(amount < GameControl.maxBacteriaPerCell){
				neighbourBacteria.amount
				+= newBacteriaAmount;
				float oldAmount = amount;
				amount *= 1/1.414f;
				GameControl.self.totalBacteria += (newBacteriaAmount + amount - oldAmount);
			}else{
				float oldAmount = amount;
				amount *= 0.575f;//sqrt of 1/3
				GameControl.self.totalBacteria += (amount - oldAmount);
			}
			
			//update my color
			UpdateGridColor();
		

		
	
	}
	public void Mutate(){

	}
	public void UpdateGridColor(){
		Color oldColor = GetComponent<SpriteRenderer>().color;
		GetComponent<SpriteRenderer>().color = new Color(
			oldColor.r,
			oldColor.g,
			oldColor.b,
			amount/GameControl.maxBacteriaPerCell
			);  
	}
	public void Init(int r, int c){
		row = r;
		col = c;
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
