using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/New Data", order = 0)]
    public class PlayerData : ScriptableObject
    {

        public float PHealth;
        public float PCalories;
        public bool isHurt;
    }
 

}
