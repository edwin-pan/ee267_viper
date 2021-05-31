using UnityEngine;
using System.Collections;

public class PlayerWalk : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () 
	{
		anim= GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () 
	{
		if (anim) 
		{
			AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo (0);

			// var goalPost = GameObject.Find("GoalPosts");
			// if (goalPost) {
			// 	var goalPostPos = goalPost.transform.position;
			// 	// Debug.Log (location);
			// 	var playerPos = GameObject.Find("m_avg").transform.position;
			// 	var distance = Vector3.Distance(playerPos, goalPostPos);
			// 	// Debug.Log (distance);
			// 	if (distance < 9) {
			// 		anim.SetBool ("JumpingJack", true);
			// 		// Debug.Log (anim.GetBool("JumpingJack"));
			// 	} else {
			// 		anim.SetBool ("JumpingJack", false);
			// 	}
			// 	Debug.Log (anim.GetBool("JumpingJack") + " : " + animState.IsName ("Base Layer.Idle") + " : " + animState.IsName ("Base Layer.JumpingJack"));
			// }

			// Jump when SPACE key is pressed
			if (animState.IsName ("Base Layer.WalkForward")) 
			{
				if (Input.GetButton ("Jump"))
					anim.SetBool ("Jump", true);
			}
			else 
			{
				anim.SetBool ("Jump", false);
			}

			// Crouch when CTRL key is pressed
			if (animState.IsName ("Base Layer.Idle")) 
			{
				if (Input.GetButtonDown ("Fire1"))
					anim.SetBool ("Crouch", true);		
			}

			// Go back to idle when CTRL key is released
			if (animState.IsName ("Base Layer.Crouch")) 
			{
				if (Input.GetButtonUp ("Fire1"))
					anim.SetBool ("Crouch", false);		
			}

			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");

			anim.SetFloat ("Speed", v);
			anim.SetFloat ("Direction", h);
			anim.speed = 2f;
		}
	}
}
