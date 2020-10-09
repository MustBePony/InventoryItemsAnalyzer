using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using AdvancedTooltip;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using Newtonsoft.Json.Linq;
using SharpDX;
// ReSharper disable StringLiteralTypo

namespace InventoryItemsAnalyzer
{
    public class InventoryItemsAnalyzer : BaseSettingsPlugin<InventoryItemsAnalyzerSettings>
    {
        private const string coroutineName = "InventoryItemsAnalyzer";
        private readonly List<RectangleF> _allItemsPos;
        private readonly List<RectangleF> _goodItemsPos;
        private readonly List<RectangleF> _highItemsPos;

        /// <summary>
        /// Price doesnt always correlate with good/bad. We want also currency like cards when flip stacked decks
        /// </summary>
        private readonly string[] _goodDivCardsCuratedList =
        {
            // T1 fail safe
            "House of Mirrors", "The Doctor", "Unrequited Love", "The Demon", "The Fiend", "Brother's Stash",
            "Gift of Asenath", "The Cheater", "Beauty Through Death", "Nook's Crown", "The Immortal",
            "Succor of the Sinless", "Alluring Bounty", "Immortal Resolve", "The Nurse", "The Samurai's Eye",
            "The Craving", "Desecrated Virtue", "Abandoned Wealth", "The Iron Bard", "Seven Years Bad Luck",
            "Pride of the First Ones", "The Progeny of Lunaris", "The White Knight", "The Long Con",
            "The Dragon's Heart", "Wealth and Power", "Azyran's Reward", "The Price of Loyalty", "The Sustenance",
            "The Greatest Intentions",

            // T2 fail safe
            "The Saint's Treasure", "The World Eater", "The Escape", "The Soul", "Hunter's Reward", "The Damned",
            "The Eye of Terror", "The Hive of Knowledge", "The Gulf", "The Spark and the Flame", "The Primordial",
            "The Strategist", "The Artist", "The Mayor", "Void of the Elements", "The Betrayal", "The Enlightened",
            "Dark Dreams", "Squandered Prosperity", "The Bitter Blossom", "Council of Cats", "The Academic",

            // T3 fail safe + currency cards
            "Chaotic Disposition", "The Hoarder", "The Offering", "The Sephirot", "The Bargain", "Etched in Blood",
            "Peaceful Moments", "Pride Before the Fall", "The Celestial Justicar", "The Queen",
            "The Wolven King's Bite", "The Life Thief", "Remembrance", "The Eldritch Decay", "A Familiar Call",
            "The Brittle Emperor", "The Cartographer", "The Polymath", "Rebirth", "Blessing of God", "The Undaunted",
            "The Celestial Stone", "A Note in the Wind", "The Awakened", "Triskaidekaphobia", "Unchained",
            "Mawr Blaidd", "The Cursed King", "The Dapper Prodigy", "The Ethereal", "The King's Heart",
            "The Last One Standing", "The Risk", "The Valkyrie", "The Vast", "The Void", "Time-Lost Relic",
            "The Breach", "The Hale Heart", "The Professor", "The Rite of Elements", "The Wilted Rose", "The Sacrifice",
            "The Landing", "Burning Blood", "The Old Man", "Divine Justice", "Underground Forest", "The Fishmonger",
            "Prometheus' Armoury", "Heterochromia",

            // currency cards
            "Boon of Justice",
            "Bowyer's Dream",
            "Buried Treasure",
            "Coveted Possession",
            "Demigod's Wager",
            "Emperor of Purity",
            //"Emperor's Luck",
            "Harmony of Souls",
            "Imperial Legacy",
            "Last Hope",
            "Loyalty",
            "Lucky Connections",
            "Lucky Deck",
            "Monochrome",
            "More is Never Enough",
            "No Traces",
            "Sambodhi's Vow",
            "The Cacophony",
            //"The Catalyst",
            "The Chains that Bind",
            "The Deal",
            "The Fool",
            "The Gemcutter",
            "The Heroic Shot",
            "The Innocent",
            "The Inventor",
            "The Journey",
            //"The Master Artisan",
            "The Seeker",
            "The Side Quest",
            "The Survivalist",
            "The Union",
            "The Wrath",
            "Three Faces in the Dark",
            //"Three Voices",
            "Vinia's Token",
            "The Warlord", //6L
            "The Dark Mage", //6L
            "The Porcupine", //6L
        };
        
        private readonly string[] _incElemDmg =
            {"FireDamagePercentage", "ColdDamagePercentage", "LightningDamagePercentage"};

        private readonly string _leagueName = "Heist";
        private readonly List<RectangleF> _veilItemsPos;

        private int _countInventory;
        private int _idenf;
        private IngameState _ingameState;
        private Entity _item;

