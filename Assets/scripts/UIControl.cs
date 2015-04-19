using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {
	public Text bacteriaCountText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float totalBacteria = 0f;
		for(int i=0; i < GameControl.self.grid.GetLength(0);i++){
			for(int j=0; j < GameControl.self.grid.GetLength(1);j++){
				totalBacteria += GameControl.self.grid[i,j].GetComponent<Bacteria>().amount;
			}
		}
		bacteriaCountText.text = totalBacteria.ToString("0");
		//bacteriaCountText.text = GameControl.self.totalBacteria.ToString("0");
	}
}
