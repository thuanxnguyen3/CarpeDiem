using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public bool isDead;

    public bool isPaused;

    [Header("Player Stats")]
    public float health;
    public float maxHealth = 100f;
    public float hunger;
    public float maxHunger = 100f;
    public float thirst;
    public float maxThirst = 100f;

    [Header("Stats Depletion")] 
    public float hungerDepletion = 0.5f;
    public float thirstDepletion = 0.75f;

    [Header("Stats Damages")]
    public float hungerDmg = 1.5f;
    public float thirstDmg = 2.25f;

    [Header("UI")]
    public StatsBar healthBar; 
    public StatsBar hungerBar;
    public StatsBar thirstBar;

    private void Start()
    {
        health = maxHealth;
        hunger = maxHunger;
        thirst = maxThirst;
    }

    private void Update()
    {
        if (isPaused)
            return;

        if (isDead)
            return;

        UpdateStats();
        UpdateUI();
        

    }

    private void UpdateStats()
    {
        if (health <= 0)
            health = 0;
        if (health >= maxHealth)
            health = maxHealth;

        if (hunger <= 0)
            hunger = 0;
        if (hunger >= maxHunger)
            hunger = maxHunger;

        if (thirst <= 0)
            thirst = 0;
        if (thirst >= maxThirst)
            thirst = maxThirst;

        // Damages
        if (hunger <= 0)
            health -= hungerDmg * Time.deltaTime;
        else if (hunger <= 25)
            health -= (hungerDmg * 0.75f) * Time.deltaTime;
        else if (hunger < 50)
            health -= (hungerDmg * 0.5f) * Time.deltaTime;


        if (thirst <= 0)
            health -= thirstDmg * Time.deltaTime;
        else if (thirst <= 25)
            health -= (thirstDmg * 0.75f) * Time.deltaTime;
        else if (thirst < 50)
            health -= (thirstDmg * 0.5f) * Time.deltaTime;

        // Depletions
        if (hunger > 0)
            hunger -= hungerDepletion * Time.deltaTime;

        if (thirst > 0)
            thirst -= thirstDepletion * Time.deltaTime;

        // Regen
        if (hunger >= 75 && thirst >= 75)
        {
            health += 10f * Time.deltaTime;
        } 
        else if (hunger >= 50 && thirst >= 50)
        {
            health += 5f * Time.deltaTime;
        }
    }

    private void UpdateUI()
    {
        healthBar.numberText.text = health.ToString("f0");
        healthBar.bar.fillAmount = health / 100;

        hungerBar.numberText.text = hunger.ToString("f0");
        hungerBar.bar.fillAmount = hunger / 100;

        thirstBar.numberText.text = thirst.ToString("f0");
        thirstBar.bar.fillAmount = thirst / 100;

    }



}
