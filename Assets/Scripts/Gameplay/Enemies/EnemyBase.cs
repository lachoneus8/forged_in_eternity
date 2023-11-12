using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController player;
    public float health;
    public float maxHealth;
    public float speed;
    public Collider attackCollider;
    public Color attackColor=new Color(0f,0f,1f,1f);
    public Color damagedColor = new Color(1f, 0f, 0f, 1f);
    public Color defaultColor = new Color(1f, 1f, 1f, 1f);

    protected void SetColor(Color color)
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }

    IEnumerator DamageColor()
    {
        SetColor(damagedColor); 
        yield return new WaitForSeconds(1);
        SetColor(defaultColor);
    }

}
