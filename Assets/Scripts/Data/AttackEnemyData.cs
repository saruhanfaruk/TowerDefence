using System;
using TowerDefence.Enemies;
using UnityEngine;

namespace TowerDefence.Data
{
    [CreateAssetMenu(fileName = "NewAttackEnemyData", menuName = "TowerDefence/Enemies/Attack Enemy Data")]
    public class AttackEnemyData : EnemyData
    {
        public float attackRange;
        public float attackDamage;
        public float attackRate;
        public Color projectileColor;

        /// <summary>
        /// Returns the runtime component type that should be used for this enemy at spawn.
        /// Used to differentiate between attack-type and runner-type behaviors.
        /// </summary>
        public override Type GetRuntimeEnemyComponentType()
        {
            return typeof(AttackEnemy);
        }
    }
}
