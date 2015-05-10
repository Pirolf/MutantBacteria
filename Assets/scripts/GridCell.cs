using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridCell : MonoBehaviour {
	public Bacteria bacteria;
	public Dictionary<string, Antibiotic> antibiotics;
	public float width;
	public float height;
	public SpriteRenderer renderer;
	void Awake(){
		renderer = GetComponent<SpriteRenderer>();
		antibiotics = new Dictionary<string, Antibiotic>();
		width = renderer.bounds.size.x - 2f * renderer.sprite.border.z/800f;
		height = renderer.bounds.size.y - 2f * renderer.sprite.border.y/800f;
	}
	// Use this for initialization
	void Start () {
		bacteria = GetComponent<Bacteria>();
	}
	public void Init(int row, int col){
		bacteria.Init(row, col);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
