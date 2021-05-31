using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class TrainerParent : MonoBehaviour
{
    [Header("Player")]
    protected List<PokemonParent> m_pokemonTeam = new List<PokemonParent>(6);
    protected PokemonParent m_currentPickedPokemon;
    protected ActionParent m_actionChosen;
    protected CombatManager.ActionType m_typeOfActionChosen;
    [SerializeField] protected bool isPlayer;

    [Header("Utilidad")]
    [SerializeField] protected Image m_imgPickedPokemon;
    [SerializeField] protected GameObject m_parentMovePicker;
    [SerializeField] protected GameObject m_parentPokemonPicker;
    

    //Eventos y Delegados
    public delegate void TMActions();
    public event TMActions OnTM0;
    public event TMActions OnTM1;
    public event TMActions OnTM2;
    public event TMActions OnTM3;

    //Properties
    public CombatManager.ActionType TypeOfActionChosen { get => m_typeOfActionChosen; }
    public ActionParent ActionChosen { get => m_actionChosen; }
    public PokemonParent CurrentPokemonPicked
    {
        get => m_currentPickedPokemon;
        set
        {
            m_currentPickedPokemon = value;
            //SetUpColorsPkmn(m_imgPickedPokemon, value);
            OnPokemonChanged?.Invoke(value);
        }
    }

    public List<PokemonParent> PokemonTeam { get => m_pokemonTeam; }

    public Action<PokemonParent> OnPokemonChanged;

    public virtual void Initialize(List<SOPokemonStats> pokemonStats)
    {
        InitPokemons(pokemonStats);

    }

    #region Metodos de inicialización
    public void InitPokemons(List<SOPokemonStats> pokemonStats)
    {
        object[] possiblePokemons;
        if (m_pokemonTeam == null)
            m_pokemonTeam = new List<PokemonParent>();
        if (pokemonStats != null)
        {
            possiblePokemons = pokemonStats.ToArray();
           
        }
        else
        {
            possiblePokemons = Resources.LoadAll("Pokemon", typeof(SOPokemonStats));
        }

        List<PokemonParent> possiblePokemonsList = new List<PokemonParent>(0);

        for (int i = 0; i < possiblePokemons.Length; i++)
        {
            GameObject newPkmn = new GameObject("Pokemon");
            newPkmn.transform.parent = this.transform;
            PokemonParent cmp = newPkmn.AddComponent<PokemonParent>();
            cmp.SetProperties((SOPokemonStats)possiblePokemons[i]);
            possiblePokemonsList.Add(cmp);
            newPkmn.name = cmp.Name;
        }

        while (m_pokemonTeam.Count < 6)
        {
            int random = Random.Range(0, possiblePokemonsList.Count);
            m_pokemonTeam.Add(possiblePokemonsList[random]);
            possiblePokemonsList.RemoveAt(random);
            //if (m_pokemonTeam.Count == 1)
            //{
            //    UpdatePickedPokemon(m_pokemonTeam[0]);
            //}
        }

        UpdatePickedPokemon(m_pokemonTeam[0]);

        //OnPokemonChanged?.Invoke(m_pokemonTeam[0]);
        for (int i = possiblePokemonsList.Count - 1; i >= 0; i--)
        {
            Destroy(possiblePokemonsList[i].gameObject);
        }
    }



    #endregion

    #region TMTriggers
    protected void TriggerTM1() { OnTM0?.Invoke(); }
    protected void TriggerTM2() { OnTM1?.Invoke(); }
    protected void TriggerTM3() { OnTM2?.Invoke(); }
    protected void TriggerTM4() { OnTM3?.Invoke(); }
    #endregion

    #region Metodos de acción
    public void ChooseAction(ActionParent action, CombatManager.ActionType typeOfAction) // Pasar a Clase abstracta
    {
        if (action != null)
        {
            m_typeOfActionChosen = typeOfAction;
            m_actionChosen = action;

            if (isPlayer)
                CombatManager.Instance.HasPicked();
        }
        //CHANGE UI
    }

    public virtual void UpdatePickedPokemon(PokemonParent newPickedPkmn)
    {
        //Debug.LogError(newPickedPkmn.Name);
        if (!m_imgPickedPokemon)
            m_imgPickedPokemon = GameObject.FindGameObjectWithTag("EnemyPlayerImg").GetComponent<Image>();
        if (newPickedPkmn == null && m_currentPickedPokemon == null)
        {
            Debug.LogError("NewPickedPkm is null & CurrentPickedPokemon is null");
            return;
        }

        if (newPickedPkmn == null)
        {
            Debug.LogError("NewPickedPkm is null");
            return;
        }


        if (isPlayer)
        {
            if(CurrentPokemonPicked != null)
            {
                m_currentPickedPokemon.DesubscribirTMs();
            }
            m_imgPickedPokemon.sprite = newPickedPkmn.Sprite;
        }
        else if(!isPlayer)
        {
            m_imgPickedPokemon.sprite = newPickedPkmn.Sprite2;
        }

        CurrentPokemonPicked = newPickedPkmn;
    } 


    #endregion

    public void SetPokemons(List<PokemonParent> pokemons)
    {
        m_pokemonTeam = pokemons;
    }

    #region Metodos de Utilidad
    protected void SetUpColorsTMs(Image img, TMParent tm)
    {
        switch (tm.TipoDeAtaque)
        {
            case AppConstants.TipoPokemon.Normal:
                {
                    img.color = Color.white;
                }
                break;
            case AppConstants.TipoPokemon.Fire:
                {
                    img.color = Color.red;
                }
                break;
            case AppConstants.TipoPokemon.Water:
                {
                    img.color = Color.cyan;
                }
                break;
            case AppConstants.TipoPokemon.Grass:
                {
                    img.color = Color.green;
                }
                break;
            default:
                break;
        }
    }

    protected void SetUpColorsPkmn(Image img, PokemonParent pkm)
    {
        switch (pkm.Type)
        {
            case AppConstants.TipoPokemon.Normal:
                {
                    img.color = Color.white;
                }
                break;
            case AppConstants.TipoPokemon.Fire:
                {
                    img.color = Color.red;
                }
                break;
            case AppConstants.TipoPokemon.Water:
                {
                    img.color = Color.cyan;
                }
                break;
            case AppConstants.TipoPokemon.Grass:
                {
                    img.color = Color.green;
                }
                break;
            default:
                break;
        }
    }
    #endregion
}
