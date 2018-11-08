using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCellScript : MonoBehaviour {
    public Shader spriteDefault, whiteSprite;
    public GoramScript fortress;
    public GameObject healthBar;
    public Vector2 healthBaseSize;
    public float cellHealth, cellHealthMax, cellHealthPercentage, hitTimer, hitTimerMax;
    public bool isMainCell;
    public MarcelloScript marcello;
    public string bodyPart;
	// Use this for initialization
	void Start () {
        cellHealth = cellHealthMax;
        healthBaseSize = healthBar.transform.localScale;
        whiteSprite = Shader.Find("GUI/Text Shader");
        spriteDefault = Shader.Find("Sprites/Default");
        marcello = GameObject.Find("Marcello Object").GetComponent<MarcelloScript>();
    }
	
	// Update is called once per frame
	void Update () {
        cellHealthPercentage = cellHealth / cellHealthMax;
        healthBar.transform.localScale = new Vector2(cellHealthPercentage * healthBaseSize.x, healthBar.transform.localScale.y);
        if(cellHealth <= 0){
            if(fortress.footSecondaryCells.Contains(this)){
                fortress.footSecondaryCells.Remove(this);
            }
            Destroy(gameObject);
        }
        hitTimer -= Time.deltaTime;
        if(hitTimer < hitTimerMax - .1f){
            gameObject.GetComponent<SpriteRenderer>().material.shader = spriteDefault;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
	}
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player Sword" && hitTimer <= 0){
            cellHealth -= marcello.swordDamage;
            fortress.health -= marcello.swordDamage;
            hitTimer = hitTimerMax;
            gameObject.GetComponent<SpriteRenderer>().material.shader = whiteSprite;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if(other.gameObject.tag == "Player Projectile" && hitTimer <= 0){
            cellHealth -= marcello.gunDamage;
            fortress.health -= marcello.gunDamage;
            hitTimer = hitTimerMax;
            gameObject.GetComponent<SpriteRenderer>().material.shader = whiteSprite;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
