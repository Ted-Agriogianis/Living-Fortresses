using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcelloRigScript : MonoBehaviour {
    public MarcelloScript marcello;
    public float timer;
    public bool startTimer;
	// Use this for initialization
	void Start () {
        marcello = gameObject.transform.parent.GetComponent<MarcelloScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AttackForward(){
        if(marcello.moveAttack == false){
            marcello.moveAttack = true;
            marcello.newAttackLocation = new Vector2(marcello.transform.position.x + marcello.attackSpeed * -marcello.xDirection, marcello.transform.position.y);
        }else{
            marcello.moveAttack = false;
        }
    }
    public void AttackBack()
    {
        if (marcello.moveAttack == false)
        {
            marcello.moveAttack = true;
            marcello.newAttackLocation = new Vector2(marcello.transform.position.x - marcello.attackSpeed * -marcello.xDirection, marcello.transform.position.y);
        }
        else
        {
            marcello.moveAttack = false;
        }
    }
    public void AttackUp()
    {
        if (marcello.moveAttack == false)
        {
            marcello.moveAttack = true;
            marcello.newAttackLocation = new Vector2(marcello.transform.position.x, marcello.transform.position.y + marcello.attackSpeed);
        }
        else
        {
            marcello.moveAttack = false;
        }
    }
    public void AttackDown()
    {
        if (marcello.moveAttack == false)
        {
            marcello.moveAttack = true;
            marcello.newAttackLocation = new Vector2(marcello.transform.position.x, marcello.transform.position.y - marcello.attackSpeed);
        }
        else
        {
            marcello.moveAttack = false;
        }
    }
    public void SwapAttackState(){
        if (startTimer == true)
        {
            timer = 0;
            startTimer = false;
        }
        timer = 0;
        if (timer <= 0)
        {
            marcello.continuingAttack = false;
            marcello.attackNumber += 1;
            timer = .2f;
            startTimer = true;
        }
        timer -= Time.deltaTime;
    }
    public void ResetAttack(){
        marcello.attackNumber = 0;
    }
}
