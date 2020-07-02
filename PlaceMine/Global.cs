using RemoteAdmin;
using Smod2;
using Smod2.API;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace PlaceMine
{
    class Global
    {
        public static Plugin plugin;
        public static System.Random rand;
        public static float distance_to_explode = 2.0f;
        public static float distance_to_remove_item = 4.0f;
        public static float distance_to_destroy_door = 3.0f;
        public static float distance_to_defuse = 4.5f;
        public static float distance_to_explode_with_grenade = 8.0f;
        
        public static float time_to_choose_defuse = 10.0f;
        public static float time_to_explode_tnt = 15.0f;
        public static float time_to_defuse = 20.0f;


        public static string _wrongusage = "Неправильное использование команды!";

        public static string _isnotholder = "У вас нет детонатора/мин/взрывчатки";

        public static string _mineisover = "У вас закончились мины";
        public static string _c4isover = "У вас закончились C4";
        public static string _tntisover = "У вас закончились TNT";

        public static string _placeC4success = "Вы поставили C4. Его id: ";
        public static string _placeminesuccess = "Вы поставили мину. Её id: ";
        public static string _placeTNTsuccess = "Вы поставили TNT. Он сработает через: ";


        public static string _C4inventoryanswer = "C4 в инвентаре: ";
        public static string _C4placedanswer = "Установленных C4 (номер(id), расстояние): ";

        public static string _successdetonate1 = "Вы взорвали C4 с id: ";
        public static string _successdetonate2 = ", который находился от вас на расстоянии: ";
        public static string _successdetonateall = "Вы взорвали все установленные C4";

        public static string _havenoplacedmines = "У вас нет установленных C4";
        public static string _wrongid = "Не существует C4 с id: ";

        public static string _isscporspectator = "Вы не можете совершить это действие";
        public static string _toolongtodefuse = "Поблизости нет мин";
        public static string _alreadydefuse = "Вы уже обезвреживаете мину";
        public static string _successstartdefuse = "Вы начали обезвреживать мину...";

        public static string _wronganswer = "Вы ошиблись при обезвреживании мины";
        public static string _rightanswer = "Вы успешно обезвредили мину";

        public static string _question = "Вычислите: ";

        //bluyat arham nice idea
        public static string _choosedefuse = "Каким способом вы хотите обезвредить мину?" + "\n" + "1) Быстрым" + "\n" + "2) Наверняка";

        public static string _alreadyplacedmineinthisposition = "Здесь уже стоит мина!";
        public static string _alreadyplaceTNTinthisposition = "Здесь уже стоит TNT!";


        public static Dictionary<Vector3, Player> Mines = new Dictionary<Vector3, Player>();
        public static Dictionary<Vector3, float[]> TNTs = new Dictionary<Vector3, float[]>();
        
        public static Player GetPlayer(string args, out Player playerOut)
        {
            Player player = null;
            int id = -1;
            string name = "";
            try
            {
                id = Convert.ToInt32(args.ToLower());
            }
            catch (FormatException)
            {
                id = -1;
                name = args.ToLower();
            }
            foreach (Player player2 in plugin.Server.GetPlayers())
            {
                if (id == -1)
                {
                    if (player2.Name.ToLower().Contains(name.ToLower()) || player2.Name.ToLower() == name.ToLower())
                    {
                        player = player2;
                    }
                }
                else
                {
                    if (player2.PlayerId == id)
                    {
                        player = player2;
                    }
                }
            }
            playerOut = player;
            return playerOut;
        }


        public static void CustomThrowG(Vector3 position, GameObject gameObject, int killer_id)
        {
            float throwForce = 0f;
            int num = 0;
            GrenadeManager component = gameObject.GetComponent<GrenadeManager>();

            Vector3 forward = new Vector3(Vector.Zero.x, Vector.Zero.y, Vector.Zero.z);
            throwForce = ((!false) ? 1f : 0.5f) * component.availableGrenades[num].throwForce;

            Grenade component2 = UnityEngine.Object.Instantiate(component.availableGrenades[num].grenadeInstance).GetComponent<Grenade>();
            component2.id = gameObject.GetComponent<QueryProcessor>().PlayerId + ":" + (component.smThrowInteger + 4096);
            GrenadeManager.grenadesOnScene.Add(component2);
            component2.SyncMovement(component.availableGrenades[num].GetStartPos(gameObject), (gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward +
                Vector3.up / 4f).normalized * throwForce, Quaternion.Euler(component.availableGrenades[num].startRotation), component.availableGrenades[num].angularVelocity);
            GrenadeManager grenadeManager2 = component;
            GrenadeManager grenadeManager = component;

            int id = num;
            int playerId = killer_id;
            int smThrowInteger = grenadeManager2.smThrowInteger;
            grenadeManager2.smThrowInteger = smThrowInteger + 1;

            grenadeManager.availableGrenades[id].timeUnitilDetonation = 0f;
            grenadeManager.CallRpcThrowGrenade(id, playerId, smThrowInteger + 4096, forward, true, position, false, 0);

        }
    }
}
