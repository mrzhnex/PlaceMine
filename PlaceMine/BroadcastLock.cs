using UnityEngine;
using Smod2.API;

namespace PlaceMine
{
    class BroadcastLock : MonoBehaviour
    {
        private float timer = 0f;
        private float timeIsUp = 0.3f;

        public Vector3 position;
        public Player owner;

        public void Update()
        {
            timer = timer + Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                if (Vector3.Distance(gameObject.transform.position, position) > Global.distance_to_defuse)
                {
                    Destroy(gameObject.GetComponent<BroadcastLock>());
                }
            }
        }
    }
}
