using Server.Commands;
using Server.Engines.Shadowguard;
using Server.Items;

namespace Server
{
    public static class TimeOfLegends
    {
        public static void Initialize()
        {
            CommandSystem.Register("DecorateTOL", AccessLevel.GameMaster, DecorateTOL_OnCommand);
        }

        [Usage("DecorateTOL")]
        [Description("Generates Time of Legends world decoration.")]
        private static void DecorateTOL_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Generating Time Of Legends world decoration, please wait.");

            Decorate.Generate("tol", "Data/Decoration/TimeOfLegends/TerMur", Map.TerMur);
            Decorate.Generate("tol", "Data/Decoration/TimeOfLegends/Felucca", Map.Felucca);

            ShadowguardController.SetupShadowguard(e.Mobile);

            MacawSpawner.Generate();

            CommandSystem.Handle(e.Mobile, CommandSystem.Prefix + "XmlLoad Spawns/Eodon.xml");

            e.Mobile.SendMessage("Time Of Legends world generating complete.");
        }
    }
}
