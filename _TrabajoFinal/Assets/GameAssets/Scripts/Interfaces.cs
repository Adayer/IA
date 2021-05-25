using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interfaces 
{
    public interface IDamageable
    {
        void TakeDamage();
        void Heal();
    }

    public interface IConsumible
    {
        void Use();
    }
}
