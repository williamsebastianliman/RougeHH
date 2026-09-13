using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeadStrike", menuName = "Skills/HeadStrike")]
public class HeadStrikeSkill : Skill
{

    public override void activateSkill()
    {
        stat.enchanceBuff = true;
        stat.bonusDamage += 0.5f;
    }
    public override void deactivateSkill()
    {
        Debug.Log("kepanggil!");
        base.deactivateSkill();
        stat.bonusDamage = 0;
        stat.enchanceBuff = false;
    }
    public override void basicDeactivate()
    {
        stat.bonusDamage -= 0.5f;
        stat.enchanceBuff = false;
    }
}
