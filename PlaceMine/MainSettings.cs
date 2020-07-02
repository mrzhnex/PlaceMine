using Smod2;
using Smod2.Attributes;

namespace PlaceMine
{
    [PluginDetails(
       author = "Innocence",
       description = "description",
       id = "place.mine",
       name = "PlaceMine",
       configPrefix = "cpm",
       SmodMajor = 3,
       SmodMinor = 0,
       SmodRevision = 0,
       version = "4.4.B.B.3"
   )]
    public class MainSettings : Plugin
    {
        public override void Register()
        {
            AddEventHandlers(new SetEvents(this));
            AddCommand("mine", new MineCommand());
            AddCommand("c4", new C4Command());
            AddCommand("tnt", new TNTCommand());
        }

        public override void OnEnable()
        {
            Info(Details.name + " on");
        }

        public override void OnDisable()
        {
            Info(Details.name + " off");
        }
    }
}
