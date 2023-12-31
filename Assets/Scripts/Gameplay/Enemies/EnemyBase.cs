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
    public Color attackColor=new Color(0f,0f,1f,1f);
    public Color damagedColor = new Color(1f, 0f, 0f, 1f);
    public Color defaultColor = new Color(1f, 1f, 1f, 1f);
    public float turnSpeed;

    protected void SetColor(Color color)
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
    protected bool LookAtPlayer(float maxSpeed)
    {
        Quaternion curRot = transform.rotation;
        transform.LookAt(player.transform);
        bool returnVal = Vector3.Angle(curRot.eulerAngles, new Vector3(transform.rotation.eulerAngles.x,curRot.eulerAngles.y,transform.rotation.eulerAngles.z)) <= 90;
        transform.rotation = Quaternion.RotateTowards(curRot, transform.rotation, maxSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(curRot.x, transform.rotation.eulerAngles.y, curRot.z));
        return returnVal;
    }
    IEnumerator DamageColor()
    {
        SetColor(damagedColor); 
        yield return new WaitForSeconds(1);
        SetColor(defaultColor);
    }

}
