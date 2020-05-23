using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ReveilScript : MonoBehaviour {

	// Use this for initialization
	private PlayableDirector pd ;
	

	void Start () {
		pd =  GetComponent<PlayableDirector>();
	}
	
	// Update is called once per frame
	void Update () {
		if(pd.state == PlayState.Paused){
			GameController.instance.Jouer();
		}
	}
}
