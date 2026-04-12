using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VidasPlayer : MonoBehaviour{
	
	public Image vidaPlayer;
	private float anchoVidasPlayer;
	public static int vida;
	private bool haMuerto;
	public GameObject gameOver;
	private const int vidaINI = 5;
	public static int puedePerderVida = 1;
	
	public Text txtPuntos;
	public Text txtRecord;
	public Text nombreR;
	
	public GameObject fm;	//Instancia del filemanager para acceder al metodo SaveToFile
	
    void Start(){
		txtPuntos = GameObject.Find("txtPuntos").GetComponent<Text>();
		vidaPlayer = GameObject.Find("VidaPlayer").GetComponent<Image>();
		gameOver = GameObject.Find("GameOver");
		fm = GameObject.Find("FileManager");
		Debug.Log("txtPuntos: " + txtPuntos);
		Debug.Log("vidaPlayer: " + vidaPlayer);
		Debug.Log("gameOver: " + gameOver);
		Debug.Log("fm: " + fm);
	    anchoVidasPlayer = vidaPlayer.GetComponent<RectTransform>().sizeDelta.x;
	    haMuerto = false;
	    vida = vidaINI;
	    gameOver.SetActive(false);
	    
	    txtRecord.text = "Record: " + FileManager.record.ToString();	//Imprime el record en pantalla
	    nombreR.text = FileManager.nombreR;	//Nombre del poseedor del record
    }
    
	private void Update(){
		txtPuntos.text = "Puntos: " + ManagerDisparo.puntosPlayer.ToString();	//Actualizacion de los puntos de la partida 
	}

	public void TomarDaño(int daño){
		if(vida > 0 && puedePerderVida == 1){
			puedePerderVida = 0;
			vida -= daño;
			DibujaVida(vida);
		}
		
		if(vida <= 0 && !haMuerto){
			haMuerto = true;
			if(ManagerDisparo.puntosPlayer > FileManager.record){	//Si se rompe el record
				fm.GetComponent<FileManager>().SaveToFile();	//Se actualiza el archivo de records
			}
			StartCoroutine(EjecutaMuerte());
		}
		
	}
	
	private void DibujaVida(int vida){
		RectTransform transformaImagen = vidaPlayer.GetComponent<RectTransform>();
		transformaImagen.sizeDelta = new Vector2(anchoVidasPlayer * (float)vida / (float)vidaINI ,transformaImagen.sizeDelta.y);
	}
	
	IEnumerator EjecutaMuerte(){
		gameOver.SetActive(true);
		yield return new WaitForSeconds(1.2f);
		SceneManager.LoadScene("Menu");
	}
	
}
