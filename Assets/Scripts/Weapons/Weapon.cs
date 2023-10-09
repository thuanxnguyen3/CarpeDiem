using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Player player;
    public AudioSource audioS;

    [HideInInspector] public Animator anim;
    [HideInInspector] public Slot slotEquippedOn;


    public ItemSO weaponData;
    public bool isAutomatic;

    public Transform shootPoint;
    public LayerMask shootableLayers;

    [Header("Aiming")]
    public float aimSpeed = 10f;
    public Vector3 hipPos;
    public Vector3 aimPos;
    public bool isAiming;

    [HideInInspector] public bool isReloading;
    [HideInInspector] public bool hasTakenOut;

    private float currentFireRate;
    private float fireRate;


    private void Start()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponentInChildren<Animator>();
        audioS = GetComponent<AudioSource>();

        transform.localPosition = hipPos;

        fireRate = weaponData.fireRate;

        currentFireRate = weaponData.fireRate;
    }

    private void Update()
    {
        UpdateAnimation();

        if (weaponData.itemType == ItemSO.ItemType.Weapon)
        {
            if (currentFireRate < fireRate)
                currentFireRate += Time.deltaTime;

            UpdateAiming();

            if (isAutomatic)
            {
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                }
            } else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
        } 
        else if (weaponData.itemType == ItemSO.ItemType.MeleeWeapon)
        {

        }
    }

    #region Fire Weapon Functions

    public void Shoot()
    {
        if (currentFireRate < fireRate || isReloading || hasTakenOut || player.running || slotEquippedOn.stackSize <= 0)
            return;


        RaycastHit hit;

        if(Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponData.range, shootableLayers))
        {
            Debug.Log($"Hitted: {hit.transform.name}");
        }


        anim.CrossFadeInFixedTime("M9 Pistol Shoot", 0.015f);

        GetComponentInParent<CameraLook>().RecoilCamera(Random.Range(-weaponData.horizontalRecoil, weaponData.horizontalRecoil), Random.Range(weaponData.minVerticalRecoil, weaponData.maxVerticalRecoil));

        audioS.PlayOneShot(weaponData.shootSound);

        currentFireRate = 0;

        slotEquippedOn.stackSize--;
        slotEquippedOn.UpdateSlot();
    }


    public void UpdateAiming()
    {
        if (Input.GetButton("Fire2") && !player.running)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimPos, aimSpeed * Time.deltaTime);
        } else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, hipPos, aimSpeed * Time.deltaTime);

        }
    }

    #endregion

    public void UpdateAnimation()
    {
        anim.SetBool("Running", player.running);
    }

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
