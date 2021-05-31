using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerTrainer : TrainerParent
{
    public Action OnInitializePlayer;
    bool iSInitializing = true;

    public override void Initialize(List<SOPokemonStats> pokemonStats)
    {
        base.Initialize(pokemonStats);
        LinkTMButtonsToEvents();
        LinkPokemonChangeButtons();
        OnInitializePlayer?.Invoke();
        CurrentPokemonPicked.OnPokemonPlayerFainted += ButtonsUninteractableAttacks;
        OnPokemonChanged += SetUpButtonInteractivity;
        
    }

    private void SetUpButtonInteractivity(PokemonParent newPokemon)
    {
        ButtonsInteractableAttacks();
        newPokemon.OnPokemonPlayerFainted += ButtonsUninteractableAttacks;
    }

    private void LinkTMButtonsToEvents()
    {
        Button[] attButtons = m_parentMovePicker.GetComponentsInChildren<Button>();

        attButtons[0].onClick.AddListener(TriggerTM1);
        attButtons[1].onClick.AddListener(TriggerTM2);
        attButtons[2].onClick.AddListener(TriggerTM3);
        attButtons[3].onClick.AddListener(TriggerTM4);
    }

    private void LinkPokemonChangeButtons()
    {
        Button[] pkmButtons = m_parentPokemonPicker.GetComponentsInChildren<Button>();

        for (int i = 0; i < pkmButtons.Length; i++)
        {
            pkmButtons[i].gameObject.GetComponent<ChangePokemon>().Pokemon = m_pokemonTeam[i];
            SetUpColorsPkmn(pkmButtons[i].GetComponent<Image>(), m_pokemonTeam[i]);
        }

        UpdatePokemonTeam();
    }

    public override void UpdatePickedPokemon(PokemonParent newPickedPkmn)
    {
        if (CurrentPokemonPicked != null)
        {
            CurrentPokemonPicked.OnPokemonPlayerFainted -= CombatManager.Instance.StopActPlayerFainted;
        }

        base.UpdatePickedPokemon(newPickedPkmn);

        m_currentPickedPokemon.SubscribirTMs();
        Button[] attButtons = m_parentMovePicker.GetComponentsInChildren<Button>();

        attButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tms[0].Name;
        SetUpColorsTMs(attButtons[0].gameObject.GetComponent<Image>(), newPickedPkmn.Tms[0]);

        attButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tms[1].Name;
        SetUpColorsTMs(attButtons[1].gameObject.GetComponent<Image>(), newPickedPkmn.Tms[1]);

        attButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tms[2].Name;
        SetUpColorsTMs(attButtons[2].gameObject.GetComponent<Image>(), newPickedPkmn.Tms[2]);

        attButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tms[3].Name;
        SetUpColorsTMs(attButtons[3].gameObject.GetComponent<Image>(), newPickedPkmn.Tms[3]);
        if(!iSInitializing)
        {
            ButtonsUninteractableAttacks();
        }

        CurrentPokemonPicked.OnPokemonPlayerFainted += CombatManager.Instance.StopActPlayerFainted;
    }

    public void UpdatePokemonTeam()
    {
        Button[] pkmButtons = m_parentPokemonPicker.GetComponentsInChildren<Button>();
        if (iSInitializing)
        {
            InteractableChangePokemonButtons();
            iSInitializing = false;
        }
        for (int i = 0; i < pkmButtons.Length; i++)
        {
            pkmButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = m_pokemonTeam[i].Name;
        }
    }

    public void UninteractableChangePokemonButtons()
    {
        ToggleButtonsInteractableChangePkmn(false);
    }

    public void InteractableChangePokemonButtons()
    {
        ToggleButtonsInteractableChangePkmn(true);
    }

    private void ToggleButtonsInteractableChangePkmn(bool newState)
    {
        Button[] pkmButtons = m_parentPokemonPicker.GetComponentsInChildren<Button>();
        for (int i = 0; i < pkmButtons.Length; i++)
        {
            if(m_pokemonTeam[i] == m_currentPickedPokemon)
            {
                pkmButtons[i].interactable = false;
            }
            else if (pkmButtons[i].GetComponent<ChangePokemon>().Pokemon.CurrentHP > 0 && newState)
                pkmButtons[i].interactable = newState;
            else
            {
                pkmButtons[i].interactable = false;
            }
        }
    }

    internal void HealAllPokemon()
    {
        for (int i = 0; i < m_pokemonTeam.Count; i++)
        {
            m_pokemonTeam[i].Heal(9999);
        }
    }

    private void ButtonsUninteractableAttacks()
    {
        ToggleButtonsInteractableAttacks(false);
        CurrentPokemonPicked.OnPokemonPlayerFainted -= ButtonsUninteractableAttacks;
    }

    private void ButtonsInteractableAttacks()
    {
        ToggleButtonsInteractableAttacks(true);
    }

    private void ToggleButtonsInteractableAttacks(bool newState)
    {
        Button[] attButtons = m_parentMovePicker.GetComponentsInChildren<Button>();
        for (int i = 0; i < attButtons.Length; i++)
        {
            attButtons[i].interactable = newState;
        }
    }


    public void DisableUI()
    {
        ButtonsUninteractableAttacks();
        UninteractableChangePokemonButtons();
    }
    public void DisableChangePokemonButtons()
    {
        UninteractableChangePokemonButtons();        
    }
    public void EnableChangePokemonButtons()
    {
        InteractableChangePokemonButtons();        
    }

    public void EnableUI()
    {
        if (CurrentPokemonPicked.CurrentHP > 0)
            ButtonsInteractableAttacks();

        InteractableChangePokemonButtons();
    }

}
