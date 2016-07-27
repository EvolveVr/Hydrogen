using System.Collections.Generic;

namespace Hydrogen
{
    /// <summary>
    /// This class is for storing enums were we can find them easily in the Hydrogen namesace
    /// </summary>
    public static class GameConstants
    {
        public const string MAGAZINE = "Magazine";

        public enum GameType
        {
            WaveGame
        }

        public enum Difficulty
        {
            Easy,
            Normal,
            Hard,
            Extreme
        }

        // Any weapon in the game can be categorized into one of these weapons
        public enum WeaponType
        {
            Automatic, AutomaticEngage, Semiautomatic, SemiautomaticEngage, Repeater, Charge
        }


        // this is going to be a static dictionary that needs to be accessble to Gun class so  it can init flag values easily
        public static Dictionary<WeaponType, GunTypeInitValues> gunTypeInitValues = new Dictionary<WeaponType, GunTypeInitValues>()
        {
            { WeaponType.Automatic, new GunTypeInitValues(_isAutomatic:true)},
            { WeaponType.AutomaticEngage, new GunTypeInitValues(_isAutomatic:true, _needsEnagement:true)},
            { WeaponType.SemiautomaticEngage, new GunTypeInitValues(_needsEnagement:true)},
            { WeaponType.Semiautomatic, new GunTypeInitValues()},
            { WeaponType.Charge, new GunTypeInitValues( _isCharge:true)},
            { WeaponType.Repeater, new GunTypeInitValues( _isRepeater:true, _needsEnagement:true)}
        };
    }

}
