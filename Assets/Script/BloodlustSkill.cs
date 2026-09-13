using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bloodlust", menuName = "Skills/Bloodlust")]
public class BloodlustSkill : Skill
{
    public override void activateSkill()
    {
        base.activateSkill();
        stat.isLifesteal = true;
        stat.lifestealAmount = 20;
    }
    public override void deactivateSkill()
    {
        base.deactivateSkill();
        stat.isLifesteal = false;
        stat.lifestealAmount = 0;
    }
}
