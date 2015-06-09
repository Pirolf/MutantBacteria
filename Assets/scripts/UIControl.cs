using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {
	public Text bacteriaCountText;
	// Use this for initialization
	void Start () {
	
	}
	public void OnClickAntibiotic(){
		//GameControl.self.selectedAntibiotic = ab;
		GameControl.self.state = (int)GameControl.State.HoldingAntibiotic;

	}
	// Update is called once per frame
	void Update () {
		float totalBacteria = 0f;
		for(int i=0; i < GameControl.self.grid.GetLength(0);i++){
			for(int j=0; j < GameControl.self.grid.GetLength(1);j++){
				if(GameControl.self.grid[i,j].GetComponent<Bacteria>() != null)
					totalBacteria += GameControl.self.grid[i,j].GetComponent<Bacteria>().amount;
			}
		}
		if(totalBacteria > 1000000){
			bacteriaCountText.text = (totalBacteria / 1000000f).ToString("0.00") + " M";
		}else if(totalBacteria > 1000){
			bacteriaCountText.text = (totalBacteria / 1000f).ToString("0.00") + " K";
		}else{
			bacteriaCountText.text = totalBacteria.ToString("0");
		}
		
		//bacteriaCountText.text = GameControl.self.totalBacteria.ToString("0");
	}
}
