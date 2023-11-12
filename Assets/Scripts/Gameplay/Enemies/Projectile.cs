using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public Collider hitCollider;
    public bool targetPlayers;
    PersistentData persistent;
    bool collided = false;
    public float lifeTime;
    GameObject parent;

    private void Start()
    {
        persistent = FindObjectOfType<PersistentData>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if(lifeTime > 0)
        {
            lifeTime += Time.deltaTime;
        }
        if (lifeTime >= 10)
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.name.StartsWith("Wall"))
        {
            lifeTime = 1;
            return;
        }
        if (!collided)
        {
            collided = true;
            parent = other.gameObject;
            return;
        }
        if(other.gameObject==parent||other.GetComponent<Projectile>()!=null)
        {
            return;
        }
        if (targetPlayers)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                persistent.PlayerDamage(damage, player.gameObject);
                
            }
        }
        Destroy(gameObject);
    }
}