        private List<ModValue> _mods;
        private DateTime _renderWait;
        private int _totalWeight;
        private TimeSpan _wait = new TimeSpan(0, 0, 0, 0, 500);

        private Vector2 _windowOffset;
        private Coroutine CoroutineWorker;

        private HashSet<string> GoodBaseTypes;
        private HashSet<string> ShitDivCards;
        private HashSet<string> GoodProphecies;
        private HashSet<string> ShitUniques;

        public InventoryItemsAnalyzer()
        {
            _mods = new List<ModValue>();
            _renderWait = DateTime.Now;
            _goodItemsPos = new List<RectangleF>();
            _allItemsPos = new List<RectangleF>();
            _highItemsPos = new List<RectangleF>();
            _veilItemsPos = new List<RectangleF>();
        }

        public override bool Initialise()
        {
            base.Initialise();

            try
            {
                ParseConfig_BaseType();

                ParsePoeNinja();
            }
            catch (Exception)
            {
            }

            Name = "INV Item Analyzer";

            var combine = Path.Combine(DirectoryFullName, "img", "GoodItem.png").Replace('\\', '/');
            Graphics.InitImage(combine, false);

            combine = Path.Combine(DirectoryFullName, "img", "Syndicate.png").Replace('\\', '/');
            Graphics.InitImage(combine, false);

            _ingameState = GameController.Game.IngameState;
            _windowOffset = GameController.Window.GetWindowRectangle().TopLeft;

            Input.RegisterKey(Settings.HotKey.Value);
            Input.RegisterKey(Keys.LControlKey);

            Settings.HotKey.OnValueChanged += () => { Input.RegisterKey(Settings.HotKey.Value); };

            Settings.League.OnValueSelectedPre += s => { ParsePoeNinja(); };

            return true;
        }

        public override void Render()
        {
            if (!_ingameState.IngameUi.InventoryPanel.IsVisible)
            {
                _countInventory = 0;
                _idenf = 0;
                return;
            }

            var wait = Math.Abs((_renderWait - DateTime.Now).Ticks);

            if (wait < _wait.Ticks)
                if (!Settings.HideUnderMouse)
                {
                    DrawSyndicateItems(_veilItemsPos);
                    DrawGoodItems(_goodItemsPos);
                    DrawHighItemLevel(_highItemsPos);

                    if (Settings.HotKey.PressedOnce())
                    {
                        CoroutineWorker = new Coroutine(ClickShit(), this, coroutineName);
                        Core.ParallelRunner.Run(CoroutineWorker);
                    }
                }

            if (wait > _wait.Ticks)
            {
                var normalInventoryItems = _ingameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory]
                    .VisibleInventoryItems;

                var temp = normalInventoryItems.Count(t => t.Item?.GetComponent<Mods>()?.Identified == true);

                //LogMessage(normalInventoryItems.Count.ToString() + " " + CountInventory.ToString() + " // " + temp .ToString() + " " + idenf.ToString(), 3f);

                if (normalInventoryItems.Count != _countInventory || temp != _idenf)
                {
                    ScanInventory(normalInventoryItems);
                    _countInventory = normalInventoryItems.Count;
                    _idenf = temp;
                }

                _renderWait = DateTime.Now;
            }
        }

        #region Scan Inventory

