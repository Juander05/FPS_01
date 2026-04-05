using UnityEngine;

public class Arma : MonoBehaviour{
   
	public float alcance;
	public float frecuenciaDisparo;
	public int dañoCausado;
	public int capacidad;
	
	public Arma(float a, float f, int d, int c){
		this.alcance = a;
		this.frecuenciaDisparo = f;
		this.dañoCausado = d;
		this.capacidad = c;
	}
}
