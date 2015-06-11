using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BacteriaMuggle : Bacteria {
	public override void Awake(){
		base.Awake();
		minMutations = 5;
		maxMutations = 8;
		//[minMutations, maxMutations]
		mutationTimes = Mathf.FloorToInt(Random.Range(minMutations, maxMutations+1f));
		//generate base color based on hsv => rgb
		float h=50f, s=166/255f, v=75/255f;
		baseColor = Utils.HSV2RGB(h,s,v,1.0f);
		nextGenBacteriaType = null;
	}
	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
