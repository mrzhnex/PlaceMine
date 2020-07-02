using Smod2.API;
using Smod2.Commands;
using UnityEngine;

namespace PlaceMine
{
    internal class MineCommand : ICommandHandler
    {
        public string GetCommandDescription()
        {
            return "description";
        }

        public string GetUsage()
        {
            return "mine <id/nick> <add> <count> | mine <id/nick> <remove>";
        }

        public string[] OnCall(ICommandSender sender, string[] args)
        {
            if (args.Length < 3)
            {
                return new string[] { "Out of args. Usage: " + GetUsage() };
            }

            Player target = Global.GetPlayer(args[0], out target);
            if (target == null)
            {
                return new string[] { "Player not found" };
            }
            if (target.TeamRole.Team == Smod2.API.Team.SCP || target.TeamRole.Team == Smod2.API.Team.SPECTATOR)
            {
                return new string[] { target.Name + " is scp or spectator" };
            }
            if (args[1] == "remove")
            {
                if ((target.GetGameObject() as GameObject).GetComponent<MineHolder>() == null)
                {
                    return new string[] { target.Name + " have no mines" };
                }
                Object.Destroy((target.GetGameObject() as GameObject).GetComponent<MineHolder>());
                target.PersonalClearBroadcasts();
                target.PersonalBroadcast(10, "<color=#228b22>Вы обнаруживаете, что у вас закончились МИНЫ</color>", true);
                return new string[] { "removed all mines from " + target.Name };
            }
            else if (args[1] == "add")
            {
                if (!int.TryParse(args[2], out int count))
                {
                    return new string[] { "Wrong args. Usage: " + GetUsage() };
                }
                if ((target.GetGameObject() as GameObject).GetComponent<MineHolder>() == null)
                {
                    (target.GetGameObject() as GameObject).AddComponent<MineHolder>();
                }
                (target.GetGameObject() as GameObject).GetComponent<MineHolder>().HaveCount = (target.GetGameObject() as GameObject).GetComponent<MineHolder>().HaveCount + count;
                target.PersonalClearBroadcasts();
                target.PersonalBroadcast(10, "<color=#228b22>Вам были выданы МИНЫ в количестве " + count.ToString() + " штук</color>", true);
                return new string[] { "add " + count.ToString() + " mines to " + target.Name };

            }
            else
            {
                return new string[] { "Wrong args. Usage: " + GetUsage() };
            }
        }
    }
}