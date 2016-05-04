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
    class VehicleTag : TemplateObject
    {
        public Vehicle Internal;

        public VehicleTag(Vehicle veh)
        {
            Internal = veh;
        }

        public static VehicleTag For(TemplateObject obj)
        {
            return (obj is VehicleTag) ? (VehicleTag)obj : null;
        }

        /// <summary>
        /// All tag handlers for this tag type.
        /// </summary>
        public static Dictionary<string, TagSubHandler> Handlers = new Dictionary<string, TagSubHandler>();

        static VehicleTag()
        {
            // TODO: Meta!
            Handlers.Add("vehicle_window_tint_integer", new TagSubHandler() { Handle = (data, obj) => new IntegerTag((int)((VehicleTag)obj).Internal.WindowTint), ReturnTypeString = "integertag" });
            // Documented in TextTag.
            Handlers.Add("duplicate", new TagSubHandler() { Handle = (data, obj) => new VehicleTag(((VehicleTag)obj).Internal), ReturnTypeString = "vehicletag" });
            // Documented in TextTag.
            // TODO: Handlers.Add("type", new TagSubHandler() { Handle = (data, obj) => new TagTypeTag(data.TagSystem.Type_Null), ReturnTypeString = "tagtypetag" });
        }

        public override TemplateObject Handle(TagData data)
        {
            if (data.Remaining == 0)
            {
                return this;
            }
            TagSubHandler handler;
            if (Handlers.TryGetValue(data[0], out handler))
            {
                return handler.Handle(data, this).Handle(data.Shrink());
            }
            return new TextTag(ToString()).Handle(data);
        }

        public override string ToString()
        {
            return "vehicle:" + Internal.Handle;
        }
    }
}