        private void ScanInventory(IList<NormalInventoryItem> normalInventoryItems)
        {
            _goodItemsPos.Clear();
            _allItemsPos.Clear();
            _highItemsPos.Clear();
            _veilItemsPos.Clear();

            foreach (var normalInventoryItem in normalInventoryItems)
            {
                #region Start

                var highItemLevel = false;
                var item = normalInventoryItem.Item;
                if (item == null)
                    continue;

                if (string.IsNullOrEmpty(item.Path))
                    continue;

                var modsComponent = item.GetComponent<Mods>();

                var drawRect = normalInventoryItem.GetClientRect();
                //fix star position
                drawRect.X -= 5;
                drawRect.Y -= 5;

                var bit = GameController.Files.BaseItemTypes.Translate(item.Path);

                #endregion

                #region Vendor for alts

                if (Settings.VendorRareJewels &&
                    modsComponent?.ItemRarity == ItemRarity.Rare &&
                    modsComponent?.Identified == true &&
                    item.Path.Contains("Jewel"))
                    _allItemsPos.Add(drawRect);

                if (Settings.VendorTalismans &&
                    modsComponent?.ItemRarity == ItemRarity.Rare &&
                    modsComponent?.Identified == true &&
                    item.Path.Contains("Talisman"))
                    _allItemsPos.Add(drawRect);

                if (Settings.VendorBreachRings &&
                    modsComponent?.ItemRarity == ItemRarity.Rare &&
                    modsComponent?.Identified == true &&
                    item.Path.Contains("BreachRing"))
                    _allItemsPos.Add(drawRect);              

                #endregion

                #region Prophecy

                if (item.HasComponent<Prophecy>())
                {
                    var prop = item.GetComponent<Prophecy>();

                    if (GoodProphecies.Contains(prop?.DatProphecy?.Name)) _goodItemsPos.Add(drawRect);
                }

                #endregion

                #region Div Card

                if (bit.ClassName.Equals("DivinationCard"))
                {
                    var highPriced = !ShitDivCards.Contains(bit.BaseName);
                    var vendor = _goodDivCardsCuratedList.All(name => name != bit.BaseName);
                    
                    if (highPriced)
                    {
                        _goodItemsPos.Add(drawRect);
                    }
                    else if (Settings.VendorShitDivCards && vendor)
                    {
                        _allItemsPos.Add(drawRect);
                    }
                }

                #endregion

                #region Filter trash uniques

                if (modsComponent?.ItemRarity == ItemRarity.Unique &&
                    ShitUniques.Contains(modsComponent.UniqueName) &&
                    !item.HasComponent<Map>() &&
                    item.GetComponent<Sockets>()?.LargestLinkSize != 6
                )
                {
                    if (bit.ClassName.Contains("MetamorphosisDNA"))
                        continue;

                    LogMessage(modsComponent.UniqueName);
                    _allItemsPos.Add(drawRect);
                    continue;
                }

                #endregion

                #region 6socket N/M for sale

                if (modsComponent?.ItemRarity == ItemRarity.Normal || modsComponent?.ItemRarity == ItemRarity.Magic)
                {
                    if (item.GetComponent<Sockets>()?.NumberOfSockets == 6)
                        _allItemsPos.Add(drawRect);

                    continue;
                }

                #endregion

                if (modsComponent?.ItemRarity != ItemRarity.Rare || modsComponent.Identified == false)
                    continue;

                _totalWeight = 0;

                var itemMods = modsComponent.ItemMods;

                _mods.Clear();

                _mods = itemMods
                    .Select(it => new ModValue(it, GameController.Files, modsComponent.ItemLevel, bit)).ToList();

                _item = item;

                #region Influence

                {
                    var baseComponent = item.GetComponent<Base>();
                    if (modsComponent.ItemLevel >= Settings.ItemLevelInfluence &&
                        (baseComponent.isElder || baseComponent.isShaper || baseComponent.isCrusader ||
                         baseComponent.isHunter || baseComponent.isRedeemer || baseComponent.isWarlord))
                    {
                        highItemLevel = true;
                        // dont vendor influenced
                        continue;
                    }
                }

                #endregion

                #region Item Level

                if (modsComponent.ItemLevel >= Settings.ItemLevelNoInfluence && GoodBaseTypes.Contains(bit.BaseName))
                    highItemLevel = true;

                #endregion

                //---------------------------------------------------------------------------------------------------------------------------
                if (highItemLevel)
                    _highItemsPos.Add(drawRect);

                if (IsVeiled())
                {
                    _veilItemsPos.Add(drawRect);
                    continue;
                }

                switch (bit?.ClassName)
                {
                    case "Body Armour":

                        CheckLife(Settings.Ba_Life);
                        CheckDefense(Settings.Ba_EnergyShield);
                        CheckResist(Settings.Ba_TotalRes);
                        CheckIntelligence(Settings.Ba_Intelligence);
                        CheckDexterity(Settings.Ba_Dexterity);
                        CheckStrength(Settings.Ba_Strength);
                        CheckComboLife(Settings.Ba_LifeCombo);

                        CheckGood(Settings.Ba_Affixes, drawRect);
                        break;

                    case "Quiver":

                        CheckLife(Settings.Q_Life);
                        CheckResist(Settings.Q_TotalRes);
                        CheckAccuracy(Settings.Q_Accuracy);
                        CheckFlatPhys(Settings.Q_PhysDamage);
                        CheckWED(Settings.Q_WeaponElemDamage);
                        CheckCritMultiGlobal(Settings.Q_CritMult);
                        CheckGlobalCritChance(Settings.Q_CritChance);

                        CheckGood(Settings.Q_Affixes, drawRect);
                        break;

                    case "Helmet":

                        CheckLife(Settings.H_Life);
                        CheckDefense(Settings.H_EnergyShield);
                        CheckResist(Settings.H_TotalRes);
                        CheckIntelligence(Settings.H_Dexterity);
                        CheckDexterity(Settings.H_Dexterity);
                        CheckStrength(Settings.H_Intelligence);
                        CheckComboLife(Settings.H_LifeCombo);
                        CheckAccuracy(Settings.H_Accuracy);
                        CheckMana(Settings.H_Mana);

                        CheckGood(Settings.H_Affixes, drawRect);
                        break;

                    case "Boots":

                        CheckLife(Settings.B_Life);
                        CheckDefense(Settings.B_EnergyShield);
                        CheckMoveSpeed(Settings.B_MoveSpeed);
                        CheckResist(Settings.B_TotalRes);
                        CheckIntelligence(Settings.B_Intelligence);
                        CheckDexterity(Settings.B_Dexterity);
                        CheckStrength(Settings.B_Strength);
                        CheckComboLife(Settings.B_LifeCombo);
                        CheckMana(Settings.B_Mana);

                        CheckGood(Settings.B_Affixes, drawRect);
                        break;

                    case "Gloves":

                        CheckLife(Settings.G_Life);
                        CheckDefense(Settings.G_EnergyShield);
                        CheckResist(Settings.G_TotalRes);
                        CheckAccuracy(Settings.G_Accuracy);
                        CheckAttackSpeed(Settings.G_AttackSpeed);
                        CheckFlatPhys(Settings.G_PhysDamage);
                        CheckStrength(Settings.G_Strength);
                        CheckIntelligence(Settings.G_Intelligence);
                        CheckDexterity(Settings.G_Dexterity);
                        CheckMana(Settings.G_Mana);
                        CheckComboLife(Settings.G_LifeCombo);

                        CheckGood(Settings.G_Affixes, drawRect);
                        break;

                    case "Shield":

                        CheckLife(Settings.S_Life);
                        CheckDefense(Settings.S_EnergyShield);
                        CheckResist(Settings.S_TotalRes);
                        CheckStrength(Settings.S_Strength);
                        CheckDexterity(Settings.S_Dexterity);
                        CheckIntelligence(Settings.S_Intelligence);
                        CheckSpellElemDamage(Settings.S_SpellDamage);
                        CheckSpellCrit(Settings.S_SpellCritChance);
                        CheckComboLife(Settings.S_LifeCombo);

                        CheckGood(Settings.S_Affixes, drawRect);
                        break;

                    case "Belt":

                        CheckLife(Settings.Be_Life);
                        CheckEnergyShieldJewel(Settings.Be_EnergyShield);
                        CheckResist(Settings.Be_TotalRes);
                        CheckStrength(Settings.Be_Strength);
                        CheckWED(Settings.Be_WeaponElemDamage);
                        CheckFlaskReduced(Settings.Be_FlaskReduced);
                        CheckFlaskDuration(Settings.Be_FlaskDuration);

                        CheckGood(Settings.Be_Affixes, drawRect);
                        break;

                    case "Ring":

                        CheckLife(Settings.R_Life);
                        CheckResist(Settings.R_TotalRes);
                        CheckEnergyShieldJewel(Settings.R_EnergyShield);
                        CheckAttackSpeed(Settings.R_AttackSpeed);
                        CheckCastSpeed(Settings.R_CastSpped);
                        CheckAccuracy(Settings.R_Accuracy);
                        CheckFlatPhys(Settings.R_PhysDamage);
                        CheckWED(Settings.R_WeaponElemDamage);
                        CheckStrength(Settings.R_Strength);
                        CheckIntelligence(Settings.R_Intelligence);
                        CheckDexterity(Settings.R_Dexterity);
                        CheckMana(Settings.R_Mana);

                        CheckGood(Settings.R_Affixes, drawRect);
                        break;

                    case "Amulet":

                        CheckLife(Settings.A_Life);
                        CheckEnergyShieldJewel(Settings.A_EnergyShield);
                        CheckResist(Settings.A_TotalRes);
                        CheckAccuracy(Settings.A_Accuracy);
                        CheckFlatPhys(Settings.A_PhysDamage);
                        CheckWED(Settings.A_WeaponElemDamage);
                        CheckGlobalCritChance(Settings.A_CritChance);
                        CheckCritMultiGlobal(Settings.A_CritMult);
                        CheckSpellElemDamage(Settings.A_TotalElemSpellDmg);
                        CheckStrength(Settings.A_Strength);
                        CheckIntelligence(Settings.A_Intelligence);
                        CheckDexterity(Settings.A_Dexterity);
                        CheckMana(Settings.A_Mana);

                        CheckGood(Settings.A_Affixes, drawRect);
                        break;

                    case "Dagger":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "Rune Dagger":
                        CheckCastWeapon();

                        CheckGood(Settings.Wc_Affixes, drawRect);
                        break;

                    case "Wand":
                        CheckCastWeapon();

                        CheckGood(Settings.Wc_Affixes, drawRect);
                        break;

                    case "Sceptre":
                        CheckCastWeapon();

                        CheckGood(Settings.Wc_Affixes, drawRect);
                        break;

                    case "Thrusting One Hand Sword":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "Staff":
                        CheckCastWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "Warstaff":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "Claw":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "One Hand Sword":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "Two Hand Sword":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "One Hand Axe":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "Two Hand Axe":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "One Hand Mace":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "Two Hand Mace":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    case "Bow":
                        CheckAttackWeapon();

                        CheckGood(Settings.Wa_Affixes, drawRect);
                        break;

                    default:
                        continue;
                }

                if (Settings.DebugMode)
                {
                    foreach (var mod in _mods) LogMessage(mod.Record.Group + " : " + mod.StatValue[0], 10f);

                    LogMessage(_totalWeight.ToString(), 10f);
                    LogMessage("--------------------", 10f);
                }
            }
        }

