using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class Navegacion : MonoBehaviour{
	
	public string nombreArchivo = "JuegoGuardado";
	public string nombreDirectorio = "Partidas";
	public GameData datosJuegos;	//Instancia de la estructura
	
	public static string nombreJugador = "";
	
	private void Awake(){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	
    void Start(){
	    if(!Directory.Exists(nombreDirectorio)){	//Si no existe el directorio lo crea igual que al archivo
	    	Directory.CreateDirectory(nombreDirectorio);
	    	BinaryFormatter formatter = new BinaryFormatter();	//Convierte los datos de unity en binario
	    	FileStream saveFile = File.Create(nombreDirectorio + "/" + nombreArchivo + ".bin");	//Ubicacion del archivo a grabar
	    	formatter.Serialize(saveFile, datosJuegos);
	    	saveFile.Close();
	    	Debug.Log("Guardado en " + Directory.GetCurrentDirectory().ToString() + "/" + nombreDirectorio + "/" + nombreArchivo + ".bin");
	    }
    }
    
	//Cuando se va al juego se lleva el nombre del usuario por si rompe el record guardarlo en el archivo
	public void IrJuego(){
		nombreJugador = GameObject.Find("txtNombre").GetComponent<InputField>().text;
		SceneManager.LoadScene("SampleScene");
	}

}
