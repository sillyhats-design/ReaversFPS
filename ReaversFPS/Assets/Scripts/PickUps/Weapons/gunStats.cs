using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public int ammoCount;
    public int magazineCount;
    public int damage;
    public int fireRate;
    public int shootDistance;
    public AudioClip audioClip;
    public GameObject model;
    public GameObject effectFlash;
    public GameObject effectHit;
}
