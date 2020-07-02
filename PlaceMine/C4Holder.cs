using UnityEngine;
using System.Collections.Generic;

namespace PlaceMine
{
    class C4Holder : MonoBehaviour
    {
        public int HaveCount = 0;
        public int PlacedCount = 0;
        public Dictionary<int, Vector3> placed = new Dictionary<int, Vector3>();
        public int GlobalPlacedCount = 0;
    }
}
