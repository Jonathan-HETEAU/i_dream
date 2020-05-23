using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jaireve
{
	public class Lexique {

		public static string IndexPAth = System.IO.Path.Combine(Application.streamingAssetsPath,"index.txt");
		public static string TextePath = System.IO.Path.Combine(Application.streamingAssetsPath,"reve.unity.txt");

		public IDictionary<bool,List<string>> monTexte = new Dictionary<bool,List<string>>();
		private IDictionary<string,int> monIndex= new Dictionary<string,int>();
		private IDictionary<string, int> occCode = new Dictionary<string,int>();

		private IDictionary<string,List<string>> monLexique = new Dictionary<string,List<string>>();

		public string [,] tabDeChoix;
		public IDictionary<int,int> references ;


		public Lexique (string texte, string index){

			
				Debug.Log("index path:"+index);
				using(StreamReader lecteur =  File.OpenText(index)){
					string line =lecteur.ReadLine();
									
					while(line != null){
						//Debug.Log(line);
						string[] tabIndex = line.Split('=');
						monIndex[tabIndex[0]]=int.Parse(tabIndex[1]);
						line = lecteur.ReadLine();
					}
				}

				monTexte[true]= new List<string>();
				monTexte[false]= new List<string>();
				references = new Dictionary<int,int>();
				Debug.Log("texte:"+texte);
				using(StreamReader lecteur =  File.OpenText(texte)){
					string line =lecteur.ReadLine();
					bool isTexte = true;	
					int refIndice = 0;			
					while(line != null){
						//Debug.Log("ligne:"+line);
						if(!isTexte){
							string[] code = line.Split('|');
						//	Debug.Log("code:"+code[0]);
							monTexte[isTexte].Add(code[0]);
							int nbr = 0;
							occCode.TryGetValue(code[0], out nbr);
							nbr ++;
							occCode[code[0]] = nbr;
							if(code.Length == 2){
								references.Add(int.Parse(code[1]),refIndice);
							}
							refIndice++;
						}else{
							monTexte[isTexte].Add(line);
						}
						line = lecteur.ReadLine();
						isTexte = !isTexte;
					}
					//Debug.Log("text size ="+monTexte[true].Count+"  Info count="+monTexte[false].Count);
				}
			
		}

		public bool nouvellePartie(){
			Debug.Log("nouvellePartie");
			monLexique = new Dictionary<string,List<string>>();
			foreach( KeyValuePair<string,int> pair in occCode){
				Debug.Log("k:"+pair.Key);
				int max = monIndex[pair.Key];
				int premier = Random.Range(0, max);
				List<string> mots = new List<string>();
				while(mots.Count < pair.Value*2 ){
					string filePath =  System.IO.Path.Combine(Application.streamingAssetsPath,System.IO.Path.Combine(pair.Key,premier+".txt"));
					Debug.Log("filepath:"+filePath);
					using(StreamReader lecteur =  File.OpenText(filePath)){
						string line =lecteur.ReadLine();
						while(line!=null){
							mots.Add(line);
							line =lecteur.ReadLine();
						}									
					}
					premier = (premier + Random.Range(1, max))%max;
				}
				
				monLexique[pair.Key]= mots;
			}

			Debug.Log("2éme partie");

			tabDeChoix = new string[monTexte[false].Count,2];
			int saut = Random.Range(0,100);
			int indice =Random.Range(0,100);
			for(int i = 0; i < monTexte[false].Count ; i++){
				string code = monTexte[false][i];
				List<string> liste = monLexique[code]; 
				indice = indice % liste.Count;
				tabDeChoix[i,0]= liste[indice];
				liste.RemoveAt(indice);
				indice = (indice+saut) % liste.Count;
				tabDeChoix[i,1]=  liste[indice];
				liste.RemoveAt(indice);
			}

			return true;
		} 
		

	}
}
