using System.Collections.Generic;
using System.Windows.Forms;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace InventoryItemsAnalyzer
{
    public class InventoryItemsAnalyzerSettings : ISettings
    {
        public InventoryItemsAnalyzerSettings()
        {
            //Defaults

            #region Enable/Additional Settings

            Enable = new ToggleNode(false);
            HotKey = Keys.F2;
            ExtraDelay = new RangeNode<int>(20, 0, 100);
            HideUnderMouse = new ToggleNode(false);
            StarOrBorder = new ToggleNode(true);
            Color = new ColorBGRA(0, 255, 0, 255);
            ColorAll = new ColorBGRA(255, 255, 255, 255);
            ItemLevelInfluence = new RangeNode<int>(84, 0, 100);
            ItemLevelNoInfluence = new RangeNode<int>(84, 0, 100);
            DebugMode = new ToggleNode(false);
            Text = new ToggleNode(false);

            #endregion

            #region BodyArmour

            BodyArmour = new ToggleNode(true);
            Ba_Life = new RangeNode<int>(100, 60, 120);
            Ba_TotalRes = new RangeNode<int>(100, 60, 100);
            Ba_EnergyShield = new RangeNode<int>(2, 1, 5);
            Ba_Strength = new RangeNode<int>(40, 30, 55);
            Ba_Intelligence = new RangeNode<int>(40, 30, 55);
            Ba_Dexterity = new RangeNode<int>(40, 30, 55);
            Ba_LifeCombo = new RangeNode<int>(4, 1, 4);
            Ba_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region Helmet

            Helmet = new ToggleNode(true);
            H_Life = new RangeNode<int>(65, 60, 100);
            H_EnergyShield = new RangeNode<int>(2, 1, 5);
            H_TotalRes = new RangeNode<int>(65, 60, 100);
            H_Accuracy = new RangeNode<int>(350, 200, 500);
            H_Strength = new RangeNode<int>(30, 30, 55);
            H_Intelligence = new RangeNode<int>(30, 30, 55);
            H_Dexterity = new RangeNode<int>(30, 30, 55);
            H_Mana = new RangeNode<int>(60, 50, 80);
            H_LifeCombo = new RangeNode<int>(3, 1, 3);
            H_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region Gloves

            Gloves = new ToggleNode(true);
            G_Life = new RangeNode<int>(60, 60, 100);
            G_TotalRes = new RangeNode<int>(65, 40, 130);
            G_EnergyShield = new RangeNode<int>(2, 1, 5);
            G_Accuracy = new RangeNode<int>(350, 200, 500);
            G_AttackSpeed = new RangeNode<int>(11, 8, 16);
            G_PhysDamage = new RangeNode<int>(7, 2, 10);
            G_Strength = new RangeNode<int>(30, 30, 55);
            G_Intelligence = new RangeNode<int>(30, 30, 55);
            G_Dexterity = new RangeNode<int>(30, 30, 55);
            G_Mana = new RangeNode<int>(60, 50, 80);
            G_LifeCombo = new RangeNode<int>(2, 1, 2);
            G_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region Boots

            Boots = new ToggleNode(true);
            B_Life = new RangeNode<int>(60, 40, 100);
            B_TotalRes = new RangeNode<int>(65, 40, 130);
            B_EnergyShield = new RangeNode<int>(2, 1, 5);
            B_Strength = new RangeNode<int>(30, 25, 55);
            B_Intelligence = new RangeNode<int>(30, 25, 55);
            B_Dexterity = new RangeNode<int>(30, 25, 55);
            B_MoveSpeed = new RangeNode<int>(20, 15, 30);
            B_Mana = new RangeNode<int>(60, 50, 80);
            B_LifeCombo = new RangeNode<int>(2, 1, 2);
            B_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region Belts

            Belt = new ToggleNode(true);
            Be_Life = new RangeNode<int>(60, 45, 100);
            Be_EnergyShield = new RangeNode<int>(2, 1, 5);
            Be_TotalRes = new RangeNode<int>(65, 40, 130);
            Be_Strength = new RangeNode<int>(30, 25, 55);
            Be_WeaponElemDamage = new RangeNode<int>(25, 20, 40);
            Be_FlaskReduced = new RangeNode<int>(-20, -20, -10);
            Be_FlaskDuration = new RangeNode<int>(10, 10, 20);
            Be_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region Rings

            Ring = new ToggleNode(true);
            R_Life = new RangeNode<int>(55, 35, 80);
            R_EnergyShield = new RangeNode<int>(2, 1, 5);
            R_TotalRes = new RangeNode<int>(65, 50, 100);
            R_PhysDamage = new RangeNode<int>(10, 6, 12);
            R_WeaponElemDamage = new RangeNode<int>(25, 20, 40);
            R_Accuracy = new RangeNode<int>(350, 200, 400);
            R_AttackSpeed = new RangeNode<int>(5, 5, 20);
            R_CastSpped = new RangeNode<int>(5, 5, 20);
            R_Strength = new RangeNode<int>(30, 25, 55);
            R_Intelligence = new RangeNode<int>(30, 25, 55);
            R_Dexterity = new RangeNode<int>(30, 25, 55);
            R_Mana = new RangeNode<int>(60, 50, 80);
            R_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region Amulets

            Amulet = new ToggleNode(true);
            A_Life = new RangeNode<int>(55, 40, 80);
            A_EnergyShield = new RangeNode<int>(2, 1, 5);
            A_TotalRes = new RangeNode<int>(65, 40, 130);
            A_PhysDamage = new RangeNode<int>(10, 6, 12);
            A_WeaponElemDamage = new RangeNode<int>(25, 20, 40);
            A_Accuracy = new RangeNode<int>(350, 200, 400);
            A_CritMult = new RangeNode<int>(25, 20, 40);
            A_CritChance = new RangeNode<int>(25, 20, 40);
            A_TotalElemSpellDmg = new RangeNode<int>(13, 10, 30);
            A_Strength = new RangeNode<int>(30, 25, 55);
            A_Intelligence = new RangeNode<int>(30, 25, 55);
            A_Dexterity = new RangeNode<int>(30, 25, 55);
            A_Mana = new RangeNode<int>(60, 50, 80);
            A_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region Shields

            Shield = new ToggleNode(true);
            S_Life = new RangeNode<int>(65, 60, 110);
            S_EnergyShield = new RangeNode<int>(2, 1, 5);
            S_TotalRes = new RangeNode<int>(65, 70, 130);
            S_Strength = new RangeNode<int>(30, 25, 55);
            S_Intelligence = new RangeNode<int>(30, 25, 55);
            S_Dexterity = new RangeNode<int>(30, 25, 55);
            S_SpellDamage = new RangeNode<int>(25, 25, 80);
            S_SpellCritChance = new RangeNode<int>(75, 60, 110);
            S_LifeCombo = new RangeNode<int>(3, 1, 3);
            S_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region Weapons Caster

            WeaponCaster = new ToggleNode(true);
            Wc_TotalElemSpellDmg = new RangeNode<int>(80, 70, 130);
            Wc_SpellCritChance = new RangeNode<int>(80, 70, 150);
            Wc_ToElemDamageSpell = new RangeNode<int>(30, 20, 90);
            Wc_CritMult = new RangeNode<int>(25, 20, 40);
            Wc_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region Weapons Attack

            WeaponAttack = new ToggleNode(true);
            Wa_PhysDmg = new RangeNode<int>(300, 250, 500);
            Wa_ElemDmg = new RangeNode<int>(250, 200, 300);
            Wa_Affixes = new RangeNode<int>(1, 0, 2); // not used

            #endregion

            #region Quivers

            Quiver = new ToggleNode(true);
            Q_Life = new RangeNode<int>(55, 40, 80);
            Q_TotalRes = new RangeNode<int>(65, 40, 130);
            Q_PhysDamage = new RangeNode<int>(10, 6, 12);
            Q_WeaponElemDamage = new RangeNode<int>(25, 20, 40);
            Q_Accuracy = new RangeNode<int>(350, 200, 400);
            Q_CritMult = new RangeNode<int>(25, 20, 40);
            Q_CritChance = new RangeNode<int>(25, 20, 40);
            Q_Affixes = new RangeNode<int>(2, 0, 5);

            #endregion

            #region PoeNinja

            Update = new ToggleNode(false);

            var listLeagues = new List<string>
            {
                "Temp SC", "Temp HC"
            };
            League = new ListNode();
            League.SetListValues(listLeagues);

            ChaosUnique = new RangeNode<int>(5, 0, 50);

            ChaosProphecy = new RangeNode<int>(5, 0, 50);

            ChaosDivCard = new RangeNode<int>(10, 0, 50);

            #endregion
        }
        //Get Set

        public ToggleNode Enable { get; set; }

        #region Body Armour

        [Menu("Body Armour", 1, Tooltip = "Body Armour Filter Settings")]
        public ToggleNode BodyArmour { get; set; }

        [Menu("Life", parentIndex = 1, Tooltip = "Set the minimum value to filter life value")]
        public RangeNode<int> Ba_Life { get; set; }

        [Menu("Energy Shield (tier)", parentIndex = 1, Tooltip = "Set the minimum value to filter defence tier value")]
        public RangeNode<int> Ba_EnergyShield { get; set; }

        [Menu("Total Resistance", parentIndex = 1, Tooltip = "Set the minimum value to filter total resistance value")]
        public RangeNode<int> Ba_TotalRes { get; set; }

        [Menu("Strength", parentIndex = 1, Tooltip = "Set the minimum value to filter Strength value")]
        public RangeNode<int> Ba_Strength { get; set; }

        [Menu("Intelligence", parentIndex = 1, Tooltip = "Set the minimum value to filter Intelligence value")]
        public RangeNode<int> Ba_Intelligence { get; set; }

        [Menu("Dexterity", parentIndex = 1, Tooltip = "Set the minimum value to filter dexterity value")]
        public RangeNode<int> Ba_Dexterity { get; set; }

        [Menu("LifeCombo ((Tier)", parentIndex = 1, Tooltip = "Set the minimum value to filter lifecombo tier value")]
        public RangeNode<int> Ba_LifeCombo { get; set; }

        [Menu("Affix Count", parentIndex = 1, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> Ba_Affixes { get; set; }

        #endregion

        #region Helmet

        [Menu("Helmets", 2, Tooltip = "Helmet Filter Settings")]
        public ToggleNode Helmet { get; set; }

        [Menu("Life", parentIndex = 2, Tooltip = "Set the minimum value to filter Life value")]
        public RangeNode<int> H_Life { get; set; }

        [Menu("Energy Shield (Tier)", parentIndex = 2, Tooltip = "Set the minimum value to filter defence tier value")]
        public RangeNode<int> H_EnergyShield { get; set; }

        [Menu("Total Resistance", parentIndex = 2, Tooltip = "Set the minimum value to filter total resistance value")]
        public RangeNode<int> H_TotalRes { get; set; }

        [Menu("Accuracy", parentIndex = 2, Tooltip = "Set the minimum value to filter total accuracy value")]
        public RangeNode<int> H_Accuracy { get; set; }

        [Menu("Strength", parentIndex = 2, Tooltip = "Set the minimum value to filter Strength value")]
        public RangeNode<int> H_Strength { get; set; }

        [Menu("Intelligence", parentIndex = 2, Tooltip = "Set the minimum value to filter Intelligence value")]
        public RangeNode<int> H_Intelligence { get; set; }

        [Menu("Dexterity", parentIndex = 2, Tooltip = "Set the minimum value to filter Dexterity value")]
        public RangeNode<int> H_Dexterity { get; set; }

        [Menu("Mana", parentIndex = 2, Tooltip = "Set the minimum value to filter Mana value")]
        public RangeNode<int> H_Mana { get; set; }

        [Menu("LifeCombo ((Tier)", parentIndex = 2, Tooltip = "Set the minimum value to filter life combo tier value")]
        public RangeNode<int> H_LifeCombo { get; set; }

        [Menu("Affix Count", parentIndex = 2, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> H_Affixes { get; set; }

        #endregion

        #region Gloves

        [Menu("Gloves", 3, Tooltip = "Glove Filter Settings")]
        public ToggleNode Gloves { get; set; }

        [Menu("Life", parentIndex = 3, Tooltip = "Set the minimum value to filter for life value")]
        public RangeNode<int> G_Life { get; set; }

        [Menu("Energy Shield (Tier)", parentIndex = 3,
            Tooltip = "Set the minimum value to filter for defence tier value")]
        public RangeNode<int> G_EnergyShield { get; set; }

        [Menu("Total Resistance", parentIndex = 3,
            Tooltip = "Set the minimum value to filter for total resistance value")]
        public RangeNode<int> G_TotalRes { get; set; }

        [Menu("Strength", parentIndex = 3, Tooltip = "Set the minimum value to filter Strength value")]
        public RangeNode<int> G_Strength { get; set; }

        [Menu("Intelligence", parentIndex = 3, Tooltip = "Set the minimum value to filter Intelligence value")]
        public RangeNode<int> G_Intelligence { get; set; }

        [Menu("Dexterity", parentIndex = 3, Tooltip = "Set the minimum value to filter Dexterity value")]
        public RangeNode<int> G_Dexterity { get; set; }

        [Menu("Accuracy", parentIndex = 3, Tooltip = "Set the minimum value to filter for accuracy value")]
        public RangeNode<int> G_Accuracy { get; set; }

        [Menu("Attack Speed", parentIndex = 3, Tooltip = "Set the minimum value to filter for attack speed value")]
        public RangeNode<int> G_AttackSpeed { get; set; }

        [Menu("Physical Damage to Attacks", parentIndex = 3,
            Tooltip = "Set the minimum value to filter for physical damage to attacks value")]
        public RangeNode<int> G_PhysDamage { get; set; }

        [Menu("Mana", parentIndex = 3, Tooltip = "Set the minimum value to filter Mana value")]
        public RangeNode<int> G_Mana { get; set; }

        [Menu("LifeCombo ((Tier)", parentIndex = 3, Tooltip = "Set the minimum value to filter life combo tier value")]
        public RangeNode<int> G_LifeCombo { get; set; }

        [Menu("Affix Count", parentIndex = 3, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> G_Affixes { get; set; }

        #endregion

        #region Boots

        [Menu("Boots", 4, Tooltip = "Boot Filter Settings")]
        public ToggleNode Boots { get; set; }

        [Menu("Life", parentIndex = 4, Tooltip = "Set the minimum value to filter for life value")]
        public RangeNode<int> B_Life { get; set; }

        [Menu("Energy Shield (Tier)", parentIndex = 4,
            Tooltip = "Set the minimum value to filter for defence tier value")]
        public RangeNode<int> B_EnergyShield { get; set; }

        [Menu("Total Resistance", parentIndex = 4,
            Tooltip = "Set the minimum value to filter for total resistance value")]
        public RangeNode<int> B_TotalRes { get; set; }

        [Menu("Strength", parentIndex = 4, Tooltip = "Set the minimum value to filter for strength value")]
        public RangeNode<int> B_Strength { get; set; }

        [Menu("Intelligence", parentIndex = 4, Tooltip = "Set the minimum value to filter for intelligence value")]
        public RangeNode<int> B_Intelligence { get; set; }

        [Menu("Dexterity", parentIndex = 4, Tooltip = "Set the minimum value to filter for Dexterity value")]
        public RangeNode<int> B_Dexterity { get; set; }

        [Menu("Movement Speed", parentIndex = 4, Tooltip = "Set the minimum value to filter for movement speed value")]
        public RangeNode<int> B_MoveSpeed { get; set; }

        [Menu("Mana", parentIndex = 4, Tooltip = "Set the minimum value to filter Mana value")]
        public RangeNode<int> B_Mana { get; set; }

        [Menu("LifeCombo ((Tier)", parentIndex = 4, Tooltip = "Set the minimum value to filter life combo tier value")]
        public RangeNode<int> B_LifeCombo { get; set; }

        [Menu("Affix Count", parentIndex = 4, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> B_Affixes { get; set; }

        #endregion

        #region Belt

        [Menu("Belts", 5, Tooltip = "Belt Settings")]
        public ToggleNode Belt { get; set; }

        [Menu("Life", parentIndex = 5, Tooltip = "Set the minimum value to filter for life value")]
        public RangeNode<int> Be_Life { get; set; }

        [Menu("Energy Shield (Tier)", parentIndex = 5,
            Tooltip = "Set the minimum value to filter for energy shield tier value")]
        public RangeNode<int> Be_EnergyShield { get; set; }

        [Menu("Total Resistance", parentIndex = 5,
            Tooltip = "Set the minimum value to filter for total resistance value")]
        public RangeNode<int> Be_TotalRes { get; set; }

        [Menu("Strength", parentIndex = 5, Tooltip = "Set the minimum value to filter for strength value")]
        public RangeNode<int> Be_Strength { get; set; }

        [Menu("Weapon Elemental Damage", parentIndex = 5,
            Tooltip = "Set the minimum value to filter for weapon elemental damage value")]
        public RangeNode<int> Be_WeaponElemDamage { get; set; }

        [Menu("Flask Reduced", parentIndex = 5, Tooltip = "Set the minimum value to filter for Flask Reduced value")]
        public RangeNode<int> Be_FlaskReduced { get; set; }

        [Menu("Flask Duration", parentIndex = 5, Tooltip = "Set the minimum value to filter for Flask Duration value")]
        public RangeNode<int> Be_FlaskDuration { get; set; }

        [Menu("Affix Count", parentIndex = 5, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> Be_Affixes { get; set; }

        //        [Menu("Other Flask Suffix", 67, 60)]
        //        public RangeNode<int> BeFlaskSuffix { get; set; }

        #endregion

        #region Ring

        [Menu("Rings", 6, Tooltip = "Ring Settings")]
        public ToggleNode Ring { get; set; }

        [Menu("Life", parentIndex = 6, Tooltip = "Set the minimum value to filter for life value")]
        public RangeNode<int> R_Life { get; set; }

        [Menu("Energy Shield (Tier)", parentIndex = 6,
            Tooltip = "Set the minimum value to filter for energy shield tier value")]
        public RangeNode<int> R_EnergyShield { get; set; }

        [Menu("Total Resistance", parentIndex = 6,
            Tooltip = "Set the minimum value to filter for total resistance value")]
        public RangeNode<int> R_TotalRes { get; set; }

        [Menu("Physical Damage to Attacks", parentIndex = 6,
            Tooltip = "Set the minimum value to filter for physical damage to attacks value")]
        public RangeNode<int> R_PhysDamage { get; set; }

        [Menu("Weapon Elemental Damage", parentIndex = 6,
            Tooltip = "Set the minimum value to filter for weapon elemental damage value")]
        public RangeNode<int> R_WeaponElemDamage { get; set; }

        [Menu("Accuracy Rating", parentIndex = 6,
            Tooltip = "Set the minimum value to filter for accuracy rating value")]
        public RangeNode<int> R_Accuracy { get; set; }

        [Menu("Attack Speed", parentIndex = 6,
            Tooltip = "Set the minimum value to filter for increased attack speed value")]
        public RangeNode<int> R_AttackSpeed { get; set; }

        [Menu("Cast Speed", parentIndex = 6,
            Tooltip = "Set the minimum value to filter for increased cast speed value")]
        public RangeNode<int> R_CastSpped { get; set; }

        [Menu("Strength", parentIndex = 6, Tooltip = "Set the minimum value to filter Strength value")]
        public RangeNode<int> R_Strength { get; set; }

        [Menu("Intelligence", parentIndex = 6, Tooltip = "Set the minimum value to filter Intelligence value")]
        public RangeNode<int> R_Intelligence { get; set; }

        [Menu("Dexterity", parentIndex = 6, Tooltip = "Set the minimum value to filter Intelligence value")]
        public RangeNode<int> R_Dexterity { get; set; }

        [Menu("Mana", parentIndex = 6, Tooltip = "Set the minimum value to filter Mana value")]
        public RangeNode<int> R_Mana { get; set; }

        [Menu("Affix Count", parentIndex = 6, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> R_Affixes { get; set; }

        #endregion

        #region Amulet

        [Menu("Amulets", 7, Tooltip = "Amulet Settings")]
        public ToggleNode Amulet { get; set; }

        [Menu("Life", parentIndex = 7, Tooltip = "Set the minimum value to filter for life value")]
        public RangeNode<int> A_Life { get; set; }

        [Menu("Energy Shield (Tier)", parentIndex = 7,
            Tooltip = "Set the minimum value to filter for energy shield tier value")]
        public RangeNode<int> A_EnergyShield { get; set; }

        [Menu("Total Resitance", parentIndex = 7,
            Tooltip = "Set the minimum value to filter for total resistance value")]
        public RangeNode<int> A_TotalRes { get; set; }

        [Menu("Physical Damage to Attacks", parentIndex = 7,
            Tooltip = "Set the minimum value to filter for physical damage to attacks value")]
        public RangeNode<int> A_PhysDamage { get; set; }

        [Menu("Weapon Elemental Damage", parentIndex = 7,
            Tooltip = "Set the minimum value to filter for weapon elemental damage value")]
        public RangeNode<int> A_WeaponElemDamage { get; set; }

        [Menu("Accuracy Rating", parentIndex = 7,
            Tooltip = "Set the minimum value to filter for accuracy rating value")]
        public RangeNode<int> A_Accuracy { get; set; }

        [Menu("Critical Strike Multiplier", parentIndex = 7,
            Tooltip = "Set the minimum value to filter for critical strike multiplier value")]
        public RangeNode<int> A_CritMult { get; set; }

        [Menu("Critical Strike Chance", parentIndex = 7,
            Tooltip = "Set the minimum value to filter for critical strike chance value")]
        public RangeNode<int> A_CritChance { get; set; }

        [Menu("Total Elemental Spell Damage", parentIndex = 7,
            Tooltip = "Set the minimum value to filter for total elemental spell Damage value")]
        public RangeNode<int> A_TotalElemSpellDmg { get; set; }

        [Menu("Strength", parentIndex = 7, Tooltip = "Set the minimum value to filter Strength value")]
        public RangeNode<int> A_Strength { get; set; }

        [Menu("Intelligence", parentIndex = 7, Tooltip = "Set the minimum value to filter Intelligence value")]
        public RangeNode<int> A_Intelligence { get; set; }

        [Menu("Dexterity", parentIndex = 7, Tooltip = "Set the minimum value to filter Dexterity value")]
        public RangeNode<int> A_Dexterity { get; set; }

        [Menu("Mana", parentIndex = 7, Tooltip = "Set the minimum value to filter Mana value")]
        public RangeNode<int> A_Mana { get; set; }

        [Menu("Affix Count", parentIndex = 7, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> A_Affixes { get; set; }

        #endregion

        #region Shield

        [Menu("Shields", 8, Tooltip = "Shield Filter Settings")]
        public ToggleNode Shield { get; set; }

        [Menu("Life", parentIndex = 8, Tooltip = "Set the minimum value to filter for life value")]
        public RangeNode<int> S_Life { get; set; }

        [Menu("Energy Shield (Tier)", parentIndex = 8,
            Tooltip = "Set the minimum value to filter for energy shield tier value")]
        public RangeNode<int> S_EnergyShield { get; set; }

        [Menu("Total Resistance", parentIndex = 8,
            Tooltip = "Set the minimum value to filter for total resistance value")]
        public RangeNode<int> S_TotalRes { get; set; }

        [Menu("Strength", parentIndex = 8, Tooltip = "Set the minimum value to filter for strength value")]
        public RangeNode<int> S_Strength { get; set; }

        [Menu("Intelligence", parentIndex = 8, Tooltip = "Set the minimum value to filter for intelligence value")]
        public RangeNode<int> S_Intelligence { get; set; }

        [Menu("Dexterity", parentIndex = 8, Tooltip = "Set the minimum value to filter for Dexterity value")]
        public RangeNode<int> S_Dexterity { get; set; }

        [Menu("Spell Damage", parentIndex = 8, Tooltip = "Set the minimum value to filter for spell damage value")]
        public RangeNode<int> S_SpellDamage { get; set; }

        [Menu("Spell Critical Strike Chance", parentIndex = 8,
            Tooltip = "Set the minimum value to filter for spell critical strike chance value")]
        public RangeNode<int> S_SpellCritChance { get; set; }

        [Menu("LifeCombo ((Tier)", parentIndex = 8, Tooltip = "Set the minimum value to filter life combo tier value")]
        public RangeNode<int> S_LifeCombo { get; set; }

        [Menu("Affix Count", parentIndex = 8, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> S_Affixes { get; set; }

        #endregion

        #region Caster Dagger/Wand/Sceptre

        [Menu("Cast Weapons", 9, Tooltip = "Cast Weapon Settings")]
        public ToggleNode WeaponCaster { get; set; }

        [Menu("Total Elemental Spell Damage", parentIndex = 9,
            Tooltip = "Set the minimum value to filter for total elemental spell damage value")]
        public RangeNode<int> Wc_TotalElemSpellDmg { get; set; }

        [Menu("Fire/Cold/Lightning Dmg to Spells", parentIndex = 9,
            Tooltip = "Set the minimum value to filter for % added elemental damage to spells value")]
        public RangeNode<int> Wc_ToElemDamageSpell { get; set; }

        [Menu("Spell Critical Chance", parentIndex = 9,
            Tooltip = "Set the minimum value to filter for spell critical chance value")]
        public RangeNode<int> Wc_SpellCritChance { get; set; }

        [Menu("Critical Strike Multiplier", parentIndex = 9,
            Tooltip = "Set the minimum value to filter for critical strike multiplier value")]
        public RangeNode<int> Wc_CritMult { get; set; }

        [Menu("Affix Count", parentIndex = 9, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> Wc_Affixes { get; set; }

        #endregion

        #region Attack Thrusting/Dagger/Wand

        [Menu("Attack Weapons", 10, Tooltip = "Attack Weapon Settings")]
        public ToggleNode WeaponAttack { get; set; }

        [Menu("Physical Damage", parentIndex = 10,
            Tooltip = "Set the minimum value to filter for total physical damage value")]
        public RangeNode<int> Wa_PhysDmg { get; set; }

        [Menu("Total Elemental Spell Damage", parentIndex = 10,
            Tooltip = "Set the minimum value to filter for total elemental spell damage value")]
        public RangeNode<int> Wa_ElemDmg { get; set; }

        [Menu("Affix Count", parentIndex = 10, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> Wa_Affixes { get; set; }

        #endregion

        #region Quiver

        [Menu("Quiver", 11, Tooltip = "Quiver Settings")]
        public ToggleNode Quiver { get; set; }

        [Menu("Life", parentIndex = 11, Tooltip = "Set the minimum value to filter for life value")]
        public RangeNode<int> Q_Life { get; set; }

        [Menu("Total Resitance", parentIndex = 11,
            Tooltip = "Set the minimum value to filter for total resistance value")]
        public RangeNode<int> Q_TotalRes { get; set; }

        [Menu("Physical Damage to Attacks", parentIndex = 11,
            Tooltip = "Set the minimum value to filter for physical damage to attacks value")]
        public RangeNode<int> Q_PhysDamage { get; set; }

        [Menu("Weapon Elemental Damage", parentIndex = 11,
            Tooltip = "Set the minimum value to filter for weapon elemental damage value")]
        public RangeNode<int> Q_WeaponElemDamage { get; set; }

        [Menu("Accuracy Rating", parentIndex = 11,
            Tooltip = "Set the minimum value to filter for accuracy rating value")]
        public RangeNode<int> Q_Accuracy { get; set; }

        [Menu("Critical Strike Multiplier", parentIndex = 11,
            Tooltip = "Set the minimum value to filter for critical strike multiplier value")]
        public RangeNode<int> Q_CritMult { get; set; }

        [Menu("Critical Strike Chance", parentIndex = 11,
            Tooltip = "Set the minimum value to filter for critical strike chance value")]
        public RangeNode<int> Q_CritChance { get; set; }

        [Menu("Affix Count", parentIndex = 11, Tooltip = "Set the minimum number of affixes for this item class")]
        public RangeNode<int> Q_Affixes { get; set; }

        #endregion

        #region PoeNinjaUnique

        [Menu("Poe Ninja", 99, Tooltip = "Parsing poe ninja")]
        public ToggleNode Update { get; set; }

        [Menu("League", parentIndex = 99, Tooltip = "Choose league")]
        public ListNode League { get; set; }

        [Menu("Chaos-Unique", parentIndex = 99, Tooltip = "Set min chaos value for uniq items")]
        public RangeNode<int> ChaosUnique { get; set; }

        [Menu("Chaos-Prophecy", parentIndex = 99, Tooltip = "Set min chaos value for prophecies")]
        public RangeNode<int> ChaosProphecy { get; set; }

        [Menu("Chaos-DivCard", parentIndex = 99, Tooltip = "Set min chaos value for div Card")]
        public RangeNode<int> ChaosDivCard { get; set; }

        #endregion

        #region Additional Menu Settings

        [Menu("Additional Menu Settings", 100)]
        public EmptyNode Settings { get; set; }

        [Menu("Hide under Mouse", parentIndex = 100, Tooltip = "Hide a star on mouse over")]
        public ToggleNode HideUnderMouse { get; set; }

        [Menu("Text or Border", parentIndex = 100, Tooltip = "Display a text or border around the filtered item")]
        public ToggleNode StarOrBorder { get; set; }

        [Menu("Text Color", parentIndex = 100,
            Tooltip = "Display color of star on or border around the Syndicate item/Good item")]
        public ColorNode Color { get; set; }

        [Menu("Frame Color", parentIndex = 100,
            Tooltip = "Display color of star on or border around the Shaper/Elder/ItemLevel")]
        public ColorNode ColorAll { get; set; }

        [Menu("Text instead of image", parentIndex = 100, Tooltip = "Replaces an image with text")]
        public ToggleNode Text { get; set; }

        [Menu("Debug Mode", parentIndex = 100,
            Tooltip = "Enable debug mode to report collected information about items")]
        public ToggleNode DebugMode { get; set; }

        [Menu("Hotkey autovendor")] public HotkeyNode HotKey { get; set; }

        [Menu("Extra Delay", "Additional delay, plugin should work without extra delay, this is merely optional.")]
        public RangeNode<int> ExtraDelay { get; set; }

        [Menu("ItemLevelInfluence", "Set min ItemLevel for highlighted items with influence")]
        public RangeNode<int> ItemLevelInfluence { get; set; }

        [Menu("ItemLevelNoInfluence", "Set min ItemLevet for highlighted (ItemBase)")]
        public RangeNode<int> ItemLevelNoInfluence { get; set; }

        [Menu("Vendor rare jewels")]
        public ToggleNode VendorRareJewels { get; set; } = new ToggleNode(true);
        
        [Menu("Vendor talismans")]
        public ToggleNode VendorTalismans { get; set; } = new ToggleNode(true);
        
        [Menu("Vendor shit div cards")]
        public ToggleNode VendorShitDivCards { get; set; } = new ToggleNode(true);

        [Menu("Vendor breach rings")]
        public ToggleNode VendorBreachRings { get; set; } = new ToggleNode(true);
        
        [Menu("Treat veiled as regular item")]
        public ToggleNode TreatVeiledAsRegularItem { get; set; } = new ToggleNode(true);
        
        [Menu("Dont sell enchanted helmets (use loot custom filter)")]
        public ToggleNode DontSellEnchantedHelmets { get; set; } = new ToggleNode(true);
        
        [Menu("Vendor for scrolls")]
        public ToggleNode VendorForScrolls { get; set; } = new ToggleNode(true);

        #endregion
    }
}