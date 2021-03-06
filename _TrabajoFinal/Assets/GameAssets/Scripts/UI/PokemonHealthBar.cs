using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonHealthBar : MonoBehaviour
{
    [SerializeField] bool isPlayerPokemonHP;
    [SerializeField] private TextMeshProUGUI m_textHP;
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
            cm.OnEnemyTrainerChanged += TrainerChanged;

    }

    private void TrainerChanged(object sender, NewTrainerArgs e)
    {
        UpdatePokemonReference(FindObjectOfType<CombatManager>().Enemy.CurrentPokemonPicked);
        FindObjectOfType<CombatManager>().Enemy.OnPokemonChanged += UpdatePokemonReference;
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
        m_textHP.text = newHealth.ToString();
    }

    private void UpdatePokemonReference(PokemonParent newPokemon)
    {
        if (currentPokemonInDisplay != null)
            currentPokemonInDisplay.OnHPChanged -= UpdateHealth;
        currentPokemonInDisplay = newPokemon;
        UpdateHealth(newPokemon.CurrentHP);
        currentPokemonInDisplay.OnHPChanged += UpdateHealth;
    }
}
