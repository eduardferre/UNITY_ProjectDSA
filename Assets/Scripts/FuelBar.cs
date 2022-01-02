using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{

    public Slider slider;

    public void setFuel(float fuel)
    {
        slider.value = fuel;
    }

    public void setMaxFuel(float fuel)
    {
        slider.maxValue = fuel;
        slider.value = fuel;
    }
}
