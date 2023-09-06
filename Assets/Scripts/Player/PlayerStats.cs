using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
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
        UpdateStats();
        UpdateUI();
    }

    private void UpdateStats()
    {
        StatsLimit(health, maxHealth);
        StatsLimit(hunger, maxHunger);
        StatsLimit(thirst, maxThirst);

        // Damages
        if (hunger <= 0)
            health -= hungerDmg * Time.deltaTime;

        if (thirst <= 0)
            health -= thirstDmg * Time.deltaTime;

        // Depletions
        if (hunger > 0)
            hunger -= hungerDepletion * Time.deltaTime;

        if (thirst > 0)
            thirst -= thirstDepletion * Time.deltaTime;
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

    private void StatsLimit(float stat, float maxStat)
    {
        if (stat <= 0)
            stat = 0;

        if (stat >= maxStat)
            stat = maxStat;
    }



}
