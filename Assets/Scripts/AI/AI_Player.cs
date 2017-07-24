using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SB;

[RequireComponent(typeof(Animator))]
public class AI_Player : Unit {

    protected Animator animator;
	public Weapon weapon;
	public Bullet bullet;

    protected void Awake () {
		base.Awake();
        animator = GetComponent<Animator>();
    }

    protected void OnEnable() {
		base.OnEnable();
	}
	
	// Use this for initialization
	override public void OnDamage(ObjectProperty skill)	{
        property.health -= skill.tb.attack_power;
        Debug.LogFormat("On Damage({0}) : {1}/{2} ", skill.tb.attack_power, property.health, property.tb.max_health);
        // call die 
        if (property.health <= 0)
            OnDie(skill);
        else
            SetAnimationTrigger("damaged");


    }
    override public void OnAttack(ObjectProperty target) {
	}

	override public void OnDie(ObjectProperty attacker) {
		GameManager.instance.GameOver();
        this.DisposeForPool();
    }

    void SetAnimationTrigger(string arg)
    {
        Debug.Assert(null != animator, "animator is null " + this.GetType().Name);
        if (animator)
            animator.SetTrigger(arg);
    }

    // Update is called once per frame
    void Update () {
		HUDDevDisplay.instance.Show("HP",property.health.ToString("0.0"));
		
	}
}
