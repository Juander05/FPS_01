using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileManager : MonoBehaviour{
	
	public string nombreArchivo = "JuegoGuardado";
	public string nombreDirectorio = "Partidas";
	public GameData datosJuego;	//Instancia de la estructura
	public static int record;	//Variable que carga el record
	public static string nombreR;	//Nombre de la persona que tiene el record de puntos
	
	void Awake(){
	    LoadFile();	//Al cargar la escena del juego se cargan los datos almacenados
    }
    
	//Metodo para salvar un archivo
	public void SaveToFile(){
		if(!Directory.Exists(nombreDirectorio)) Directory.CreateDirectory(nombreDirectorio);
		BinaryFormatter formatter = new BinaryFormatter();	//Convierte los datos de unity en binarios
		using(FileStream saveFile = File.Create(nombreDirectorio + "/" + nombreArchivo + ".bin")){			//Ubicacion del archivo a grabar
			GameData datosjuego = new GameData(ManagerDisparo.puntosPlayer, 5.1f, Navegacion.nombreJugador);	//Salva los datos del juego cuando pierde el jugador
			formatter.Serialize(saveFile, datosjuego);
		}
		//GameData datosjuego = new GameData(ManagerDisparo.puntosPlayer, 5.1f, Navegacion.nombreJugador);	//Salva los datos del juego cuando pierde el jugador
		//formatter.Serialize(saveFile, datosjuego);
		//saveFile.Close();
		Debug.Log("Guardado en " + Directory.GetCurrentDirectory().ToString() + "/Saves/" + nombreArchivo + ".bin");
	}

	//Metodo para cargar los datos del archivo
	public void LoadFile(){
		
		string path = nombreDirectorio + "/" + nombreArchivo + ".bin";

		if (!File.Exists(path))
		{
			Debug.Log("No hay archivo guardado");
			return;
		}

		BinaryFormatter formatter = new BinaryFormatter();

		using (FileStream saveFile = File.Open(path, FileMode.Open))
		{
			GameData loadData = (GameData)formatter.Deserialize(saveFile);

			Debug.Log("Datos cargados ***********");
			Debug.Log("Nombre " + loadData.nombre);
			Debug.Log("Puntos " + loadData.puntos);
			Debug.Log("Tiempo " + loadData.tiempo);

			record = loadData.puntos;
			nombreR = loadData.nombre;
		}
		/*BinaryFormatter formatter = new BinaryFormatter();
		FileStream saveFile = File.Open(nombreDirectorio + "/" + nombreArchivo + ".bin", FileMode.Open);
		GameData loadData = (GameData)formatter.Deserialize(saveFile);
		Debug.Log("Datos cargados ***********");
		Debug.Log("Nombre " + loadData.nombre);
		Debug.Log("Puntos " + loadData.puntos);
		Debug.Log("Tiempo " + loadData.tiempo);
		record = loadData.puntos;
		nombreR = loadData.nombre;
		*/
	}
}
