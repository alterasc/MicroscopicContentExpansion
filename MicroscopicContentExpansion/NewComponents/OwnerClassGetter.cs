using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;

namespace MicroscopicContentExpansion.NewComponents {
    public class OwnerClassGetter : PropertyValueGetter {

        public BlueprintCharacterClass CharacterClass;

        public override int GetBaseValue(UnitEntityData unit) {
            if (!unit.IsPet || unit.Master == null) {
                return 0;
            }
            return unit.Master.Progression.GetClassLevel(CharacterClass);
        }
    }
}
