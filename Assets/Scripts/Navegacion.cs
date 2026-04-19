using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using Photon.Pun;

public class Navegacion : MonoBehaviour
{
	public string nombreArchivo = "JuegoGuardado";
	public string nombreDirectorio = "Partidas";
	public GameData datosJuegos;

	public static string nombreJugador = "";

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void Start()
	{
		if (!Directory.Exists(nombreDirectorio))
		{
			Directory.CreateDirectory(nombreDirectorio);

			BinaryFormatter formatter = new BinaryFormatter();
			FileStream saveFile = File.Create(nombreDirectorio + "/" + nombreArchivo + ".bin");

			formatter.Serialize(saveFile, datosJuegos);
			saveFile.Close();

			Debug.Log("Guardado en " + Directory.GetCurrentDirectory() + "/" + nombreDirectorio + "/" + nombreArchivo + ".bin");
		}
	}

	public void IrJuego()
	{
		nombreJugador = GameObject.Find("txtNombre").GetComponent<InputField>().text;

		// 🔴 DIFERENCIA CLAVE
		if (PhotonNetwork.InRoom)
		{
			Debug.Log("Cargando escena ONLINE");

			PhotonNetwork.AutomaticallySyncScene = true;

			// 🔵 ESCENA MULTIPLAYER (crea esta escena)
			PhotonNetwork.LoadLevel("JuegoEnLinea");
		}
		else
		{
			Debug.Log("Cargando escena OFFLINE");

			// 🟢 ESCENA OFFLINE
			SceneManager.LoadScene("SampleScene");
		}
	}
}