using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;

    private int startingStamina = 3;
    private int maxStamina;

    protected override void Awake()
    {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = maxStamina;
    }

}
