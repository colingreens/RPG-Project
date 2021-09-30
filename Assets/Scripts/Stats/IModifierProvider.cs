using System.Collections.Generic;
using RPG.Stats;

public interface IModifierProvider
{
    IEnumerable<float> GetAdditiveModifiers(StatClass stat);

    IEnumerable<float> GetPercentageModifiers(StatClass stat);
}
