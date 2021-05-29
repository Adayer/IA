using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonHealthBar : MonoBehaviour
{
    [SerializeField] bool isPlayerPokemonHP;
    PokemonParent currentPokemonInDisplay;
    Image hpBar;
    private void Start()
    {
        if (!hpBar)
            hpBar = GetComponent<Image>();
        CombatManager cm = FindObjectOfType<CombatManager>();
        
        if (isPlayerPokemonHP)
        {            
            cm.Player.OnPokemonChanged += UpdatePokemonReference;
        }
        else
        {
            cm.Enemy.OnPokemonChanged += UpdatePokemonReference;
        }
    }

    private void OnDestroy()
    {
        CombatManager cm = FindObjectOfType<CombatManager>();
        if (isPlayerPokemonHP)
        {
            cm.Player.OnPokemonChanged -= UpdatePokemonReference;
        }
        else
        {
            cm.Enemy.OnPokemonChanged -= UpdatePokemonReference;
        }
    }
    private void UpdateHealth(int newHealth)
    {
        hpBar.fillAmount = (float)newHealth / currentPokemonInDisplay.MaxHp;
    }

    private void UpdatePokemonReference(PokemonParent newPokemon)
    {
        if (currentPokemonInDisplay != null)
            currentPokemonInDisplay.OnHPChanged -= UpdateHealth;
        currentPokemonInDisplay = newPokemon;
        UpdateHealth(newPokemon.MaxHp);
        currentPokemonInDisplay.OnHPChanged += UpdateHealth;
    }
}
