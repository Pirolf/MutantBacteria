using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BacteriaStrain : MonoBehaviour {
	public string name;
	public Color baseColor;
	public Dictionary<int, DNASegment> mutantSegments;
	public DNASegment segmentTemplate; //for instantiation
	// Use this for initialization
	void Start () {
		mutantSegments = new Dictionary<int, DNASegment>();
	}
	public void AddMutantSegment(DNASegment mutantSegment){
		if(mutantSegments.ContainsKey(mutantSegment.segmentIndex)){
			//already mutant
			Debug.Log("DNASegment " + mutantSegment.segmentIndex + " is alreay mutant!");
			return;
		}
		mutantSegments.Add(mutantSegment.segmentIndex, mutantSegment);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
