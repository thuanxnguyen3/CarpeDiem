using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Player player;

    [HideInInspector] public Animator anim;
    [HideInInspector] public Slot slotEquippedOn;


    public ItemSO weaponData;
    [Header("Aiming")]
    public float aimSpeed = 10f;
    public Vector3 hipPos;
    public Vector3 aimPos;
    public bool isAiming;


    private void Start()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponentInChildren<Animator>();

        transform.localPosition = hipPos;
    }

    private void Update()
    {
        if (weaponData.itemType == ItemSO.ItemType.Weapon)
        {
            UpdateAiming();
        } 
        else if (weaponData.itemType == ItemSO.ItemType.MeleeWeapon)
        {

        }
    }

    #region Fire Weapon Functions

    public void UpdateAiming()
    {
        if (Input.GetButton("Fire1") && !player.running)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimPos, aimSpeed * Time.deltaTime);
        } else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, hipPos, aimSpeed * Time.deltaTime);

        }
    }


    #endregion

    public void Equip(Slot slot)
    {
        gameObject.SetActive(true);

        slotEquippedOn = slot;

        transform.localPosition = hipPos;
    }

    public void UnEquip()
    {
        gameObject.SetActive(false);

        slotEquippedOn = null;
    }
}
