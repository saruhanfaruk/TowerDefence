using TowerDefence.Towers;
using UnityEngine;

namespace TowerDefence.Data
{
    /// <summary>
    /// Data container for defining tower configuration in the game.
    /// </summary>
    [CreateAssetMenu(menuName = "TowerDefence/Tower Data")]
    public class TowerData : ScriptableObject
    {
        [SerializeField] private string towerName;
        [SerializeField] private string prefabAddress;
        [SerializeField] private float health;
        [SerializeField] private float range;
        [SerializeField] private float damage;
        [SerializeField] private float fireRate;
        [SerializeField] private TowerAttackType attackType;
        [SerializeField] private Color projectileColor;

        public string TowerName => towerName;
        public string PrefabAddress => prefabAddress;
        public float Health => health;
        public float Range => range;
        public float Damage => damage;
        public float FireRate => fireRate;
        public TowerAttackType AttackType => attackType;
        public Color ProjectileColor => projectileColor;
    }
}
