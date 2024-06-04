using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;

namespace MicroscopicContentExpansion.NewComponents;
[TypeId("378aa84e70d44cceb01e884e690e623c")]
public class MasterClassLevelGetter : PropertyValueGetter
{

    public BlueprintCharacterClass CharacterClass;

    public override int GetBaseValue(UnitEntityData unit)
    {
        if (!unit.IsPet || unit.Master == null)
        {
            return 0;
        }
        return unit.Master.Progression.GetClassLevel(CharacterClass);
    }
}
