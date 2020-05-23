using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Jaireve;
public class GameController : MonoBehaviour {

	public static GameController instance= null;

	public Lexique lexique;
	// Use this for initialization
	void Start () {
		if( GameController.instance == null){
			GameController.instance = this;
			DontDestroyOnLoad(this.gameObject);
			lexique = new Lexique(Lexique.TextePath,Lexique.IndexPAth);
			if(lexique == null){
				Application.Quit();
			}
			lexique.nouvellePartie();
		}else{
			if(GameController.instance != this){
				Destroy(this.gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Cancel")){
			Quitter();
		}
	}

	public void Quitter(){
		Application.Quit();
	}

	public void Menu(){
		SceneManager.LoadScene("Menu");
	}

	public void Reveil(){
		SceneManager.LoadScene("Reveil");
	}
	public void Jouer(){
		lexique.nouvellePartie();
		SceneManager.LoadScene("Jeu");
	}

	public void Credits(){
		SceneManager.LoadScene("Crédits");
	} 

}
