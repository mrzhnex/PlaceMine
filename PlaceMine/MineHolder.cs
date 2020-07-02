using UnityEngine;
using System.Collections.Generic;

namespace PlaceMine
{
    class MineHolder : MonoBehaviour
    {
        public int HaveCount = 0;
        public int GlobalPlacedCount = 0;
        public Dictionary<Vector3, int> selfMines = new Dictionary<Vector3, int>(); //остановился здесь
    }
}
