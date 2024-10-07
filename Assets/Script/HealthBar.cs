using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    Vector3 localScale;

    void Start()
    {
        localScale = transform.localScale;
    }

        public void SetHealth(int health)
    {
        slider.value = health;
        //normalizedValue means from 0-1
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void Flip()
    {
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
