using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Bacteria : MonoBehaviour {
	public float amount;
	public int row ;
	public int col ;
	public List<GameObject> neighbours;
	public float ticks;
	
	public int mutationTimes; // max number of mutations
	public int minMutations;
	public int maxMutations;

	public float baseMutationRate;
	public float remainingTimeUntilNextUpdate;
	public Color baseColor;
	public Color previousColor; //used for brush moving

	public BacteriaStrain strain;
	public string nextGenBacteriaType; // next generation to mutate into
	public static float sqrt2 = Mathf.Sqrt(2f);
	public static float sqrt3 = Mathf.Sqrt(3f);
	public virtual void Awake(){
		row = -1;
		col = -1;
		baseMutationRate = 0.05f;
		neighbours = new List<GameObject>();
		minMutations = 5;
		maxMutations = 8;
		nextGenBacteriaType = "BacteriaMuggle";
	}
	// Use this for initialization
	public virtual void Start () {
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
			float newBacteriaAmount = amount*1/sqrt2;
			float mutationRnd = Random.Range(0f, 100f);
			if(mutationRnd < baseMutationRate * 100f){
				//mutate
				if(nextGenBacteriaType != ""){
					Mutate();
					Destroy(GetComponent<Bacteria>());
					gameObject.AddComponent<BacteriaMuggle>();
					GetComponent<BacteriaMuggle>().row = row;
					GetComponent<BacteriaMuggle>().col = col;
					GetComponent<BacteriaMuggle>().neighbours = neighbours;
					return;
				}


			}
			float randNeighbour = Random.Range(0, neighbours.Count-0.01f);
			Bacteria neighbourBacteria 
				= neighbours[Mathf.FloorToInt(randNeighbour)].GetComponent<Bacteria>();
			neighbourBacteria.baseColor = this.baseColor;

			if(amount < GameControl.maxBacteriaPerCell){
				neighbourBacteria.amount
				+= newBacteriaAmount;
				float oldAmount = amount;
				amount *= 1/sqrt2;
				GameControl.self.totalBacteria += (newBacteriaAmount + amount - oldAmount);
			}else{
				float oldAmount = amount;
				amount *= 1/sqrt3;//sqrt of 1/3
				GameControl.self.totalBacteria += (amount - oldAmount);
			}
			
			//update my color
			UpdateGridColor();
	}
	public void Mutate(){

		float h=50f, s=166/255f, v=75/255f;
		Color newBaseColor = Utils.HSV2RGB(h,s,v,1f);
		baseColor = newBaseColor;
		Debug.Log("mutate!");
		UpdateGridColor();

	}
	public void UpdateGridColor(){
		float h=0,s=0,v=0;
		float r=0,g=0,b=0;
		//Color oldColor = GetComponent<SpriteRenderer>().color;
		//Debug.Log("oldColor: " + oldColor);
		Utils.RGB2HSV(baseColor.r,baseColor.g,baseColor.b,
		 			  ref h, ref s, ref v);
		v = Mathf.Min(240f/255f, 75f/255f + amount/GameControl.maxBacteriaPerCell);
		//Debug.Log(v);
		Color newColor = Utils.HSV2RGB(h,s,v, 1f);
		
		GetComponent<SpriteRenderer>().color = newColor;
	}
	public void Init(int r, int c){
		row = r;
		col = c;
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
