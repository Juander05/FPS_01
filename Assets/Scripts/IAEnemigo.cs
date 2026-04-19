using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class IAEnemigo : MonoBehaviour{
	
	[SerializeField] private NavMeshAgent agente;
	private GameObject player;
	private float velEnemigo;
	private float dist;
	public static int vidaEnemigo;
	private float frecAtaque = 2.5f, tiempSigAtaque = 0, iniciaConteo;
	
	public Hordas hordas;
	public PhotonView pvEnemigo;
	
	void Start(){
    	
		player = GameObject.Find("PlayerCapsule");
		hordas = GameObject.Find("Hordas").GetComponent<Hordas>();
		dist = Vector3.Distance(player.transform.position, transform.position);
		agente.speed = Random.RandomRange(1,5); 
		vidaEnemigo = 1;
    }

	void Update(){
    	
		dist = Vector3.Distance(player.transform.position, transform.position);
		
		if(tiempSigAtaque > 0){
			tiempSigAtaque = frecAtaque + iniciaConteo - Time.time;
		} else {
			tiempSigAtaque = 0;
			agente.SetDestination(player.transform.position);
		}
        
	}
    
	private void OnTriggerEnter(Collider obj)
	{
		if (obj.CompareTag("Player"))
		{
			VidasPlayer vp = obj.GetComponentInChildren<VidasPlayer>();
			PhotonView pvPlayer = obj.GetComponent<PhotonView>();

			if (vp != null)
			{
				// Solo el dueño del player recibe daño
				if (!PhotonNetwork.InRoom || pvPlayer.IsMine)
				{
					vp.puedePerderVida = 1;
					vp.TomarDaño(1);
				}
			}
		}
	}
	

	public void TomarDaño(int daño){
		Debug.Log("Se llamó TomarDaño en enemigo " + pvEnemigo.ViewID);
		if (PhotonNetwork.InRoom)
		{
			pvEnemigo.RPC("AplicarDemo", RpcTarget.MasterClient, daño, pvEnemigo.ViewID);
		}
		else
		{
			AplicarDemo(daño, pvEnemigo.ViewID);
		}		
	}

	[PunRPC]
	public void AplicarDemo(int daño, int viewID)
	{
		// En online: solo el Master procesa
		if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
			return;

		if (pvEnemigo.ViewID == viewID)
		{
			vidaEnemigo -= daño;
			Debug.Log("Vida enemigo: " + vidaEnemigo);

			if (vidaEnemigo <= 0)
			{
				Debug.Log("ENEMIGO MUERTO");

				hordas.enemigosVivos--;
				Debug.Log("Enemigos vivos ahora: " + hordas.enemigosVivos);

				if (PhotonNetwork.InRoom)
					PhotonNetwork.Destroy(gameObject);
				else
					Destroy(gameObject);
			}
		}
	}
	
}