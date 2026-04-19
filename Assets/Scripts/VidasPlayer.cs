using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class VidasPlayer : MonoBehaviour
{
	public Image vidaPlayer;
	private float anchoVidasPlayer;
	public int vida; 
	private bool haMuerto;
	public GameObject gameOver;
	private const int vidaINI = 5;
	public int puedePerderVida = 1;

	public Text txtPuntos;
	public Text txtRecord;
	public Text nombreR;

	public GameObject fm;

	private PhotonView pv;

	void Start()
	{
		Debug.Log("gameOver encontrado: " + gameOver);
		Debug.Log("nombreR encontrado: " + nombreR);
		Debug.Log("txtPuntos encontrado: " + txtPuntos);

		pv = GetComponent<PhotonView>();

		if (PhotonNetwork.InRoom && !pv.IsMine)
			return;

		txtPuntos = GameObject.Find("txtPuntos")?.GetComponent<Text>();
		vidaPlayer = GameObject.Find("VidaPlayer")?.GetComponent<Image>();
		gameOver = GameObject.Find("GameOver");
		fm = GameObject.Find("FileManager");

		txtRecord = GameObject.Find("txtRecord")?.GetComponent<Text>();
		nombreR = GameObject.Find("txtNombre")?.GetComponent<Text>();


		if (vidaPlayer != null)
			anchoVidasPlayer = vidaPlayer.GetComponent<RectTransform>().sizeDelta.x;

		haMuerto = false;
		vida = vidaINI;

		if (gameOver != null)
			gameOver.SetActive(false);

		if (txtRecord != null)
			txtRecord.text = "Record: " + FileManager.record.ToString();

		if (nombreR != null)
			nombreR.text = FileManager.nombreR;
		
	}

	private void Update()
	{
		if (PhotonNetwork.InRoom && !pv.IsMine)
			return;

		if (txtPuntos != null)
			txtPuntos.text = "Puntos: " + ManagerDisparo.puntosPlayer.ToString();
			
	}

	public void TomarDaño(int daño)
	{
		
		if (PhotonNetwork.InRoom && !pv.IsMine)
			return;

		if (vida > 0 && puedePerderVida == 1)
		{
			puedePerderVida = 0;
			vida -= daño;
			DibujaVida(vida);
		}

		if (vida <= 0 && !haMuerto)
		{
			haMuerto = true;

			if (ManagerDisparo.puntosPlayer > FileManager.record && fm != null)
			{
				fm.GetComponent<FileManager>().SaveToFile();
			}

			StartCoroutine(EjecutaMuerte());
		}
	}

	private void DibujaVida(int vida)
	{
		if (vidaPlayer == null) return;

		RectTransform transformaImagen = vidaPlayer.GetComponent<RectTransform>();
		transformaImagen.sizeDelta = new Vector2(
			anchoVidasPlayer * (float)vida / (float)vidaINI,
			transformaImagen.sizeDelta.y
		);
	}

	IEnumerator EjecutaMuerte()
	{
		if (gameOver != null)
			gameOver.SetActive(true);

		yield return new WaitForSeconds(2.2f);
		SceneManager.LoadScene("Menu");
	}
}