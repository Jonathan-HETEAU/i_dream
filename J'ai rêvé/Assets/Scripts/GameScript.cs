using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Jaireve;

public class GameScript : MonoBehaviour {


	public Text Histoire ;
	public Text ChoixA;
	public Text ChoixB;

	public GameObject ButtonChoixA;
	public GameObject ButtonChoixB;

	public GameObject ButtonRestart;

	public Scrollbar ScrollbarHistoire;


	private int ChoixCourrent = -1;
	private string [,] tabChoix ;
	private string [] decisions;
	private string [] reve; 

	private bool updateScrollBar = true;

	private IDictionary<int,int> references ;
	// Use this for initialization
	void Start () {

		Lexique lexique = GameController.instance.lexique;
		reve = lexique.monTexte[true].ToArray();
		references = lexique.references;
		tabChoix = lexique.tabDeChoix;
		decisions = new string[tabChoix.Length];
		Next();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(updateScrollBar){
			ScrollbarHistoire.value = 0.0f;
		}
	}

	public void Choix(int c){
		switch(c){
			case 0:
			case 1:
				decisions[ChoixCourrent] = tabChoix[ChoixCourrent,c];
				Histoire.text = Histoire.text + tabChoix[ChoixCourrent,c];
				break;
			default:
				break;
		}
		Next();
		
	}

	public void Next(){
		ChoixCourrent ++;
		
		if(ChoixCourrent < reve.Length){
			Histoire.text = Histoire.text + Regex.Replace(reve[ChoixCourrent],@"\{([0-9]*)\}",new MatchEvaluator(this.LocalReplaceMatchCase));
		}


		if(ChoixCourrent>= tabChoix.GetLength(0)){
			ButtonChoixA.gameObject.SetActive(false);		
			ButtonChoixB.gameObject.SetActive(false);
			ButtonRestart.gameObject.SetActive(true);
			updateScrollBar = false;
		}else{
			ChoixA.text = tabChoix[ChoixCourrent,0];
			//Debug.Log("choixA:"+tabChoix[ChoixCourrent,0]);
			ChoixB.text = tabChoix[ChoixCourrent,1];
			//Debug.Log("choixB:"+tabChoix[ChoixCourrent,1]);
		}
	
	}

	public void Copier(){
		
		TextEditor te = new TextEditor();
		te.text = Histoire.text;
		te.SelectAll();
		te.Copy();
		
	}

	public void Quitter(){
		GameController.instance.Menu();
	}

	public void Restart(){
		GameController.instance.Jouer();
	}

	string LocalReplaceMatchCase(Match matchExpression){
		Debug.Log("LocalReplaceMatchCase:"+matchExpression.Groups[1].Value);
		return  decisions[references[int.Parse(matchExpression.Groups[1].Value)]];
	}

}
