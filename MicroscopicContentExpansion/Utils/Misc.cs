using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroscopicContentExpansion.Utils {
    internal class Misc {

        public static ContextDurationValue Instant() {
            return new ContextDurationValue() {
                m_IsExtendable = true,
                DiceCountValue = new ContextValue(),
                BonusValue = new ContextValue()
            };
        }
    }
}
