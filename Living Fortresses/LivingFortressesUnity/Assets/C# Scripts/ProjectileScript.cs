using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public Rigidbody2D rb;
    public float speed, lifeTime, damage;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        lifeTime = 5;
	}
	
	// Update is called once per frame
	void Update () {
        rb.MovePosition(transform.position + transform.up * -1 * speed * Time.deltaTime);
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0){
            Destroy(gameObject);
        }
	}
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Marcello" && other.gameObject.GetComponent<MarcelloScript>().hitTimer <= 0){
            if(other.GetComponent<MarcelloScript>().hitTimer <= 0)
            other.GetComponent<MarcelloScript>().health -= damage;
            other.GetComponent<MarcelloScript>().hitTimer = other.GetComponent<MarcelloScript>().hitTimerMax;
        }
        Destroy(gameObject);
    }
}
