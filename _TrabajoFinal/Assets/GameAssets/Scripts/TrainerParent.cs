using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainerParent : MonoBehaviour
{
    private PokemonParent m_currentPokemonPicked;    

    private ActionParent m_actionChosen;
    private CombatManager.ActionType m_typeOfActionChosen;

    [SerializeField] private List<PokemonParent> m_pokemonTeam = new List<PokemonParent>(6);
    [SerializeField] private bool isPlayer;

    [SerializeField] private Image m_imgPickedPokemon;
    [SerializeField] private GameObject m_parentMovePicker;

    public CombatManager.ActionType TypeOfActionChosen { get => m_typeOfActionChosen;}
    public ActionParent ActionChosen { get => m_actionChosen; }
    public PokemonParent CurrentPokemonPicked { get => m_currentPokemonPicked; set => m_currentPokemonPicked = value; }

    private void Start()
    {
        object[] possiblePokemons = Resources.LoadAll("Pokemon", typeof(SOPokemonStats));

        List<PokemonParent> possiblePokemonsList = new List<PokemonParent> (0);

        for (int i = 0; i < possiblePokemons.Length; i++)
        {
            GameObject newPkmn = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity, this.transform);
            PokemonParent cmp = newPkmn.AddComponent<PokemonParent>();
            cmp.SetProperties((SOPokemonStats)possiblePokemons[i]);
            possiblePokemonsList.Add(cmp);
            newPkmn.name = cmp.Name;
        }

        while(m_pokemonTeam.Count < 6)
        {
            int random = Random.Range(0, possiblePokemonsList.Count);
            m_pokemonTeam.Add(possiblePokemonsList[random]);
            possiblePokemonsList.RemoveAt(random);
            if(m_pokemonTeam.Count == 1)
            {
                UpdatePickedPokemon(m_pokemonTeam[0]);
            }
            UpdatePokemonTeam();
        }

        for (int i = possiblePokemonsList.Count - 1; i >= 0 ; i--)
        {
            Destroy(possiblePokemonsList[i].gameObject);
        }
    }

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
        m_imgPickedPokemon.sprite = newPickedPkmn.Sprite;
        if (isPlayer)
        {
            Button[] attButtons = m_parentMovePicker.GetComponentsInChildren<Button>();

            attButtons[0].onClick.AddListener(()=> ChooseAction(newPickedPkmn.Tm1, CombatManager.ActionType.Attack));
            attButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tm1.Name;
            SetUpColors(attButtons[0].gameObject.GetComponent<Image>(), newPickedPkmn.Tm1);
            attButtons[1].onClick.AddListener(()=> ChooseAction(newPickedPkmn.Tm2, CombatManager.ActionType.Attack));
            attButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tm2.Name;
            SetUpColors(attButtons[1].gameObject.GetComponent<Image>(), newPickedPkmn.Tm2);
            attButtons[2].onClick.AddListener(()=> ChooseAction(newPickedPkmn.Tm3, CombatManager.ActionType.Attack));
            attButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tm3.Name;
            SetUpColors(attButtons[2].gameObject.GetComponent<Image>(), newPickedPkmn.Tm3);
            attButtons[3].onClick.AddListener(()=> ChooseAction(newPickedPkmn.Tm4, CombatManager.ActionType.Attack));
            attButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = newPickedPkmn.Tm4.Name;
            SetUpColors(attButtons[3].gameObject.GetComponent<Image>(), newPickedPkmn.Tm4);
            SetUpColors(attButtons[3].gameObject.GetComponent<Image>(), newPickedPkmn.Tm4);
        }
    }
    private void SetUpColors(Image img, TMParent tm)
    {
        switch (tm.TipoDeAtaque)
        {
            case AppConstants.TipoPokemon.Normal:
                {
                    img.color = Color.gray;
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

    public void UpdatePokemonTeam()
    {

    }
}
