using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : EnemyBase
{ 
    public List<BossAttack> attacks;
    public float cooldown;
    public Projectile projectile;
    public float turnSpeed;


    // Update is called once per frame
    void Update()
    {
        if (cooldown <= 0)
        {

        }
    }

    [Serializable]
    public struct BossAttack
    {
        public float delay;
        public float cooldown;
        public float repeats;
        public float repeatDelay;

        public float damage;

        public float maxDist;
        public float minDist;
        public bool isRanged;

        public float rangedSpeed;
        public float rangedSize;

        public Collider meleeCollider;
    }

    IEnumerator Attack(BossAttack attack)
    {
        SetColor(attackColor);
        yield return new WaitForSeconds(attack.delay);
        while (attack.repeats > 0)
        {
            if(attack.isRanged)
            {
                Projectile spawnedProjectile=Instantiate(projectile, Vector3.zero, Quaternion.identity);
                projectile.speed = attack.rangedSpeed;
                projectile.damage = attack.damage;
                projectile.transform.localScale *= attack.rangedSize;
            }
        }
        SetColor(defaultColor);
    }
}
