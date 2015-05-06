﻿using UnityEngine;
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
	public enum State{
		HoldingAntibiotic,
		PlayerIdle
	};
	public int state;
	private Stack<GameObject> prevBrushCells;

	void Awake(){
		self = this;
		grid = new GameObject[GameConfig.NUM_GRID_ROW, GameConfig.NUM_GRID_COL];
		gridStart = new Vector2(-20f, 15f);
		gridCellMargin = 0f;
		maxBacteriaPerCell = 1275f;
		Application.targetFrameRate = 150;
		selectedAntibiotic = null;
	}
	// Use this for initialization
	void Start () {
		prevBrushCells = new Stack<GameObject>();
		brushSize = 5;
		InitGrid();
		for(int i=0; i < grid.GetLength(0);i++){
			for(int j=0; j < grid.GetLength(1);j++){
				StartCoroutine(grid[i,j].GetComponent<Bacteria>().RunLifeCycle());
			}
		}
		state = (int)State.PlayerIdle;
		brushhead.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(state == (int)State.HoldingAntibiotic){
			int centerCell_i = 0, centerCell_j = 0;
			ScreenPointToGridIndices(ref centerCell_i, ref centerCell_j);
			//Debug.Log(centerCell_i + ", " + centerCell_j);
			//clear previous colored cells
			while(prevBrushCells.Count > 0){
				GameObject p = prevBrushCells.Pop();
				p.GetComponent<SpriteRenderer>().color = Color.white;
			}

			if(!IndicesWithinBounds(centerCell_i, centerCell_j))return;
			DrawBrush(centerCell_i, centerCell_j);
			
			if(Input.GetMouseButtonDown(0)){

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
				prevBrushCells.Push(grid[ci,cj]);
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
