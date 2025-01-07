using Game.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.App
{
    [CreateAssetMenu(
        fileName = "LevelConfig",
        menuName = "Levels/New LevelConfig"
    )]
    public sealed class LevelConfig : ScriptableObject
    {
        public int Number => number;
        public int GoalCount => goalCount;
        public ItemType GoalType => goalType;

        public LevelSnapshot Field => field;

        public Sprite NumberIcon => numberIcon;

        [FormerlySerializedAs("level")]
        [SerializeField]
        private int number;

        [FormerlySerializedAs("_field")]
        [SerializeField]
        private LevelSnapshot field;

        [Header("Goal")]
        [FormerlySerializedAs("collectCount")]
        [SerializeField]
        private int goalCount;

        [SerializeField]
        private ItemType goalType;

        [SerializeField]
        private Sprite numberIcon;
    }
}