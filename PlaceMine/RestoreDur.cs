using UnityEngine;
using Smod2.API;
using RemoteAdmin;

namespace PlaceMine
{
    class RestoreDur : MonoBehaviour
    {
        private float timer = 0f;
        private float timeIsUp = 2f;

        public void Update()
        {
            timer = timer + Time.deltaTime;
            if (timer >= timeIsUp)
            {
                CustomNotThrowAndRestoreDur(ItemType.FRAG_GRENADE, true, Vector.Zero, true, new Vector(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), true, 0, gameObject, false);
                Destroy(gameObject.GetComponent<RestoreDur>());
            }
        }

        public void CustomNotThrowAndRestoreDur(ItemType grenadeType, bool isCustomDirection, Vector direction, bool isEnvironmentallyTriggered, Vector position, bool isCustomForce, float throwForce, GameObject player, bool slowThrow = false)
        {
            int num = 0;
            if (grenadeType != ItemType.FRAG_GRENADE)
            {
                if (grenadeType == ItemType.FLASHBANG)
                {
                    num = 1;
                }
            }
            else
            {
                num = 0;
            }
            GrenadeManager component = player.GetComponent<GrenadeManager>();
            Vector3 forward = player.GetComponent<Scp049PlayerScript>().plyCam.transform.forward;
            if (isCustomDirection)
            {
                forward = new Vector3(direction.x, direction.y, direction.z);
            }
            if (!isCustomForce)
            {
                throwForce = ((!slowThrow) ? 1f : 0.5f) * component.availableGrenades[num].throwForce;
            }
            Grenade component2 = UnityEngine.Object.Instantiate<GameObject>(component.availableGrenades[num].grenadeInstance).GetComponent<Grenade>();
            component2.id = player.GetComponent<QueryProcessor>().PlayerId + ":" + (component.smThrowInteger + 4096);
            GrenadeManager.grenadesOnScene.Add(component2);
            component2.SyncMovement(component.availableGrenades[num].GetStartPos(player), (player.GetComponent<Scp049PlayerScript>().plyCam.transform.forward +
                Vector3.up / 4f).normalized * throwForce, Quaternion.Euler(component.availableGrenades[num].startRotation), component.availableGrenades[num].angularVelocity);
            GrenadeManager grenadeManager2 = component;
            GrenadeManager grenadeManager = component;

            int id = num;
            int playerId = player.GetComponent<QueryProcessor>().PlayerId;
            int smThrowInteger = grenadeManager2.smThrowInteger;
            grenadeManager2.smThrowInteger = smThrowInteger + 1;

            grenadeManager.availableGrenades[id].timeUnitilDetonation = 3f;
        }
    }
}
