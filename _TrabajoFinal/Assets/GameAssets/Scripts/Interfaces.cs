using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interfaces 
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
        void Heal(int amount);
    }

    public interface IConsumible
    {
        void Use();
    }
}
