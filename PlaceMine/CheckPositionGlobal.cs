using UnityEngine;
using System.Collections.Generic;
using Smod2.API;
using ServerMod2.API;

namespace PlaceMine
{
    class CheckPositionGlobal : MonoBehaviour
    {
        private float timer = 0f;
        private readonly float timeIsUp = 0.5f;

        public void Update()
        {
            timer = timer + Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                foreach (Player curplayer in Global.plugin.Server.GetPlayers())
                {
                    if (curplayer.TeamRole.Team == Smod2.API.Team.SPECTATOR)
                    {
                        continue;
                    }
                    if ((curplayer.GetGameObject() as GameObject).GetComponent<DefuseProgressChoose>() != null)
                    {
                        continue;
                    }
                    if ((curplayer.GetGameObject() as GameObject).GetComponent<DefuseTest>() != null)
                    {
                        continue;
                    }
                    foreach (KeyValuePair<Vector3, Player> kvp in Global.Mines)
                    {
                        if (Vector3.Distance(curplayer.GetPosition().ToVector3(), kvp.Key) < Global.distance_to_explode)
                        {
                            if (curplayer.TeamRole.Role == kvp.Value.TeamRole.Role && curplayer.PlayerId == kvp.Value.PlayerId)
                            {
                                if ((curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>() == null)
                                {
                                    curplayer.PersonalClearBroadcasts();
                                    curplayer.PersonalBroadcast(7, "<color=#228b22>Вы замечаете странное устройство (похожее на мину) рядом с вашими ногами..." + "</color>", true);
                                    (curplayer.GetGameObject() as GameObject).AddComponent<BroadcastLock>();
                                    (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().position = kvp.Key;
                                    (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().owner = kvp.Value;
                                }
                                continue;
                            }
                            if (curplayer.TeamRole.Team != kvp.Value.TeamRole.Team)
                            {
                                if (curplayer.TeamRole.Role == Role.FACILITY_GUARD && mtf.Contains(kvp.Value.TeamRole.Role))
                                {
                                    if ((curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>() == null)
                                    {
                                        curplayer.PersonalClearBroadcasts();
                                        curplayer.PersonalBroadcast(7, "<color=#228b22>Вы замечаете странное устройство (похожее на мину) рядом с вашими ногами..." + "</color>", true);
                                        (curplayer.GetGameObject() as GameObject).AddComponent<BroadcastLock>();
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().position = kvp.Key;
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().owner = kvp.Value;
                                    }
                                    continue;
                                }
                            }
                            else
                            {
                                if (curplayer.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY && kvp.Value.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY)
                                {
                                    if ((curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>() == null)
                                    {
                                        curplayer.PersonalClearBroadcasts();
                                        curplayer.PersonalBroadcast(7, "<color=#228b22>Вы замечаете странное устройство (похожее на мину) рядом с вашими ногами..." + "</color>", true);
                                        (curplayer.GetGameObject() as GameObject).AddComponent<BroadcastLock>();
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().position = kvp.Key;
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().owner = kvp.Value;
                                    }
                                    continue;
                                }
                                if (mtf.Contains(curplayer.TeamRole.Role) && mtf.Contains(kvp.Value.TeamRole.Role))
                                {
                                    if ((curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>() == null)
                                    {
                                        curplayer.PersonalClearBroadcasts();
                                        curplayer.PersonalBroadcast(7, "<color=#228b22>Вы замечаете странное устройство (похожее на мину) рядом с вашими ногами..." + "</color>", true);
                                        (curplayer.GetGameObject() as GameObject).AddComponent<BroadcastLock>();
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().position = kvp.Key;
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().owner = kvp.Value;
                                    }
                                    continue;
                                }
                                if (curplayer.TeamRole.Role == Role.FACILITY_GUARD && mtf.Contains(kvp.Value.TeamRole.Role))
                                {
                                    if ((curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>() == null)
                                    {
                                        curplayer.PersonalClearBroadcasts();
                                        curplayer.PersonalBroadcast(7, "<color=#228b22>Вы замечаете странное устройство (похожее на мину) рядом с вашими ногами..." + "</color>", true);
                                        (curplayer.GetGameObject() as GameObject).AddComponent<BroadcastLock>();
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().position = kvp.Key;
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().owner = kvp.Value;
                                    }
                                    continue;
                                }
                                if (curplayer.TeamRole.Role == Role.FACILITY_GUARD && kvp.Value.TeamRole.Role == Role.FACILITY_GUARD)
                                {
                                    if ((curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>() == null)
                                    {
                                        curplayer.PersonalClearBroadcasts();
                                        curplayer.PersonalBroadcast(7, "<color=#228b22>Вы замечаете странное устройство (похожее на мину) рядом с вашими ногами..." + "</color>", true);
                                        (curplayer.GetGameObject() as GameObject).AddComponent<BroadcastLock>();
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().position = kvp.Key;
                                        (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().owner = kvp.Value;
                                    }
                                    continue;
                                }
                            }

                            curplayer.PersonalClearBroadcasts();
                            curplayer.PersonalBroadcast(10, "<color=#ff0000>Вы подорвались на мине " + kvp.Value.Name + "</color>", true);
                            foreach (KeyValuePair<Vector3, int> self_kvp in (kvp.Value.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines)
                            {
                                if (self_kvp.Key == kvp.Key)
                                {
                                    kvp.Value.PersonalClearBroadcasts();
                                    kvp.Value.PersonalBroadcast(10, "<color=#228b22>Ваша мина была номер " + self_kvp.Value + " задействована" + "</color>", true);
                                    (kvp.Value.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines.Remove(kvp.Key);
                                    break;
                                }
                            }
                            foreach (Smod2.API.Item item in Global.plugin.Server.Map.GetItems(ItemType.COIN, true))
                            {
                                if (Vector3.Distance(kvp.Key, item.GetPosition().ToVector3()) < Global.distance_to_remove_item)
                                {
                                    item.Remove();
                                }
                            }
                            Global.CustomThrowG(kvp.Key, curplayer.GetGameObject() as GameObject, kvp.Value.PlayerId);

                            (curplayer.GetGameObject() as GameObject).AddComponent<RestoreDur>();
                            Global.Mines.Remove(kvp.Key);
                        }
                        else if (Vector3.Distance(curplayer.GetPosition().ToVector3(), kvp.Key) < Global.distance_to_defuse)
                        {
                            if ((curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>() == null)
                            {
                                curplayer.PersonalClearBroadcasts();
                                curplayer.PersonalBroadcast(7, "<color=#228b22>Вы замечаете странное устройство (похожее на мину) рядом с вашими ногами..." + "</color>", true);
                                (curplayer.GetGameObject() as GameObject).AddComponent<BroadcastLock>();
                                (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().position = kvp.Key;
                                (curplayer.GetGameObject() as GameObject).GetComponent<BroadcastLock>().owner = kvp.Value;
                            }
                        }
                    }

                }

                foreach (KeyValuePair<Vector3, float[]> kvp in Global.TNTs)
                {
                    Global.TNTs[kvp.Key][0] = Global.TNTs[kvp.Key][0] - timeIsUp;

                    if (Global.TNTs[kvp.Key][0] <= 0f)
                    {
                        foreach (Smod2.API.Door door in Global.plugin.Server.Map.GetDoors())
                        {
                            if (Vector3.Distance(kvp.Key, door.Position.ToVector3()) < Global.distance_to_destroy_door)
                            {
                                door.Destroyed = true;                              
                            }
                        }
                        Global.CustomThrowG(kvp.Key, Global.plugin.Server.GetPlayers()[0].GetGameObject() as GameObject, int.Parse(Global.TNTs[kvp.Key][0].ToString()));
                        (Global.plugin.Server.GetPlayers()[0].GetGameObject() as GameObject).AddComponent<RestoreDur>();
                        Global.TNTs.Remove(kvp.Key);
                    }
                }
            }
        }

        private List<Role> mtf = new List<Role>()
        {
            Role.NTF_CADET,
            Role.NTF_COMMANDER,
            Role.NTF_LIEUTENANT,
            Role.NTF_SCIENTIST
        };

    }
}
