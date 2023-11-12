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
    bool collided = false;
    GameObject parent;

    private void Start()
    {
        persistent = FindObjectOfType<PersistentData>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (!collided)
        {
            collided = true;
            parent = other.gameObject;
            return;
        }
        if(other.gameObject==parent)
        {
            return;
        }
        if (targetPlayers)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                persistent.PlayerDamage(damage);
                
            }
        }
        Destroy(gameObject);
    }
}
