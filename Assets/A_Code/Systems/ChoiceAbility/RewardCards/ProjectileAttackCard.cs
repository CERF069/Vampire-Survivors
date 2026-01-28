using A_Code.Attack.Type.ProjectileAttack;
using A_Code.Systems.ChoiceAbility;
using UnityEngine;
using Zenject;

public class ProjectileAttackCard : CardBase
{
    [Inject] private ProjectileAttackSystem _projectileAttack;

    protected override void OnSelect()
    {
        if (_projectileAttack == null)
        {
            Debug.LogError("ProjectileAttackCard: ProjectileAttackSystem не инжектирован!");
            return;
        }

        _projectileAttack.Enable();
    }
}