        #endregion

        #region DrawHighItemLevel

        private void DrawHighItemLevel(List<RectangleF> HighItemLevel)
        {
            foreach (var position in HighItemLevel)
                if (Settings.StarOrBorder)
                {
                    var border = new RectangleF
                    {
                        X = position.X + 8, Y = position.Y + 8, Width = position.Width - 6, Height = position.Height - 6
                    };
                    Graphics.DrawFrame(border, Settings.ColorAll, 1);
                }
        }

        #endregion

        #region Draw GoodItems

        private void DrawGoodItems(List<RectangleF> goodItems)
        {
            foreach (var position in goodItems)
                if (Settings.StarOrBorder)
                {
                    var border = new RectangleF
                    {
                        X = position.X + 8, Y = position.Y + 8, Width = (position.Width - 6) / 1.5f,
                        Height = (position.Height - 6) / 1.5f
                    };

                    if (!Settings.Text)
                        Graphics.DrawImage("GoodItem.png", border);
                    else
                        Graphics.DrawText(@" Good Item ", position.TopLeft, Settings.Color, 30);
                }
        }

        #endregion

        #region Draw Syndicate items

        private void DrawSyndicateItems(List<RectangleF> SyndicateItems)
        {
            foreach (var position in SyndicateItems)
                if (Settings.StarOrBorder)
                {
                    var border = new RectangleF
                    {
                        X = position.X + 8, Y = position.Y + 8, Width = (position.Width - 6) / 1.5f,
                        Height = (position.Height - 6) / 1.5f
                    };

                    if (!Settings.Text)
                        Graphics.DrawImage("Syndicate.png", border);
                    else
                        Graphics.DrawText(@" Syndicate ", position.TopLeft, Settings.Color, 30);
                }
        }

