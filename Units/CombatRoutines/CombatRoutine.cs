using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stab_Face.Misc;

namespace Stab_Face.Units.CombatRoutines
{
    public interface CombatRoutine
    {
        CombatRequest getRequest(Player p);
    }
}
