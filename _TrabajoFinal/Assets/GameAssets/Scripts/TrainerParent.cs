using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainerParent : MonoBehaviour
{
    [Header("Player")]
    private List<PokemonParent> m_pokemonTeam = new List<PokemonParent>(6);
    private PokemonParent m_currentPickedPokemon;   
    private ActionParent m_actionChosen;
    private CombatManager.ActionType m_typeOfActionChosen;
    [SerializeField] private bool isPlayer;

    [Header("Utilidad")]
    [SerializeField] private Image m_imgPickedPokemon;
    [SerializeField] private GameObject m_parentMovePicker;
    [SerializeField] private GameObject m_parentPokemonPicker;


    //Eventos y Delegados
    public delegate void TMActions();
    public event TMActions OnTM1;
    public event TMActions OnTM2;
    public event TMActions OnTM3;
    public event TMActions OnTM4;

    //Properties
    public CombatManager.ActionType TypeOfActionChosen { get => m_typeOfActionChosen;}
    public ActionParent ActionChosen { get => m_actionChosen; }
    public PokemonParent CurrentPokemonPicked { get => m_currentPickedPokemon; set => m_currentPickedPokemon = value; }

    private void Start()
    {
        InitPokemons();

        if (isPlayer)
        {
            LinkTMButtonsToEvents();
            LinkPokemonChangeButtons();
        }
    }

    #region Metodos de inicialización
    private void InitPokemons()
    {
        object[] possiblePokemons = Resources.LoadAll("Pokemon", typeof(SOPokemonStats));

        List<PokemonParent> possiblePokemonsList = new List<PokemonParent>(0);

        for (int i = 0; i < possiblePokemons.Length; i++)
        {
            GameObject newPkmn = Instantiate(new GameObject("Pokemon"), Vector3.zero, Quaternion.identity, this.transform);
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
            if (m_pokemonTeam.Count == 1)
            {
                UpdatePickedPokemon(m_pokemonTeam[0]);
            }
        }

        for (int i = possiblePokemonsList.Count - 1; i >= 0; i--)
        {
            Destroy(possiblePokemonsList[i].gameObject);
        }
    }
    private void LinkTMButtonsToEvents()
    {
        Button[] attButtons = m_parentMovePicker.GetComponentsInChildren<Button>();

        attButtons[0].onClick.AddListener(TriggerTM1);
    }
    private void LinkPokemonChangeButtons()
    {
        Button[] pkmButtons = m_parentPokemonPicker.GetComponentsInChildren<Button>();

        for (int i = 0; i < pkmButtons.Length; i++)
        {
            pkmButtons[i].gameObject.GetComponent<ChangePokemon>().Pokemon = m_pokemonTeam[i];  
        }

        UpdatePokemonTeam();
    }

    #endregion

    #region TMTriggers
    private void TriggerTM1() { OnTM1.Invoke(); }
    private void TriggerTM2() { OnTM2.Invoke(); }
    private void TriggerTM3() { OnTM3.Invoke(); }
    private void TriggerTM4() { OnTM4.Invoke(); }
    #endregion

    #region Metodos de acción
    public void ChooseAction (ActionParent action, CombatManager.ActionType typeOfAction)
    {
        if(action != null)
        {
            m_typeOfActionChosen = typeOfAction;
            m_actionChosen = action;

            CombatManager.Instance.HasPicked();
        }
        //CHANGE UI
    }

    public void UpdatePickedPokemon(PokemonParent newPickedPkmn)
    {
        if(newPickedPkmn == null && m_currentPickedPokemon == null)
        {
            Debug.LogError("NewPickedPkm is null & CurrentPickedPokemon is null");
            return;
        }

        if(newPickedPkmn == null)
        {
            Debug.LogError("NewPickedPkm is null");
            return;
        }

        if(isPlayer && m_currentPickedPokemon != null)
            m_currentPickedPokemon.DesubscribirTMs();

        m_currentPickedPokemon = newPickedPkmn;
        m_imgPickedPokemon.sprite = newPickedPkmn.Sprite;

        if (isPlayer)
        {
            m_currentPickedPokemon.SubscribirTMs();
            Button[] attButtons = m_parentMovePicker.GetComponentsInChildren<Button>();

            attButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tm1.Name;
            SetUpColors(attButtons[0].gameObject.GetComponent<Image>(), newPickedPkmn.Tm1);
                       
            attButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tm2.Name;
            SetUpColors(attButtons[1].gameObject.GetComponent<Image>(), newPickedPkmn.Tm2);
                        
            attButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tm3.Name;
            SetUpColors(attButtons[2].gameObject.GetComponent<Image>(), newPickedPkmn.Tm3);
                       
            attButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tm4.Name;           
            SetUpColors(attButtons[3].gameObject.GetComponent<Image>(), newPickedPkmn.Tm4);
        }
    }
    
    public void UpdatePokemonTeam()
    {
        Button[] pkmButtons = m_parentPokemonPicker.GetComponentsInChildren<Button>();

        for (int i = 0; i < pkmButtons.Length; i++)
        {
            if(m_pokemonTeam[i] == m_currentPickedPokemon)
            {
                pkmButtons[i].interactable = false;
            }
            else if(m_pokemonTeam[i].CurrentHP >= 0)
            {
                pkmButtons[i].interactable = true;
            }

            pkmButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = m_pokemonTeam[i].Name;
        }
    }
    #endregion

    #region Metodos de Utilidad
    private void SetUpColors(Image img, TMParent tm)
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
    #endregion
}
