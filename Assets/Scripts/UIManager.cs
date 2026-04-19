using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;

	public Text txtPuntos;
	public Text txtRecord;
	public Text nombreR;
	public Image vidaPlayer;
	public GameObject gameOver;

	void Awake()
	{
		instance = this;
	}
}