using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public Collider hitCollider;
    public bool targetPlayers;
    PersistentData persistent;

    private void Start()
    {
        persistent = FindObjectOfType<PersistentData>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (targetPlayers)
        {
            var player=collision.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                persistent.PlayerDamage(damage);
            }
        }
    }
}
