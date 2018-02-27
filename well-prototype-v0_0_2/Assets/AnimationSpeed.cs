using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeed : MonoBehaviour {
 public Animator animator1;
 public Animator animator2;
 public Animator animator3;
 public Animator animator4;
 public Animator animator5;

 public float desiredSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

 
 	animator1.speed = desiredSpeed;
 	animator2.speed = desiredSpeed;
 	animator3.speed = desiredSpeed;
 	animator4.speed = desiredSpeed;
 	animator5.speed = desiredSpeed;
		
	}
}
