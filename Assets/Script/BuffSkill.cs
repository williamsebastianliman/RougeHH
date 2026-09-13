using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Skills/Buff")]
public class BuffSkill : Skill
{
    public override void activateSkill()
    {
        base.activateSkill();
        stat.isBuff = true;
        stat.bonusDamage += 0.2f;
        stat.bonusDefense += 0.2f;
    }
    public override void deactivateSkill()
    {
        base.deactivateSkill();
        stat.isBuff = false;
        stat.bonusDamage -= 0.2f;
        stat.bonusDefense -= 0.2f;
    }
}
