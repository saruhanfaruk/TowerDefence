using TowerDefence.Data;

namespace TowerDefence.Towers
{
    /// <summary>
    /// Interface for defining tower attack behaviors.
    /// Implement different strategies such as single-target, AoE, etc.
    /// </summary>
    public interface ITowerAttackStrategy
    {
        /// <summary>
        /// Called every frame to execute attack logic.
        /// </summary>
        void Tick();

        /// <summary>
        /// Initializes the strategy with the associated tower and its data.
        /// </summary>
        /// <param name="tower">The tower using this strategy.</param>
        /// <param name="towerData">Data describing the tower's attributes.</param>
        void Initialize(TowerBase tower, TowerData towerData);
    }
}
