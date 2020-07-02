using Smod2.API;
using UnityEngine;
using RemoteAdmin;
using ServerMod2.API;
using System.Collections.Generic;

namespace PlaceMine
{
    class DefuseProgressChoose : MonoBehaviour
    {
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        private Player defuser;

        public float progress = Global.time_to_choose_defuse;
        public Vector3 minePos;
        public Player ownerPlayer;

        public void Start()
        {
            foreach (Player p in Global.plugin.Server.GetPlayers())
            {
                if (p.PlayerId == gameObject.GetComponent<QueryProcessor>().PlayerId)
                {
                    defuser = p;
                    break;
                }
            }
        }

        public void Update()
        {
            timer = timer + Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                progress = progress - timeIsUp;

                if (Vector3.Distance(minePos, gameObject.transform.position) > Global.distance_to_defuse)
                {
                    defuser.PersonalClearBroadcasts();
                    defuser.PersonalBroadcast(10, "<color=#ff0000>Вы подорвались на мине " + ownerPlayer.Name + "</color>", true);

                    foreach (KeyValuePair<Vector3, int> self_kvp in (ownerPlayer.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines)
                    {
                        if (self_kvp.Key == minePos)
                        {
                            ownerPlayer.PersonalClearBroadcasts();
                            ownerPlayer.PersonalBroadcast(10, "<color=#228b22>Ваша мина была номер " + self_kvp.Value + " задействована" + "</color>", true);
                            (ownerPlayer.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines.Remove(minePos);
                            break;
                        }
                    }

                    foreach (Smod2.API.Item item in Global.plugin.Server.Map.GetItems(ItemType.COIN, true))
                    {
                        if (Vector3.Distance(minePos, item.GetPosition().ToVector3()) < Global.distance_to_remove_item)
                        {
                            item.Remove();
                        }
                    }
                    Global.CustomThrowG(minePos, gameObject, ownerPlayer.PlayerId);
                    Global.Mines.Remove(minePos);
                    gameObject.AddComponent<RestoreDur>();
                    Destroy(gameObject.GetComponent<DefuseProgressChoose>());
                }

                if (progress <= 0f)
                {
                    bool explode = false;
                    if (defuser.PlayerId == ownerPlayer.PlayerId && defuser.TeamRole.Role == ownerPlayer.TeamRole.Role)
                    {
                        explode = false;
                    }
                    else if (defuser.TeamRole.Team == Smod2.API.Team.CLASSD)
                    {
                        if (Global.rand.Next(0, 10) > 4)
                        {
                            explode = true;
                        }
                    }
                    else if (defuser.TeamRole.Team == Smod2.API.Team.SCIENTIST)
                    {
                        if (Global.rand.Next(0, 10) > 5)
                        {
                            explode = true;
                        }
                    }
                    else if (defuser.TeamRole.Role == Role.FACILITY_GUARD)
                    {
                        if (Global.rand.Next(0, 100) > 94)
                        {
                            explode = true;
                        }
                    }
                    else if (defuser.TeamRole.Team == Smod2.API.Team.NINETAILFOX)
                    {
                        if (Global.rand.Next(0, 100) > 97)
                        {
                            explode = true;
                        }
                    }
                    else if (defuser.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY)
                    {
                        if (Global.rand.Next(0, 100) > 97)
                        {
                            explode = true;
                        }
                    }
                    foreach (Smod2.API.Item item in Global.plugin.Server.Map.GetItems(ItemType.COIN, true))
                    {
                        if (Vector3.Distance(minePos, item.GetPosition().ToVector3()) < Global.distance_to_remove_item)
                        {
                            item.Remove();
                        }
                    }
                    if (explode)
                    {
                        defuser.PersonalClearBroadcasts();
                        defuser.PersonalBroadcast(10, "<color=#ff0000>Вы подорвались на мине " + ownerPlayer.Name + "</color>", true);
                        foreach (KeyValuePair<Vector3, int> self_kvp in (ownerPlayer.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines)
                        {
                            if (self_kvp.Key == minePos)
                            {
                                ownerPlayer.PersonalClearBroadcasts();
                                ownerPlayer.PersonalBroadcast(10, "<color=#228b22>Ваша мина была номер " + self_kvp.Value + " задействована" + "</color>", true);
                                (ownerPlayer.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines.Remove(minePos);
                                break;
                            }
                        }
                        Global.CustomThrowG(minePos, gameObject, ownerPlayer.PlayerId);
                        gameObject.AddComponent<RestoreDur>();
                    }
                    else
                    {
                        defuser.PersonalClearBroadcasts();
                        defuser.PersonalBroadcast(10, "<color=#42aaff>Вы обезвредили мину." + "</color>", true);
                        foreach (KeyValuePair<Vector3, int> self_kvp in (ownerPlayer.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines)
                        {
                            if (self_kvp.Key == minePos)
                            {
                                ownerPlayer.PersonalClearBroadcasts();
                                ownerPlayer.PersonalBroadcast(10, "<color=#228b22>Ваша мина была номер " + self_kvp.Value + " обезврежена" + "</color>", true);
                                (ownerPlayer.GetGameObject() as GameObject).GetComponent<MineHolder>().selfMines.Remove(minePos);
                                break;
                            }
                        }
                    }
                    Global.Mines.Remove(minePos);
                    Destroy(gameObject.GetComponent<DefuseProgressChoose>());

                }
            }
            
        }

        public void OnDestroy()
        {
            Destroy(gameObject.GetComponent<BroadcastLock>());
        }
    }
}