        #endregion

        #region ClickShit

        private IEnumerator ClickShit()
        {
            Input.KeyDown(Keys.LControlKey);
            foreach (var position in _allItemsPos)
            {
                var vector2 = new Vector2(position.X + 25, position.Y + 25);

                yield return Input.SetCursorPositionSmooth(vector2 + _windowOffset);

                yield return new WaitTime(Settings.ExtraDelay.Value / 2);

                Input.Click(MouseButtons.Left);

                yield return new WaitTime(Settings.ExtraDelay.Value);
            }

            Input.KeyUp(Keys.LControlKey);
        }

        #endregion

        private void CheckAttackWeapon()
        {
            GetWeaponElemDamage();
            GetWeaponPhysDamage();
        }

        private void CheckCastWeapon()
        {
            CheckSpellCrit(Settings.Wc_SpellCritChance);

            CheckSpellElemDamage(Settings.Wc_TotalElemSpellDmg);

            CheckSpellAddedElem(Settings.Wc_ToElemDamageSpell);

            CheckCritMultiSpell(Settings.Wc_CritMult);

            CheckDOTMultiplier(20);
        }

        #region Load config

        #region Local Config

        private void ParseConfig_BaseType()
        {
            var path = $"{DirectoryFullName}\\BaseType.txt";

            CheckGoodBase(path);

            using (var reader = new StreamReader(path))
            {
                var text = reader.ReadToEnd();

                GoodBaseTypes = text.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToHashSet();

                reader.Close();
            }
        }

        private void CheckGoodBase(string path)
        {
            if (File.Exists(path)) return;

            // Tier 1-3 NeverSink
            var text = "Opal Ring" + "\r\n" + "Steel Ring" + "\r\n" + "Vermillion Ring" + "\r\n" +
                       "Blue Pearl Amulet" + "\r\n" + "Bone Helmet" + "\r\n" +
                       "Cerulean Ring" + "\r\n" + "Convoking Wand" + "\r\n" + "Crystal Belt" + "\r\n" +
                       "Fingerless Silk Gloves" + "\r\n" + "Gripped Gloves" + "\r\n" +
                       "Marble Amulet" + "\r\n" + "Sacrificial Garb" + "\r\n" + "Spiked Gloves" + "\r\n" +
                       "Stygian Vise" + "\r\n" + "Two-Toned Boots" + "\r\n" +
                       "Vanguard Belt" + "\r\n" + "Diamond Ring" + "\r\n" + "Onyx Amulet" + "\r\n" +
                       "Two-Stone Ring" + "\r\n" + "Colossal Tower Shield" + "\r\n" +
                       "Eternal Burgonet" + "\r\n" + "Hubris Circlet" + "\r\n" + "Lion Pelt" + "\r\n" +
                       "Sorcerer Boots" + "\r\n" + "Sorcerer Gloves" + "\r\n" +
                       "Titanium Spirit Shield" + "\r\n" + "Vaal Regalia" + "\r\n";


            using (var streamWriter = new StreamWriter(path, true))
            {
                streamWriter.Write(text);
                streamWriter.Close();
            }
        }

