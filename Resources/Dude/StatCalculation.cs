using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DudeBattler.Scripts;

namespace DudeBattler.Resources.Dude;

public static class StatCalculation
{
    public static int GetMaxHealth(StatBlock stats, Statuses effects)
    {
        var basic = 15;
        var statBonus = (stats.Endurance * 1.5f).RoundDown();
        statBonus += (stats.Strength * 0.5f).RoundDown();
        _ = effects; // TODO: Effects/other
        return basic + statBonus;
    }

    public static int GetMaxEnergy(StatBlock stats, Statuses effects)
    {
        var basic = 20;
        var statBonus = (stats.Endurance * 2f).RoundDown();
        statBonus += (stats.Strength * 0.5f).RoundDown();
        statBonus += (stats.Agility * 0.5f).RoundDown();
        _ = effects; // TODO: Effects/other
        return basic + statBonus;
    }

    public static int GetMaxMana(StatBlock stats, Statuses effects)
    {
        var basic = 15;
        var statBonus = (stats.Mind * 1.5f).RoundDown();
        statBonus += stats.Will;
        _ = effects; // TODO: Effects/other
        return basic + statBonus;
    }
}
