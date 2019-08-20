using Scenes.FightScene.HPDisplay.DisplayData;

namespace Scenes.FightScene.HPDisplay {
    public interface IHPDataAdjustable {
        /// <summary>
        /// Adjust self according to health data
        /// </summary>
        void SetHealthData(HPDisplayData healthDisplayData);
    }
}