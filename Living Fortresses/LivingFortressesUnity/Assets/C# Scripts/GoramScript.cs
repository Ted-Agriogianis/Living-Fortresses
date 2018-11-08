using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoramScript : MonoBehaviour {
	public Rigidbody2D rb;
	public float moveSpeed;
    public float health, maxHealth, healthPercentage;
    public Image healthBar;
    public GameObject flaps;
    public PowerCellScript footMainCell;
    public List<PowerCellScript> footSecondaryCells;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D> ();
        foreach (PowerCellScript cell in footSecondaryCells){
            maxHealth += cell.cellHealth;
        }
        maxHealth += footMainCell.cellHealth;
        health = maxHealth;
        flaps.GetComponent<Animator>().speed = 0;
        footMainCell.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		rb.MovePosition (transform.position - transform.right * moveSpeed * Time.deltaTime);
        healthPercentage = health / maxHealth;
        healthBar.GetComponent<Image>().fillAmount = healthPercentage;

        if(footSecondaryCells.Count <= 0){
            flaps.GetComponent<Animator>().speed = 1;
            footMainCell.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
	}
}
