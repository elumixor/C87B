using Character;
using Character.HP;
using Character.Trait;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Scenes.FightScene.CharacterParts.Hull {
    public class Hull : CharacterPart<Hull.Data> {
        /// <summary>
        /// Number of provided hull points
        /// </summary>
        [SerializeField, HideInInspector]
        private float hullPoints;
        
        [Trait, PublicAPI]
        private HitPoints HitPoints => new HitPoints(HPType.Armor, hullPoints);

        [CreateAssetMenu(fileName = "Hull", menuName = "Character Parts/Hull")]
        public class Data : ScriptableObject {
            [Range(0, 500)]
            public float hullPoints;
        }
        
        protected override void AssignFromData(Data data) {
            hullPoints = data.hullPoints;
        }
    }
}
