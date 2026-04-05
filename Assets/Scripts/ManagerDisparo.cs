using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class ManagerDisparo : MonoBehaviour{

	public Camera playerCamera;
	
	public GameObject objFusil, objSniper;
	
	public Arma fusil;
	public Arma sniper;
	
	[SerializeField]private float rango;
	[SerializeField]public float frecuenciaDisparo;
	[SerializeField]private int dañoCausado;
	[SerializeField]private int capacidadArma;
	
	private float tiempoDisparo;	//Tiempo para poder disparar
	
	[SerializeField]private LayerMask lMask, otro;
	
	public ParticleSystem particulasDisparo;
	public GameObject impacto;
	
	public static int puntosPlayer;	//Puntos de la partida, despues seran almacenados en el archivo records
	

	private void Awake(){
		//Instancias de las armas a utilizar en el juego
		fusil = new Arma(20.0f, 0.20f, 1, 30);
		sniper = new Arma(70.0f, 1.25f, 5, 5);
		
		//Carga fusil por default
		OcultaArmas();
		objFusil.SetActive(true);
		
		puntosPlayer = 0;	//Iniciamos en 0 puntos la partida
	}

	void Update(){
		CambiaArma();
		tiempoDisparo += Time.deltaTime;
		if(Input.GetButtonDown("Fire1") && tiempoDisparo > frecuenciaDisparo){
	    	Dispara();
	    }
    }
    
	private void Dispara(){
		particulasDisparo.Play();
		tiempoDisparo = 0;
		Vector3 origen = playerCamera.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0));
		RaycastHit hit;
		
		if(Physics.Raycast(origen, playerCamera.transform.forward, out hit, rango, lMask)){
			
			GameObject objImpacto = Instantiate(impacto, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(objImpacto, 1.2f);
			IAEnemigo enemigo = hit.transform.GetComponent<IAEnemigo>();
			if(enemigo != null){	//Le estoy dando a un enemigo
				puntosPlayer++;
				enemigo.TomarDaño(dañoCausado);
			}
			Destroy(hit.transform.gameObject);
			
		} else if (Physics.Raycast(origen, playerCamera.transform.forward, out hit, rango, otro)){
			if (hit.rigidbody != null){
				hit.rigidbody.AddForce(hit.normal * 70.0f);
			}
			GameObject objImpacto = Instantiate(impacto, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(objImpacto, 1.2f);
		}
		
	}
	
	private void CambiaArma(){
		if(Input.GetKeyUp(KeyCode.Alpha1)){	//Fusil
			OcultaArmas();
			objFusil.SetActive(true);
			rango = fusil.alcance;
			frecuenciaDisparo = fusil.frecuenciaDisparo;
			dañoCausado = fusil.dañoCausado;
			capacidadArma = fusil.capacidad;
		}
		
		if(Input.GetKeyUp(KeyCode.Alpha2)){	//Sniper
			OcultaArmas();
			objSniper.SetActive(true);
			rango = sniper.alcance;
			frecuenciaDisparo = sniper.frecuenciaDisparo;
			dañoCausado = sniper.dañoCausado;
			capacidadArma = sniper.capacidad;
		}
	}
	
	private void OcultaArmas(){
		objFusil.SetActive(false);
		objSniper.SetActive(false);
	}
	
}
