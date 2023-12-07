using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponSO : ScriptableObject {

    public string nameString;
    public float damage;
    public int magazineSize;
    public int maxMagazines;
    public float fireRate;
    public float reloadTime;
    public float bulletsPerShoot;

}
