using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	public static GameControl self;
	public static float maxBacteriaPerCell;
	public float totalBacteria = 0.0f;
	public GameObject gridCellModel;
	public GameObject[,] grid;
	public Vector2 gridStart;
	public float gridCellMargin;

	public Antibiotic selectedAntibiotic;
	public GameObject brushhead;
	public enum State{
		HoldingAntibiotic,
		PlayerIdle
	};
	public int state;
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
			//ray cast brush onto the grid
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10.0f;
			Vector3 brushPos = Camera.main.ScreenToWorldPoint(mousePos);
			brushhead.transform.position = brushPos;
			brushhead.SetActive(true);
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
