using System;
using Scenes.FightScene.HPDisplay.DisplayData;
using Shared.Behaviours.Progressable.CubicProgressable;
using UnityEngine;

namespace Scenes.FightScene.HPDisplay.HPBar {
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class HPBar : CubicProgressable, IHPDataAdjustable {
        private MeshRenderer meshRenderer;

        public void SetHealthData(HPDisplayData healthDisplayData) {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.materials = new[] {healthDisplayData.material};
            var tr = transform;
            tr.localPosition = new Vector3(0, tr.localScale.y * healthDisplayData.order, 0);
        }
    }
}