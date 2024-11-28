using System;
using Modules.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Modules.Planets
{
    public sealed class Planet : IFixedTickable, IPlanet
    {
        public event Action OnUnlocked; 
        public event Action<bool> OnIncomeReady;
        public event Action<int> OnGathered;
        public event Action<int> OnUpgraded;
        public event Action<int> OnPopulationChanged;
        public event Action<int> OnIncomeChanged;
        public event Action<float> OnIncomeTimeChanged; 

        [ShowInInspector, ReadOnly]
        public string Name => _config.Name;

        [ShowInInspector, ReadOnly]
        public bool CanUpgrade => IsUnlocked && !IsMaxLevel && _moneyAdapter.IsEnough(Price);
        
        [ShowInInspector, ReadOnly]
        public bool CanUnlock => _moneyAdapter.IsEnough(Price) && !IsUnlocked;

        [ShowInInspector, ReadOnly]
        public bool CanUnlockOrUpgrade => !IsUnlocked ? this.CanUnlock : CanUpgrade;

        [ShowInInspector, ReadOnly]
        public bool IsUnlocked { get; private set; }

        [ShowInInspector, ReadOnly]
        public int Price => !IsUnlocked ? _config.PurchasePrice : _config.GetUpgradePrice(Level + 1);

        [ShowInInspector, ReadOnly]
        public int Population { get; private set; }
        
        [Title("Level")]
        [ShowInInspector, ReadOnly]
        public bool IsMaxLevel => Level == _config.MaxLevel;
        
        [ShowInInspector, ReadOnly]
        public int Level { get; private set; }

        [ShowInInspector, ReadOnly]
        public int MaxLevel => _config.MaxLevel;
        
        [ShowInInspector, ReadOnly]
        public int NextLevel => Level + 1;
        
        [Title("Income")]
        [ShowInInspector, ReadOnly, ProgressBar(0, 1)]
        public float IncomeProgress => 1 - _countdown.RemainingTime / _countdown.Duration;

        [ShowInInspector, ReadOnly]
        public bool IsIncomeReady { get; private set; }
        
        [ShowInInspector, ReadOnly]
        public int MinuteIncome => !IsUnlocked ? 0 : _config.GetIncome(Level);
        
        [ShowInInspector, ReadOnly]
        public int NextMinuteIncome => _config.GetIncome(Level + 1);
        
        private readonly PlanetConfig _config;
        private readonly Countdown _countdown;
        private readonly IMoneyAdapter _moneyAdapter;
        private float _populationAcc;

        public Planet(PlanetConfig config, IMoneyAdapter moneyAdapter)
        {
            _config = config;
            _countdown = new Countdown(config.IncomeDuration);
            _moneyAdapter = moneyAdapter;
        }

        [Title("Methods")]
        [Button]
        public bool UnlockOrUpgrade()
        {
            return !IsUnlocked 
                ? Unlock() 
                : Upgrade();
        }
        
        [Button]
        public bool Unlock()
        {
            if (!CanUnlock)
                return false;
            
            _moneyAdapter.Spend(Price);
            
            Level = 1;
            Population = Random.Range(100, 1000);
            OnPopulationChanged?.Invoke(Population);
            
            IsUnlocked = true;
            OnUnlocked?.Invoke();
            return true;
        }

        [Button]
        public bool Upgrade()
        {
            if (!CanUpgrade)
                return false;

            _moneyAdapter.Spend(Price);

            Level++;
   
            Population = Mathf.RoundToInt(this.Population * Random.Range(1.2f, 2f));
            OnPopulationChanged?.Invoke(Population);

            OnUpgraded?.Invoke(Level);
            OnIncomeChanged?.Invoke(MinuteIncome);
            return true;
        }
        
        [Button]
        public bool GatherIncome()
        {
            if (!IsUnlocked)
                return false;

            if (!IsIncomeReady)
                return false;

            int income = MinuteIncome;
            _moneyAdapter.Earn(income);
            OnGathered?.Invoke(income);
            
            Population = Mathf.RoundToInt(this.Population * Random.Range(1.1f, 1.2f));
            OnPopulationChanged?.Invoke(Population);

            _countdown.Reset();
            OnIncomeTimeChanged?.Invoke(_countdown.RemainingTime);
            
            IsIncomeReady = false;
            OnIncomeReady?.Invoke(IsIncomeReady);

            return true;
        }

        public Sprite GetIcon(bool unlocked)
        {
            return _config.GetIcon(unlocked);
        }

        void IFixedTickable.FixedTick()
        {
            if (!IsUnlocked)
                return;

            float deltaTime = Time.fixedDeltaTime;
            this.UpdateIncome(deltaTime);
            this.UpdatePopulation(deltaTime);
        }

        private void UpdatePopulation(float deltaTime)
        {
            _populationAcc += deltaTime;
            if (_populationAcc > 1)
            {
                Population++;
                OnPopulationChanged?.Invoke(Population);
                _populationAcc--;
            }
        }

        private void UpdateIncome(float deltaTime)
        {
            if (IsIncomeReady)
                return;

            if (_countdown.IsPlaying())
            {
                _countdown.Tick(deltaTime);
                OnIncomeTimeChanged?.Invoke(_countdown.RemainingTime);
                return;
            }

            IsIncomeReady = true;
            OnIncomeReady?.Invoke(IsIncomeReady);
        }
    }
}