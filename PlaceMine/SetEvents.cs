using Smod2.EventHandlers;
using Smod2.Events;
using UnityEngine;
using System.Collections.Generic;
using ServerMod2.API;
using Smod2.API;

namespace PlaceMine
{
    internal class SetEvents : IEventHandler, IEventHandlerCallCommand, IEventHandlerRoundStart, IEventHandlerSetRole, IEventHandlerPlayerPickupItem, IEventHandlerGrenadeExplosion, IEventHandlerWaitingForPlayers
    {
        public SetEvents(MainSettings mainSettings)
        {
            Global.plugin = mainSettings;
            Global.rand = new System.Random();
        }

        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            string command = ev.Command.Split(new char[]
            {
                ' '
            })[0].ToLower();

            if (ev.Command.ToLower().Contains("c4") && ev.Command.ToLower().Contains("list"))
            {
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>() == null)
                {
                    ev.ReturnMessage = Global._isnotholder;
                    return;
                }
                string answer = "";

                for (int i = 1; i <= (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().GlobalPlacedCount; i++)
                {
                    if ((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed.ContainsKey(i))
                    {
                        answer = answer + i.ToString() + ") " + Vector3.Distance((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed[i], (ev.Player.GetGameObject() as GameObject).transform.position).ToString() + "\n";
                    }
                }
                ev.ReturnMessage = Global._C4inventoryanswer + (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().HaveCount.ToString() + "\n" + Global._C4placedanswer + "\n" + answer;
                return;
            }
            else if (ev.Command.ToLower().Contains("place") && ev.Command.ToLower().Contains("mine"))
            {
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>() == null)
                {
                    ev.ReturnMessage = Global._isnotholder;
                    return;
                }
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>().HaveCount <= 0)
                {
                    ev.ReturnMessage = Global._mineisover;
                    return;
                }
                foreach (KeyValuePair<Vector3, Player> kvp in Global.Mines)
                {
                    if (Vector3.Distance((ev.Player.GetGameObject() as GameObject).transform.position, kvp.Key) <= Global.distance_to_defuse)
                    {
                        ev.ReturnMessage = Global._alreadyplacedmineinthisposition;
                        return;
                    }
                }
                (ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>().HaveCount = (ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>().HaveCount - 1;
                (ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>().GlobalPlacedCount = (ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>().GlobalPlacedCount + 1;
                (ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines.Add((ev.Player.GetGameObject() as GameObject).transform.position, (ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>().GlobalPlacedCount);
                Global.Mines.Add((ev.Player.GetGameObject() as GameObject).transform.position, ev.Player);
                Global.plugin.Server.Map.SpawnItem(ItemType.COIN, new Vector((ev.Player.GetGameObject() as GameObject).transform.position.x, (ev.Player.GetGameObject() as GameObject).transform.position.y, (ev.Player.GetGameObject() as GameObject).transform.position.z), new Smod2.API.Vector(0f, 0f, 0f));
                ev.ReturnMessage = Global._placeminesuccess + (ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>().GlobalPlacedCount.ToString();
                return;
            }
            else if (ev.Command.ToLower().Contains("place") && ev.Command.ToLower().Contains("c4"))
            {
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>() == null)
                {
                    ev.ReturnMessage = Global._isnotholder;
                    return;
                }
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().HaveCount <= 0)
                {
                    ev.ReturnMessage = Global._c4isover;
                    return;
                }
                (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().HaveCount = (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().HaveCount - 1;
                (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().GlobalPlacedCount = (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().GlobalPlacedCount + 1;
                (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed.Add((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().GlobalPlacedCount, (ev.Player.GetGameObject() as GameObject).transform.position);
                (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().PlacedCount = (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed.Count;
                ev.ReturnMessage = Global._placeC4success + (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().GlobalPlacedCount;
                return;
            }
            else if (ev.Command.ToLower().Contains("place") && ev.Command.ToLower().Contains("tnt"))
            {
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<TNTHolder>() == null)
                {
                    ev.ReturnMessage = Global._isnotholder;
                    return;
                }
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<TNTHolder>().HaveCount <= 0)
                {
                    ev.ReturnMessage = Global._tntisover;
                    return;
                }
                if (Global.TNTs.ContainsKey(ev.Player.GetPosition().ToVector3()))
                {
                    ev.ReturnMessage = Global._alreadyplaceTNTinthisposition;
                    return;
                }
                (ev.Player.GetGameObject() as GameObject).GetComponent<TNTHolder>().HaveCount = (ev.Player.GetGameObject() as GameObject).GetComponent<TNTHolder>().HaveCount - 1;

                Global.TNTs.Add(ev.Player.GetPosition().ToVector3(), new float[] { Global.time_to_explode_tnt, ev.Player.PlayerId });
                ev.ReturnMessage = Global._placeTNTsuccess + Global.time_to_explode_tnt;
                return;
            }
            else if (command == "detonate")
            {
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>() == null)
                {
                    ev.ReturnMessage = Global._isnotholder;
                    return;
                }
                if (ev.Command.ToLower().Contains("all"))
                {
                    if ((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().PlacedCount <= 0)
                    {
                        ev.ReturnMessage = Global._havenoplacedmines;
                        return;
                    }
                    for (int i = 1; i <= (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().GlobalPlacedCount; i++)
                    {
                        if ((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed.ContainsKey(i))
                        {
                            Global.CustomThrowG((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed[i], ev.Player.GetGameObject() as GameObject, ev.Player.PlayerId);
                            (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed.Remove(i);
                        }
                    }
                    (ev.Player.GetGameObject() as GameObject).AddComponent<RestoreDur>();
                    (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().PlacedCount = (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed.Count;
                    ev.ReturnMessage = Global._successdetonateall;
                    return;
                }
                else if (ev.Command.Split(new char[] { ' ' }).Length > 1)
                {
                    if (!int.TryParse(ev.Command.Split(new char[] { ' ' })[1].ToLower(), out int id))
                    {
                        ev.ReturnMessage = Global._wrongusage;
                        return;
                    }
                    if ((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().PlacedCount <= 0)
                    {
                        ev.ReturnMessage = Global._havenoplacedmines;
                        return;
                    }
                    if (!(ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed.ContainsKey(id))
                    {
                        ev.ReturnMessage = Global._wrongid + id;
                        return;
                    }
                    Global.CustomThrowG((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed[id], ev.Player.GetGameObject() as GameObject, ev.Player.PlayerId);
                    (ev.Player.GetGameObject() as GameObject).AddComponent<RestoreDur>();
                    ev.ReturnMessage = Global._successdetonate1 + id.ToString() + Global._successdetonate2 + Vector3.Distance((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed[id], (ev.Player.GetGameObject() as GameObject).transform.position).ToString();
                    (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed.Remove(id);
                    (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().PlacedCount = (ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>().placed.Count;
                    return;
                }
                else
                {
                    ev.ReturnMessage = Global._wrongusage;
                    return;
                }
            }
            else if (command == "defuse")
            {
                if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP || ev.Player.TeamRole.Team == Smod2.API.Team.SPECTATOR)
                {
                    ev.ReturnMessage = Global._isscporspectator;
                    return;
                }
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>() != null)
                {
                    if (ev.Command.Split(new char[] { ' ' }).Length < 2 || ev.Command.Split(new char[] { ' ' }).Length > 2)
                    {
                        ev.ReturnMessage = Global._wrongusage;
                        return;
                    }
                    if (!int.TryParse(ev.Command.Split(new char[] { ' ' })[1].ToLower(), out int answer))
                    {
                        ev.ReturnMessage = Global._wrongusage;
                        return;
                    }
                    (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().done = true;
                    if (answer == (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().right_answer)
                    {
                        (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().explode = false;
                        ev.ReturnMessage = Global._rightanswer;
                        return;
                    }
                    else
                    {
                        (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().explode = true;
                        ev.ReturnMessage = Global._wronganswer;
                        return;
                    }
                }
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>() != null)
                {
                    if (ev.Command.Split(new char[] { ' ' }).Length < 2 || ev.Command.Split(new char[] { ' ' }).Length > 2)
                    {
                        ev.ReturnMessage = Global._wrongusage;
                        return;
                    }
                    if (!int.TryParse(ev.Command.Split(new char[] { ' ' })[1].ToLower(), out int choose))
                    {
                        ev.ReturnMessage = Global._wrongusage;
                        return;
                    }
                    if (choose == 1)
                    {
                        (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>().progress = 0f;
                        ev.ReturnMessage = Global._successstartdefuse;
                        return;
                    }
                    else if (choose == 2)
                    {
                        (ev.Player.GetGameObject() as GameObject).AddComponent<DefuseTest>();
                        (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().minePos = (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>().minePos;
                        (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().ownerPlayer = (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>().ownerPlayer;

                        (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().first_sum = Global.rand.Next(0, 1000);
                        (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().second_sum = Global.rand.Next(0, 1000);
                        string symbol = " + ";
                        if (Global.rand.Next(0, 2) == 0)
                        {
                            (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().right_answer = (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().first_sum + (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().second_sum;
                        }
                        else
                        {
                            (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().right_answer = (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().first_sum - (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().second_sum;
                            symbol = " - ";
                        }
                        Object.Destroy((ev.Player.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>());
                        ev.ReturnMessage = Global._question + (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().first_sum + symbol + (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>().second_sum + " = ?";
                        return;
                    }
                    else
                    {
                        ev.ReturnMessage = Global._wrongusage;
                        return;
                    }
                }
                if ((ev.Player.GetGameObject() as GameObject).GetComponent<BroadcastLock>() == null)
                {
                    ev.ReturnMessage = Global._toolongtodefuse;
                    return;
                }

                (ev.Player.GetGameObject() as GameObject).AddComponent<DefuseProgressChoose>();
                (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>().minePos = (ev.Player.GetGameObject() as GameObject).GetComponent<BroadcastLock>().position;
                (ev.Player.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>().ownerPlayer = (ev.Player.GetGameObject() as GameObject).GetComponent<BroadcastLock>().owner;

                ev.ReturnMessage = Global._choosedefuse;
                return;
            }
        }

        public void OnGrenadeExplosion(PlayerGrenadeExplosion ev)
        {
            List<Vector3> tempKeys = new List<Vector3>();
            Vector3 point = new Vector3();
            Player owner = null;
            foreach (Grenade grenade in GrenadeManager.grenadesOnScene)
            {
                foreach (KeyValuePair<Vector3, Player> kvp in Global.Mines)
                {
                    if (Vector3.Distance(grenade.transform.position, kvp.Key) <= Global.distance_to_explode_with_grenade)
                    {
                        foreach (Smod2.API.Item item in Global.plugin.Server.Map.GetItems(ItemType.COIN, true))
                        {
                            if (Vector3.Distance(kvp.Key, item.GetPosition().ToVector3()) < Global.distance_to_remove_item)
                            {
                                item.Remove();
                            }
                        }

                        foreach (KeyValuePair<Vector3, int> self_kvp in (kvp.Value.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines)
                        {
                            if (self_kvp.Key == kvp.Key)
                            {
                                kvp.Value.PersonalClearBroadcasts();
                                kvp.Value.PersonalBroadcast(10, "<color=#228b22>Ваша мина была номер " + self_kvp.Value + " задействована</color>", true);
                                (kvp.Value.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines.Remove(kvp.Key);
                                owner = kvp.Value;
                                point = new Vector3(kvp.Key.x, kvp.Key.y, kvp.Key.z);
                                break;
                            }
                        }

                        tempKeys.Add(kvp.Key);
                    }
                }
            }

            foreach (Vector3 key in tempKeys)
            {
                Global.Mines.Remove(key);
            }
            if (point != null && owner != null)
            {
                Global.CustomThrowG(point, Global.plugin.Server.GetPlayers()[0].GetGameObject() as GameObject, owner.PlayerId);
                (Global.plugin.Server.GetPlayers()[0].GetGameObject() as GameObject).AddComponent<RestoreDur>();
            }
        }

        public void OnPlayerPickupItem(PlayerPickupItemEvent ev)
        {
            if (ev.Item.ItemType == ItemType.COIN)
            {
                foreach (KeyValuePair<Vector3, Player> kvp in Global.Mines)
                {
                    if (Vector3.Distance(ev.Item.GetPosition().ToVector3(), kvp.Key) <= Global.distance_to_remove_item)
                    {
                        ev.Allow = false;
                        foreach (Smod2.API.Item item in Global.plugin.Server.Map.GetItems(ItemType.COIN, false))
                        {
                            if (Vector.Distance(item.GetPosition(), ev.Player.GetPosition()) < 3)
                            {
                                item.Remove();
                                Global.plugin.Info("remove");
                                break;
                            }
                        }
                        if (ev.Player.PlayerId == kvp.Value.PlayerId && ev.Player.TeamRole.Role == kvp.Value.TeamRole.Role)
                        {
                            ev.Player.PersonalClearBroadcasts();
                            ev.Player.PersonalBroadcast(10, "<color=#42aaff>Вы обезвредили собственную мину</color>", true);
                        }
                        else
                        {
                            ev.Player.PersonalClearBroadcasts();
                            ev.Player.PersonalBroadcast(10, "<color=#ff0000>Вы подорвались на мине " + Global.Mines[kvp.Key].Name + "</color>", true);
                            foreach (KeyValuePair<Vector3, int> self_kvp in (kvp.Value.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines)
                            {
                                if (self_kvp.Key == kvp.Key)
                                {
                                    kvp.Value.PersonalClearBroadcasts();
                                    kvp.Value.PersonalBroadcast(10, "<color=#228b22>Ваша мина была номер " + self_kvp.Value + " задействована</color>", true);
                                    (kvp.Value.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines.Remove(kvp.Key);
                                    break;
                                }
                            }

                            Global.CustomThrowG(kvp.Key, ev.Player.GetGameObject() as GameObject, Global.Mines[kvp.Key].PlayerId);
                            (ev.Player.GetGameObject() as GameObject).AddComponent<RestoreDur>();
                        }
                        Global.Mines.Remove(kvp.Key);
                        break;
                    }
                }
            }
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            GameObject.FindWithTag("FemurBreaker").AddComponent<CheckPositionGlobal>();
        }

        public void OnSetRole(PlayerSetRoleEvent ev)
        {
            if ((ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>() != null)
            {
                Object.Destroy((ev.Player.GetGameObject() as GameObject).GetComponent<MineHolder>());
            }

            if ((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>() != null)
            {
                Object.Destroy((ev.Player.GetGameObject() as GameObject).GetComponent<C4Holder>());
            }

            if ((ev.Player.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>() != null)
            {
                Object.Destroy((ev.Player.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>());
            }

            if ((ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>() != null)
            {
                Object.Destroy((ev.Player.GetGameObject() as GameObject).GetComponent<DefuseTest>());
            }

            if ((ev.Player.GetGameObject() as GameObject).GetComponent<BroadcastLock>() != null)
            {
                Object.Destroy((ev.Player.GetGameObject() as GameObject).GetComponent<BroadcastLock>());
            }

        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            Global.Mines = new Dictionary<Vector3, Player>();
            Global.TNTs = new Dictionary<Vector3, float[]>();
        }
    }
}