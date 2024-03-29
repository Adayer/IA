using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using CleverCrow.Fluid.BTs.Samples;

public class HealingPotion : ActionParent, Interfaces.IConsumible
{
    public static Action OnPlayerUsedPotion;
    public static Action OnIAPotionUsed;

    private void Start()
    {
        if (this.GetComponent<EnemyTrainerIA>() == null)
        {
            GetComponent<Button>().onClick.AddListener(() => CombatManager.Instance.Player.ChooseAction(this, CombatManager.ActionType.UseItem)/*StartCoroutine(Effect(CombatManager.Instance.Player))*/);
            GetComponentInChildren<TextMeshProUGUI>().text = "Use healing potion (" + CombatManager.Instance.Player.Potions + " left)";
        }
    }
    public override IEnumerator Effect(TrainerParent trainer)
    {
        CombatManager.Instance.Player.DisableUI();
        print("Se ha curado a "+ trainer.CurrentPokemonPicked.Name + " toda la vida.");
        yield return new WaitForSeconds(0.5f);
        if (trainer is PlayerTrainer)
        {
            ((PlayerTrainer)trainer).UpdatePokemonTeam();
        }
        ;
        Use(trainer);
    }

    public void Use(TrainerParent trainer)
    {
        if (trainer is PlayerTrainer)
        {
            CombatManager.Instance.Player.CurrentPokemonPicked.Heal(9999);
            OnPlayerUsedPotion?.Invoke();
            GetComponentInChildren<TextMeshProUGUI>().text = "Use healing potion (" + CombatManager.Instance.Player.Potions + " left)";
        }
        else
        {
            CombatManager.Instance.Enemy.CurrentPokemonPicked.Heal(9999);
            OnIAPotionUsed?.Invoke();
        }
    }

    public override void Act()
    {
        CombatManager.Instance.Player.ChooseAction(this, tipoAccion);
        print("Used healing potion.");
    }
}
