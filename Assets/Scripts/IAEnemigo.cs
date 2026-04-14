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
		hordas = GameObject.Find("HORDAS").GetComponent<Hordas>();
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
			VidasPlayer.puedePerderVida = 1;
		}
        
	}
    
	private void OnTriggerEnter(Collider obj){
		if(obj.tag == "Player"){ 
			tiempSigAtaque = frecAtaque;
			iniciaConteo = Time.time;
			obj.transform.GetComponentInChildren<VidasPlayer>().TomarDaño(1);
		}
	}
	

	public void TomarDaño(int daño){
		pvEnemigo.RPC("AplicarDemo", RpcTarget.All, daño, pvEnemigo.ViewID);		
	}

	[PunRPC]
	public void AplicarDemo(int daño,int viewID)
	{
		if (pvEnemigo.ViewID == viewID)
		{
			vidaEnemigo -= daño;
			if (vidaEnemigo <= 0)
			{
				if(!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && pvEnemigo.IsMine))
				{
					Debug.Log("Decrementa en Horda");
					hordas.enemigosVivos--;
				}
				Destroy(gameObject);				
			}
		}
	}
	
}