using RemoteAdmin;
using ServerMod2.API;
using Smod2.API;
using System.Collections.Generic;
using UnityEngine;

namespace PlaceMine
{
    class DefuseTest : MonoBehaviour
    {
        private float timer = 0f;
        private float timeIsUp = 1.0f;

        private float progress = Global.time_to_defuse;
        private Player defuser;
        public Vector3 minePos;
        public Player ownerPlayer;
        public int right_answer;
        public bool explode = false;
        public bool done = false;

        public int first_sum;
        public int second_sum;

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


                if (done)
                {
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
                        foreach (Smod2.API.Item item in Global.plugin.Server.Map.GetItems(ItemType.COIN, true))
                        {
                            if (Vector3.Distance(minePos, item.GetPosition().ToVector3()) < Global.distance_to_remove_item)
                            {
                                item.Remove();
                            }
                        }
                        Global.Mines.Remove(minePos);
                        Global.CustomThrowG(minePos, gameObject, ownerPlayer.PlayerId);
                        gameObject.AddComponent<RestoreDur>();
                        Destroy(gameObject.GetComponent<DefuseTest>());
                    }
                    else
                    {
                        defuser.PersonalClearBroadcasts();
                        defuser.PersonalBroadcast(10, "<color=#42aaff>Вы обезвредили мину" + "</color>", true);
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
                        foreach (Smod2.API.Item item in Global.plugin.Server.Map.GetItems(ItemType.COIN, true))
                        {
                            if (Vector3.Distance(minePos, item.GetPosition().ToVector3()) < Global.distance_to_remove_item)
                            {
                                item.Remove();
                            }
                        }
                        Global.Mines.Remove(minePos);
                        Destroy(gameObject.GetComponent<DefuseTest>());
                    }
                }

                if (progress <= 0f)
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
                    Global.Mines.Remove(minePos);
                    Global.CustomThrowG(minePos, gameObject, ownerPlayer.PlayerId);
                    gameObject.AddComponent<RestoreDur>();
                    Destroy(gameObject.GetComponent<DefuseTest>());
                }

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
                    Global.Mines.Remove(minePos);
                    Global.CustomThrowG(minePos, gameObject, ownerPlayer.PlayerId);
                    gameObject.AddComponent<RestoreDur>();
                    Destroy(gameObject.GetComponent<DefuseTest>());
                }
            }
        }
    }
}
