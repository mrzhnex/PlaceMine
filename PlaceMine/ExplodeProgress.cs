using RemoteAdmin;
using Smod2.API;
using System.Collections.Generic;
using UnityEngine;

namespace PlaceMine
{
    class ExplodeProgress : MonoBehaviour
    {
        private float timer = 0;
        private readonly float timeIsUp = 1.0f;
        public float progress = Global.time_to_explode;
        private Player exploder;
        public Vector3 minePos;
        public GameObject ownerMine;
        public bool silent = true;

        public void Start()
        {        
            foreach (Player p in Global.plugin.Server.GetPlayers())
            {
                if (p.PlayerId == gameObject.GetComponent<QueryProcessor>().PlayerId)
                {
                    exploder = p;
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
                if (!silent)
                {
                    progress = progress - timeIsUp;
                }
                if (Vector3.Distance(minePos, gameObject.transform.position) > Global.distance_to_mine)
                {
                    Destroy(gameObject.GetComponent<ExplodeProgress>());
                }

                if (progress <= 0f)
                {
                    if (!is_ghost_mine)
                    {
                        ownerMine.GetComponent<MineHolder>().placedMines.Remove(id_mine);
                        ownerMine.AddComponent<RestoreDur>();
                    }
                    else
                    {
                        Global.Mines.Remove(minePos);
                    }
                    Global.CustomThrowG(minePos, gameObject, ownerMine.GetComponent<QueryProcessor>().PlayerId);
                    Destroy(gameObject.GetComponent<ExplodeProgress>());
                }
            }
           
        }
    }
}
