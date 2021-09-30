using System.Collections.Generic;
using RPG.Stats;

public interface IModifierProvider
{
    IEnumerable<float> GetAdditiveModifier(StatClass stat);
}
