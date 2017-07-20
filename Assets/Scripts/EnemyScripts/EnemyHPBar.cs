using UnityEngine;
using System.Collections;

public class EnemyHPBar : MonoBehaviour {

	public GameObject eHPBar;
	public BMEnemyScript BMEnemyScript;
	public float eMaxHP;
	public float eHealth;


	void Start () {
		BMEnemyScript = GetComponent<BMEnemyScript> ();
		eMaxHP = 10;
	}


	void Update () {
		eHealth = BMEnemyScript.eHealth;
		if (eHealth >= eMaxHP) {
			eHealth = eMaxHP;
		} else if (eHealth <= 0) {
			eHealth = 0;
		}
		float eCalcHealth = eHealth / eMaxHP;
		SetHealth (eCalcHealth);
	}


	public void SetHealth(float enemyHealth){
		eHPBar.transform.localScale = new Vector3(enemyHealth,eHPBar.transform.localScale.y, eHPBar.transform.localScale.z);
	}
}
