using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
	public static GameControl self;
	public static float maxBacteriaPerCell;
	public float totalBacteria = 0.0f;
	public GameObject gridCellModel;
	public GameObject[,] grid;
	public Vector2 gridStart;
	public float gridCellMargin;
	public int brushSize;
	public Antibiotic selectedAntibiotic;
	public GameObject brushhead;
	public bool initFinished;
	public float timeUntilNextUpdate;

	public enum State{
		HoldingAntibiotic,
		PlayerIdle
	};
	public int state;
	[SerializeField]private List<GameObject> prevBrushCells;

	public void DropAntibiotic(){
		Debug.Log("dropping Antibiotic");
		Dictionary<string, Antibiotic> abs;
				
		foreach(GameObject cell in prevBrushCells){
			abs = cell.GetComponent<GridCell>().antibiotics;

			if(abs.ContainsKey(selectedAntibiotic.name)){
				Antibiotic ab = abs[selectedAntibiotic.name];
				ab.amount += 1f;
			}else{
				Antibiotic ab = Instantiate(selectedAntibiotic) as Antibiotic;
				ab.amount = 1f;
				abs.Add(ab.name, ab);
			}
		}
	}

	void Awake(){
		initFinished = false;
		self = this;
		grid = new GameObject[GameConfig.NUM_GRID_ROW, GameConfig.NUM_GRID_COL];
		gridStart = new Vector2(-20f, 15f);
		gridCellMargin = 0f;
		maxBacteriaPerCell = 800f;
		Application.targetFrameRate = 200;
		timeUntilNextUpdate = 0.05f;
		//selectedAntibiotic = null;
	}
	// Use this for initialization
	void Start () {
		prevBrushCells = new List<GameObject>();
		brushSize = 5;
		InitGrid();
		/*
		for(int i=0; i < grid.GetLength(0);i++){
			for(int j=0; j < grid.GetLength(1);j++){
				StartCoroutine(grid[i,j].GetComponent<Bacteria>().RunLifeCycle());
			}
		}
		*/
		state = (int)State.PlayerIdle;
		brushhead.SetActive(false);
		initFinished = true;
		//StartCoroutine(RunLifeCycle());

	}
	void RunLifeCycle(){
		int r = Mathf.FloorToInt(Random.Range(0, grid.GetLength(0)-0.01f));
		int c = Mathf.FloorToInt(Random.Range(0, grid.GetLength(1)-0.01f));
		grid[r,c].GetComponent<Bacteria>().RunLifeCycle();
		/*)
		for(int i=0; i < grid.GetLength(0);i++){
			for(int j=0; j < grid.GetLength(1);j++){
				//yield return new WaitForSeconds(0.05f);
				grid[i,j].GetComponent<Bacteria>().RunLifeCycle();
			}
		}
		*/
	}
	// Update is called once per frame
	void Update () {
		if(timeUntilNextUpdate > 0f){
			timeUntilNextUpdate -= Time.deltaTime;
		}else{
			//RunLifeCycle();
			timeUntilNextUpdate = 0.005f;
		}
		RunLifeCycle();
		if(state == (int)State.HoldingAntibiotic){
			int centerCell_i = 0, centerCell_j = 0;
			ScreenPointToGridIndices(ref centerCell_i, ref centerCell_j);
			//Debug.Log(centerCell_i + ", " + centerCell_j);
			//clear previous colored cells
			while(prevBrushCells.Count > 0){
				GameObject p = prevBrushCells[0];
				p.GetComponent<SpriteRenderer>().color = p.GetComponent<Bacteria>().previousColor;
				prevBrushCells.RemoveAt(0);
			}

			if(!IndicesWithinBounds(centerCell_i, centerCell_j))return;
			DrawBrush(centerCell_i, centerCell_j);
			
			if(Input.GetMouseButton(0)){
				
				DropAntibiotic();
			}
		}
	}
	void ScreenPointToGridIndices(ref int r, ref int c){
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10.0f;
		Vector3 brushPos = Camera.main.ScreenToWorldPoint(mousePos);
		//brushhead.transform.position = brushPos;
		//brushhead.SetActive(true);
		float cellWidth = gridCellModel.GetComponent<GridCell>().width;
		c = Mathf.FloorToInt((brushPos.x-gridStart.x) / cellWidth);
		r = Mathf.FloorToInt(-(brushPos.y-gridStart.y) / cellWidth);
	}
	bool IndicesWithinBounds(int r, int c){
		return c >= 0 && c < grid.GetLength(1)
				&& r >= 0 && r < grid.GetLength(0);
	}
	//draws a dimond shape brush
	void DrawBrush(int centerCell_i, int centerCell_j){
		int offset = brushSize / 2;
		for(int k = -offset; k <= offset; k++){
			//
			for(int j = centerCell_j - offset + Mathf.Abs(k); 
				j <= centerCell_j + offset - Mathf.Abs(k); 
				j++){
				int ci = centerCell_i + k;
				int cj = j;

				if(!IndicesWithinBounds(ci, cj))continue;
				prevBrushCells.Add(grid[ci,cj]);
				grid[ci,cj].GetComponent<Bacteria>().previousColor 
					= grid[ci,cj].GetComponent<SpriteRenderer>().color;
				grid[ci,cj].GetComponent<SpriteRenderer>().color = Color.red;
			}
			
		}
	}
	void InitGrid(){
		//clear grid
		for(int i=0; i < grid.GetLength(0);i++){
			for(int j=0; j < grid.GetLength(1);j++){
				GameObject cell = grid[i,j];
				if(cell != null){
					Destroy(cell);
				}
				Vector2 pos;
				pos = new Vector2(
						gridStart.x + j * (gridCellModel.GetComponent<GridCell>().width + gridCellMargin),
						gridStart.y - i * (gridCellModel.GetComponent<GridCell>().height + gridCellMargin)
					);
				grid[i,j] = Instantiate(gridCellModel, pos, Quaternion.identity) as GameObject;
				grid[i,j].SetActive(true);
				grid[i,j].GetComponent<GridCell>().Init(i,j);
			}
		}
		for(int i=0; i < grid.GetLength(0);i++){
			for(int j=0; j < grid.GetLength(1);j++){
				grid[i,j].GetComponent<Bacteria>().SetNeighbours();
				if(float.IsNaN(grid[i,j].GetComponent<Bacteria>().amount)){
					Debug.Log("init error");
				}
			}
		}
	}
}
