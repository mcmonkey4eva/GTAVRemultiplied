using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticScript;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using GTAVRemultiplied.ServerSystem.TagObjects;
using GTA;
namespace GTAVRemultiplied.ServerSystem.TagBases
{
    public class GameTagBase : TemplateTagBase
    {
        public GameTagBase()
        {
            Name = "game";
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
                case "main_player_character":
                    return new CharacterTag(Game.Player.Character).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }
    }
}
