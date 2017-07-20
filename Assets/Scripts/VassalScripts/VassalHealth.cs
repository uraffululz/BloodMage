using UnityEngine;
using System.Collections;

public class VassalHealth : MonoBehaviour {

	public GameObject HPBar;
	public BMVassalScript BMVassalScript;
	public float maxHealth;
	public float vHealth;

	// Use this for initialization
	void Start () {
		BMVassalScript = GetComponent<BMVassalScript> ();
		maxHealth = 10;
	}
	
	// Update is called once per frame
	void Update () {
		vHealth = BMVassalScript.vHealth;
		if (vHealth >= maxHealth) {
			vHealth = maxHealth;
		} else if (vHealth <= 0) {
			vHealth = 0;
		}
		float calcHealth = vHealth / maxHealth;
		SetHealth (calcHealth);
	}

	public void SetHealth(float myHealth){
		HPBar.transform.localScale = new Vector3(myHealth,HPBar.transform.localScale.y, HPBar.transform.localScale.z);
	}
}
