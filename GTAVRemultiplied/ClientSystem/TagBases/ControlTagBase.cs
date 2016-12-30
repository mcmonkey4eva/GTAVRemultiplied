using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticScript;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using GTA;
using GTA.Native;

namespace GTAVRemultiplied.ClientSystem.TagBases
{
    public class ControlTagBase : TemplateTagBase
    {
        public ControlTagBase()
        {
            Name = "control";
        }

        public static bool ControlDown(Control ctrl)
        {
            return Game.IsControlPressed(0, ctrl) || Game.IsControlPressed(2, ctrl);
        }

        public override TemplateObject Handle(TagData data)
        {
            data.Shrink();
            if (data.Remaining == 0)
            {
                return new TextTag(ToString());
            }
            switch (data[0])
            {
                case "game_button_down":
                    {
                        Control ctrl = (Control)Enum.Parse(typeof(Control), data.GetModifier(0), true);
                        return new BooleanTag(ControlDown(ctrl)).Handle(data.Shrink());
                    }
                    // TODO: Move this to a proper gtav general tag.
                case "debug_hash":
                    {
                        return new IntegerTag(Function.Call<uint>(Hash.GET_HASH_KEY, data.GetModifier(0)));
                    }
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }
    }
}
