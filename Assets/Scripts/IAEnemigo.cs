using UnityEngine;
using UnityEngine.AI;

public class IAEnemigo : MonoBehaviour{
	
	[SerializeField] private NavMeshAgent agente;
	private GameObject player;
	private float velEnemigo;
	private float dist;
	public static int vidaEnemigo;
	private float frecAtaque = 2.5f, tiempSigAtaque = 0, iniciaConteo;
	
	public Hordas hordas;
	
	
	void Start(){
    	
		player = GameObject.Find("PlayerCapsule");
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
		
		/*
		if(dist <= 15){ //Enemigo sigue al player
			agente.SetDestination(player.transform.position);
		}
		*/
        
	}
    
	private void OnTriggerEnter(Collider obj){
		if(obj.tag == "Player"){ //Daño que el enemigo le genera al player 
			tiempSigAtaque = frecAtaque;
			iniciaConteo = Time.time;
			obj.transform.GetComponentInChildren<VidasPlayer>().TomarDaño(1);
		}
	}
	
	public void TomarDaño(int daño){
		vidaEnemigo -= daño;
		if(vidaEnemigo <= 0){
			hordas.enemigosVivos--;
			Destroy(gameObject);
		}
	}
}