        #endregion

        private void ParsePoeNinja()
        {
            ShitUniques = new HashSet<string>();
            GoodProphecies = new HashSet<string>();
            ShitDivCards = new HashSet<string>();

            if (!Settings.Update.Value)
                return;

            #region Unique

            List<string> uniquesUrls;

            switch (Settings.League.Value)
            {
                case "Temp SC":
                    uniquesUrls = new List<string>
                    {
                        @"https://poe.ninja/api/data/itemoverview?league=" + _leagueName +
                        @"&type=UniqueJewel&language=en",
                        @"https://poe.ninja/api/data/itemoverview?league=" + _leagueName +
                        @"&type=UniqueFlask&language=en",
                        @"https://poe.ninja/api/data/itemoverview?league=" + _leagueName +
                        @"&type=UniqueWeapon&language=en",
                        @"https://poe.ninja/api/data/itemoverview?league=" + _leagueName +
                        @"&type=UniqueArmour&language=en",
                        @"https://poe.ninja/api/data/itemoverview?league=" + _leagueName +
                        @"&type=UniqueAccessory&language=en"
                    };
                    break;

                case "Temp HC":
                    uniquesUrls = new List<string>
                    {
                        @"https://poe.ninja/api/data/itemoverview?league=Hardcore+" + _leagueName +
                        @"&type=UniqueJewel&language=en",
                        @"https://poe.ninja/api/data/itemoverview?league=Hardcore+" + _leagueName +
                        @"&type=UniqueFlask&language=en",
                        @"https://poe.ninja/api/data/itemoverview?league=Hardcore+" + _leagueName +
                        @"&type=UniqueWeapon&language=en",
                        @"https://poe.ninja/api/data/itemoverview?league=Hardcore+" + _leagueName +
                        @"&type=UniqueArmour&language=en",
                        @"https://poe.ninja/api/data/itemoverview?league=Hardcore+" + _leagueName +
                        @"&type=UniqueAccessory&language=en"
                    };
                    break;

                default:
                    uniquesUrls = new List<string>();
                    break;
            }

            var result = new List<string>();

            foreach (var url in uniquesUrls)
                using (var wc = new WebClient())
                {
                    var json = wc.DownloadString(url);
                    var o = JObject.Parse(json);
                    foreach (var line in o?["lines"])
                    {
                        if (int.TryParse((string) line?["links"], out var links) &&
                            links == 6)
                            continue;

                        if (float.TryParse((string) line?["chaosValue"], out var chaosValue) &&
                            chaosValue < Settings.ChaosUnique.Value)
                            result.Add((string) line?["name"]);
                    }
                }

            ShitUniques = result.ToHashSet();

            #endregion

            #region Prophecy

            string url2;

            switch (Settings.League.Value)
            {
                case "Temp SC":
                    url2 = @"https://poe.ninja/api/data/itemoverview?league=" + _leagueName +
                           @"&type=Prophecy&language=en";
                    break;

                case "Temp HC":
                    url2 = @"https://poe.ninja/api/data/itemoverview?league=Hardcore+" + _leagueName +
                           @"&type=Prophecy&language=en";
                    break;

                default:
                    url2 = "";
                    break;
            }

            result.Clear();

            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url2);
                var o = JObject.Parse(json);
                foreach (var line in o?["lines"])
                    if (float.TryParse((string) line?["chaosValue"], out var chaosValue) &&
                        chaosValue >= Settings.ChaosProphecy.Value)
                        result.Add((string) line?["name"]);
            }

            GoodProphecies = result.ToHashSet();

            #endregion

            #region DivCard

            string url3;

            switch (Settings.League.Value)
            {
                case "Temp SC":
                    url3 = @"https://poe.ninja/api/data/itemoverview?league=" + _leagueName +
                           @"&type=DivinationCard&language=en";
                    break;

                case "Temp HC":
                    url3 = @"https://poe.ninja/api/data/itemoverview?league=Hardcore+" + _leagueName +
                           @"&type=DivinationCard&language=en";
                    break;

                default:
                    url3 = "";
                    break;
            }

            result.Clear();

            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url3);
                var o = JObject.Parse(json);
                foreach (var line in o?["lines"])
                    if (float.TryParse((string) line?["chaosValue"], out var chaosValue) &&
                        chaosValue < Settings.ChaosProphecy.Value)
                        result.Add((string) line?["name"]);
            }

            ShitDivCards = result.ToHashSet();

            #endregion

            #region Test

            // string text = "";
            //
            // foreach (var v in GoodProphecies)
            // {
            //     text += v + Environment.NewLine;
            // }
            //
            // string path = $"{DirectoryFullName}\\Test.txt";
            // using (StreamWriter streamWriter = new StreamWriter(path, true))
            // {
            //     streamWriter.Write(text);
            //     streamWriter.Close();
            // }

            #endregion
        }

        #endregion

        #region Supports

        private int Average(IReadOnlyList<int> x)
        {
            return (x[0] + x[1]) / 2;
        }

        private int FixTierEs(string key)
        {
            return 9 - int.Parse(key.Last().ToString());
        }

        private bool IsVeiled()
        {
            return _mods.Exists(mod =>
                mod.Record.Group.Contains("VeiledSuffix") || mod.Record.Group.Contains("VeiledPrefix"));
        }

        private void CheckGood(int value, RectangleF drawRect)
        {
            if (_totalWeight >= value)
                _goodItemsPos.Add(drawRect);
            else
                _allItemsPos.Add(drawRect);
        }

        #endregion

        #region Mods

        private bool OneSimpleMod(string textmod, int value)
        {
            try
            {
                return _mods.Exists(mod =>
                    mod.Record.Group.Contains(textmod) &&
                    mod.StatValue[0] >= value);
            }
            catch
            {
                return false;
            }
        }

        protected void CheckDefense(int value, int weight = 1)
        {
            try
            {
                foreach (var VARIABLE in _mods.Where(mod =>
                    (mod.Record.Group.Contains("DefencesPercent") || mod.Record.Group.Contains("BaseLocalDefences")) &&
                    !mod.Record.Group.Contains("And") &&
                    mod.Tier <= value && mod.Tier > 0))
                    _totalWeight += weight;
            }
            catch
            {
                // ignored
            }
        }

        protected void CheckResist(int value, int weight = 1)
        {
            try
            {
                var elemRes = 0;

                var aaaa = _mods.Count(m => m.Record.Group.Contains("Resist"));
                var bbbb = _mods.Count;

                LogMessage(aaaa.ToString(), 10f);
                LogMessage(bbbb.ToString(), 10f);

                foreach (var mod in _mods.Where(m => m.Record.Group.Contains("Resist")))
                {
                    if (mod.Record.Group.Contains("AllResistances"))
                        elemRes += mod.StatValue[0] * 3;

                    if (mod.Record.Group.Contains("And"))
                        elemRes += mod.StatValue[0] * 2;
                    else
                        elemRes += mod.StatValue[0];
                }

                if (elemRes >= value) _totalWeight += weight;
            }
            catch
            {
                // ignored
            }
        }

        protected void CheckLife(int value, int weight = 1)
        {
            if (OneSimpleMod("IncreasedLife", value))
                _totalWeight += weight;
        }

        protected void CheckStrength(int value, int weight = 1)
        {
            if (OneSimpleMod("Strength", value))
                _totalWeight += weight;
        }

        protected void CheckIntelligence(int value, int weight = 1)
        {
            if (OneSimpleMod("Intelligence", value))
                _totalWeight += weight;
        }

        protected void CheckDexterity(int value, int weight = 1)
        {
            if (OneSimpleMod("Dexterity", value))
                _totalWeight += weight;
        }

        protected void CheckComboLife(int value, int weight = 1)
        {
            try
            {
                if (_mods.Exists(mod =>
                    mod.Record.Group.Contains("BaseLocalDefencesAndLife") &&
                    mod.Tier <= value
                    && mod.Tier > 0))
                    _totalWeight += weight;
            }
            catch
            {
                // ignored
            }
        }

        protected void CheckAccuracy(int value, int weight = 1)
        {
            if (OneSimpleMod("IncreasedAccuracy", value))
                _totalWeight += weight;
        }

        protected void CheckFlatPhys(int value, int weight = 1)
        {
            try
            {
                if (_mods.Exists(mod => mod.Record.Group == "PhysicalDamage" &&
                                        Average(mod.StatValue) >= value))
                    _totalWeight += weight;
            }
            catch
            {
                // ignored
            }
        }

        protected void CheckWED(int value, int weight = 1)
        {
            if (OneSimpleMod("IncreasedWeaponElementalDamagePercent", value))
                _totalWeight += weight;
        }

        protected void CheckCritMultiGlobal(int value, int weight = 1)
        {
            if (OneSimpleMod("CriticalStrikeMultiplier", value))
                _totalWeight += weight;
        }

        protected void CheckCritMultiSpell(int value, int weight = 1)
        {
            if (OneSimpleMod("SpellCriticalStrikeMultiplier", value))
                _totalWeight += weight;
        }

        protected void CheckGlobalCritChance(int value, int weight = 1)
        {
            if (OneSimpleMod("CriticalStrikeChanceIncrease", value))
                _totalWeight += weight;
        }

        protected void CheckMana(int value, int weight = 1)
        {
            if (OneSimpleMod("IncreasedMana", value))
                _totalWeight += weight;
        }

        protected void CheckMoveSpeed(int value, int weight = 1)
        {
            if (OneSimpleMod("MovementVelocity", value))
                _totalWeight += weight;
        }

        protected void CheckAttackSpeed(int value, int weight = 1)
        {
            if (OneSimpleMod("IncreasedAttackSpeed", value))
                _totalWeight += weight;
        }

        protected void CheckEnergyShieldJewel(int value, int weight = 1)
        {
            try
            {
                foreach (var mod in _mods.Where(mod => mod.Record.Group.Contains("EnergyShield")))
                {
                    var tier = mod.Tier > 0 ? mod.Tier : FixTierEs(mod.Record.Key);

                    if (tier <= value) _totalWeight += weight;
                }
            }
            catch
            {
                // ignored
            }
        }

        protected void CheckSpellElemDamage(int value, int weight = 1)
        {
            try
            {
                var totalValue = 0;

                foreach (var mod in _mods.Where(mod =>
                    mod.Record.Group.Equals("SpellDamage") && _incElemDmg.Contains(mod.Record.Group)))
                    totalValue += mod.StatValue[0];

                if (totalValue >= value)
                    _totalWeight += weight;
            }
            catch
            {
                // ignored
            }
        }

        protected void CheckFlaskReduced(int value, int weight = 1)
        {
            if (OneSimpleMod("BeltFlaskCharges", value))
                _totalWeight += weight;
        }

        protected void CheckFlaskDuration(int value, int weight = 1)
        {
            if (OneSimpleMod("BeltFlaskDuration", value))
                _totalWeight += weight;
        }

        protected void CheckCastSpeed(int value, int weight = 1)
        {
            if (OneSimpleMod("IncreasedCastSpeed", value))
                _totalWeight += weight;
        }

        protected void CheckSpellCrit(int value, int weight = 1)
        {
            if (OneSimpleMod("SpellCriticalStrikeChanceIncrease", value))
                _totalWeight += weight;
        }

        protected void CheckSpellAddedElem(int value, int weight = 1)
        {
            try
            {
                var totalValue = 0;

                foreach (var mod in _mods.Where(mod => mod.Record.Group.Contains("SpellAddedElementalDamage")))
                    totalValue += Average(mod.StatValue);

                if (totalValue >= value)
                    _totalWeight += weight;
            }
            catch
            {
                // ignored
            }
        }

        protected void CheckDOTMultiplier(int value, int weight = 1)
        {
            if (OneSimpleMod("DamageOverTimeMultiplier", value))
                _totalWeight += weight;
        }

        protected float GetWeaponElemDamage()
        {
            var component = _item.GetComponent<Weapon>();
            var mods = _item.GetComponent<Mods>().ItemMods;

            var attackSpeed = 1f / (component.AttackTime / 1000f);

            attackSpeed *= 1f + mods.GetStatValue("LocalIncreasedAttackSpeed") / 100f;

            var phyDmg = (component.DamageMin + component.DamageMax) / 2f +
                         mods.GetAverageStatValue("LocalAddedPhysicalDamage");

            phyDmg *= 1f + (mods.GetStatValue("LocalIncreasedPhysicalDamagePercent") + 20) / 100f;

            return phyDmg * attackSpeed;
        }

        protected float GetWeaponPhysDamage()
        {
            var component = _item.GetComponent<Weapon>();
            var mods = _item.GetComponent<Mods>().ItemMods;

            var attackSpeed = 1f / (component.AttackTime / 1000f);

            attackSpeed *= 1f + mods.GetStatValue("LocalIncreasedAttackSpeed") / 100f;

            var elemDmg = mods.GetAverageStatValue("LocalAddedColdDamage")
                          + mods.GetAverageStatValue("LocalAddedFireDamage")
                          + mods.GetAverageStatValue("LocalAddedLightningDamage");

            return elemDmg * attackSpeed;
        }

        #endregion
    }

    #region Get item Stats

    public static class ModsExtension
    {
        public static float GetStatValue(this List<ItemMod> mods, string name)
        {
            try
            {
                var m = mods.FirstOrDefault(mod => mod.Name == name);
                return m?.Value1 ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        public static float GetAverageStatValue(this List<ItemMod> mods, string name)
        {
            try
            {
                var m = mods.FirstOrDefault(mod => mod.Name == name);
                return (m?.Value1 + m?.Value2) / 2 ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }

    #endregion
}