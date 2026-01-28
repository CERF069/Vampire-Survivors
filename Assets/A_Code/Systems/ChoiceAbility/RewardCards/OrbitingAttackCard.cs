using A_Code.Attack.Type.OrbitingAttack;
using A_Code.Systems.ChoiceAbility;
using UnityEngine;
using Zenject;

public class OrbitingAttackCard : CardBase
{
    [Inject] private OrbitAttackSystem _orbitAttack;

    protected override void OnSelect()
    {
        if (_orbitAttack == null)
        {
            Debug.LogError("OrbitingAttackCard: OrbitAttackSystem не инжектирован!");
            return;
        }

        _orbitAttack.Enable();
    }
}