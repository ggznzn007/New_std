using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Hawkeye,
    Blackwidow,
    IronMan
}

public class WeaponManager : MonoBehaviour
{
    public GameObject _hawkeye;
    public GameObject _blackwidow;
    public GameObject _ironman;

    private GameObject myWeapon;

    // ������
    private IWeapon weapon;

    private void setWeaponType(WeaponType weaponType)
    {

        Component c = gameObject.GetComponent<IWeapon>() as Component;  // ���� ���� ������Ʈ�� IWeapon Ÿ���� ������Ʈ�� �����´�.

        if (c != null)
        {
            Destroy(c);
        }

        switch (weaponType)
        {
            case WeaponType.Blackwidow:
                weapon = gameObject.AddComponent<Blackwidow>();
                myWeapon = _blackwidow;
                break;

            case WeaponType.IronMan:
                weapon = gameObject.AddComponent<IronMan>();
                myWeapon = _ironman;
                break;

            case WeaponType.Hawkeye:
                weapon = gameObject.AddComponent<Hawkeye>();
                myWeapon = _hawkeye;
                break;

            default:
                weapon = gameObject.AddComponent<Blackwidow>();
                myWeapon = _blackwidow;
                break;
        }
    }

    void Start()
    {
        setWeaponType(WeaponType.Blackwidow);
    }

    public void ChangeBlackwidow()
    {
        setWeaponType(WeaponType.Blackwidow);
    }

    public void ChangeIronMan()
    {
        setWeaponType(WeaponType.IronMan);
    }

    public void ChangeHawkeye()
    {
        setWeaponType(WeaponType.Hawkeye);
    }

    public void Fire()
    {
        weapon.Shoot(myWeapon);
    }
}
