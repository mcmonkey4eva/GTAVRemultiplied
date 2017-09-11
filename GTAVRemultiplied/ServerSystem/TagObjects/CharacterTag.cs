using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticScript;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using GTA;

namespace GTAVRemultiplied.ServerSystem.TagObjects
{
    class CharacterTag : TemplateObject
    {
        public Ped Internal;

        public CharacterTag(Ped chr)
        {
            Internal = chr;
        }

        public static CharacterTag For(TemplateObject obj)
        {
            return (obj is CharacterTag) ? (CharacterTag)obj : null;
        }

        // TODO: Tags!
        /*
        static CharacterTag()
        {
            // TODO: Meta!
            Handlers.Add("vehicle", new TagSubHandler() { Handle = (data, obj) => new VehicleTag(((CharacterTag)obj).Internal.CurrentVehicle), ReturnTypeString = "vehicletag" });
            // Documented in TextTag.
            Handlers.Add("duplicate", new TagSubHandler() { Handle = (data, obj) => new CharacterTag(((CharacterTag)obj).Internal), ReturnTypeString = "charactertag" });
            // Documented in TextTag.
            // TODO: Handlers.Add("type", new TagSubHandler() { Handle = (data, obj) => new TagTypeTag(data.TagSystem.Type_Null), ReturnTypeString = "tagtypetag" });
        }*/

        public override string ToString()
        {
            return "character:" + Internal.Handle;
        }

        public override string GetTagTypeName()
        {
            return "charactertag";
        }
    }
}
