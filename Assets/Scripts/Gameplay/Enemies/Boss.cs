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
    public float targetDist;
    public float distBuffer = 0.5f;
    public CharacterController characterController;
    bool isAttacking = false;

    // Update is called once per frame
    void Update()
    {
        if (cooldown <= 0&&!isAttacking)
        {
            int newAttack=UnityEngine.Random.Range(0,attacks.Count);
            StartCoroutine(Attack(attacks[newAttack]));
        }
        else if (!isAttacking)
        {
            cooldown -= Time.deltaTime;
        }

        LookAtPlayer(turnSpeed*Time.deltaTime);
        if(Vector3.Distance(transform.position, player.transform.position) > targetDist+distBuffer)
        {
            characterController.Move(transform.forward*Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < targetDist - distBuffer)
        {
            characterController.Move(transform.forward * -1 * Time.deltaTime);
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
        public Color projectileColor;

        public Collider meleeCollider;
    }

    IEnumerator Attack(BossAttack attack)
    {
        isAttacking = true;
        targetDist = (attack.maxDist + attack.minDist) / 2.0f;
        //yield return new WaitUntil(() => Vector3.Distance(transform.position, player.transform.position) > attack.minDist && Vector3.Distance(transform.position, player.transform.position) < attack.maxDist);
        SetColor(attackColor);
        yield return new WaitForSeconds(attack.delay);
        for (int i=0;i<attack.repeats;i++)
        {
            if(attack.isRanged)
            {
                var spawnPos = transform.position;
                spawnPos.y = player.transform.position.y;
                Projectile spawnedProjectile=Instantiate(projectile, spawnPos, transform.rotation);
                spawnedProjectile.speed = attack.rangedSpeed;
                spawnedProjectile.damage = attack.damage;
                //spawnedProjectile.transform.localScale *= attack.rangedSize;
                spawnedProjectile.GetComponent<MeshRenderer>().material.color = attack.projectileColor;
            }

            yield return new WaitForSeconds(attack.repeatDelay);
        }
        SetColor(defaultColor);
        cooldown = attack.cooldown;
        isAttacking = false;
    }
}
