using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBarSlider : MonoBehaviour
{
    /*
        This script controls the health bar of the enemy.
    */
    public Slider mainSlider, secondarySlider;
    public float secondarySliderDelay = .33f;
    private float delayTimer = 0;
    public float lingerTime = 2;
    private float lingerTimer = 0;
    public GameObject canvas;
    void Start()
    {
        mainSlider.value = 1;
        secondarySlider.value = 1;
        canvas.SetActive(false);
    }
    public void UpdateHealthBar(float _currHealth, float _maxHealth)
    {
        mainSlider.value = _currHealth / _maxHealth;
        delayTimer = 0;
        canvas.SetActive(true);
        lingerTimer = 0;
    }
    void Update()
    {
        if(secondarySlider.value <= mainSlider.value)
        {
            lingerTimer += Time.deltaTime;
            if(lingerTimer < lingerTime){return;}
            canvas.SetActive(false); return; 
        }
        delayTimer += Time.deltaTime;
        if(delayTimer <= secondarySliderDelay){return;}
        secondarySlider.value -= Time.deltaTime;
    }
}
