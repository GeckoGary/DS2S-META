﻿using PropertyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META
{
    public class DS2SHook : PHook, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public IntPtr BaseAddress => Process?.MainModule.BaseAddress ?? IntPtr.Zero;
        public string ID => Process?.Id.ToString() ?? "Not Hooked";

        private string _version;
        public string Version 
        { 
            get =>_version;
            private set
            {
                _version = value;
                OnPropertyChanged(nameof(Version));
            }
        }

        public static bool Reading { get; set; }

        private PHPointer BaseASetup;
        private PHPointer GiveSoulsFunc;
        private PHPointer ItemGiveFunc;
        private PHPointer ItemStruct2dDisplay;
        private PHPointer DisplayItem;

        private PHPointer BaseA;
        private PHPointer PlayerName;
        private PHPointer AvailableItemBag;
        private PHPointer ItemGiveWindow;
        private PHPointer PlayerBaseMisc;
        private PHPointer PlayerCtrl;
        private PHPointer PlayerPosition;
        private PHPointer PlayerGravity;
        private PHPointer PlayerParam;
        private PHPointer PlayerType;
        private PHPointer PlayerMapData;
        private PHPointer Bonfire;
        private PHPointer BonfireLevels;

        private PHPointer LevelUpSoulsParam;
        private PHPointer WeaponParam;
        private PHPointer WeaponReinforceParam;
        private PHPointer CustomAttrSpecParam;
        private PHPointer ArmorParam;
        private PHPointer ArmorReinforceParam;
        private PHPointer ItemParam;

        private PHPointer BaseBSetup;
        private PHPointer BaseB;
        private PHPointer Connection;

        private PHPointer Camera;
        private PHPointer Camera2;

        private PHPointer SpeedFactorAccel;

        public bool Loaded => PlayerCtrl != null && PlayerCtrl.Resolve() != IntPtr.Zero;

        public bool Focused => Hooked && User32.GetForegroundProcessID() == Process.Id;

        public DS2SHook(int refreshInterval, int minLifetime) :
            base(refreshInterval, minLifetime, p => p.MainWindowTitle == "DARK SOULS II")
        {
            Version = "Not Hooked";
            BaseASetup = RegisterAbsoluteAOB(DS2SOffsets.BaseAAob);
            SpeedFactorAccel = RegisterAbsoluteAOB(DS2SOffsets.SpeedFactorAccelOffset);
            GiveSoulsFunc = RegisterAbsoluteAOB(DS2SOffsets.GiveSoulsFunc);
            ItemGiveFunc = RegisterAbsoluteAOB(DS2SOffsets.ItemGiveFunc);
            ItemStruct2dDisplay = RegisterAbsoluteAOB(DS2SOffsets.ItemStruct2dDisplay);
            DisplayItem = RegisterAbsoluteAOB(DS2SOffsets.DisplayItem); 

            BaseBSetup = RegisterAbsoluteAOB(DS2SOffsets.BaseBAoB);

            OnHooked += DS2Hook_OnHooked;
            OnUnhooked += DS2Hook_OnUnhooked;
        }
        private void DS2Hook_OnHooked(object sender, PHEventArgs e)
        {
            Version = "Vanilla";

            BaseA = CreateBasePointer(BasePointerFromSetupPointer(BaseASetup));
            if (BaseA.Resolve() == IntPtr.Zero)
            {
                BaseASetup = RegisterAbsoluteAOB(DS2SOffsets.BaseABabyJumpAoB);
                RescanAOB();
                BaseA = CreateBasePointer(BasePointerFromSetupBabyJ(BaseASetup));
                Version = "BabyJump Dll";
            }

            PlayerName = CreateChildPointer(BaseA, (int)DS2SOffsets.PlayerNameOffset);
            AvailableItemBag = CreateChildPointer(PlayerName, (int)DS2SOffsets.AvailableItemBagOffset, (int)DS2SOffsets.AvailableItemBagOffset);
            ItemGiveWindow = CreateChildPointer(BaseA, (int)DS2SOffsets.ItemGiveWindowPointer);
            PlayerBaseMisc = CreateChildPointer(PlayerName, (int)DS2SOffsets.PlayerBaseMiscOffset);
            PlayerCtrl = CreateChildPointer(BaseA, (int)DS2SOffsets.PlayerCtrlOffset);
            PlayerPosition = CreateChildPointer(PlayerCtrl, (int)DS2SOffsets.PlayerPositionOffset1, (int)DS2SOffsets.PlayerPositionOffset2);
            PlayerGravity = CreateChildPointer(PlayerCtrl, (int)DS2SOffsets.PlayerMapDataOffset1);
            PlayerParam = CreateChildPointer(PlayerCtrl, (int)DS2SOffsets.PlayerParamOffset);
            PlayerType = CreateChildPointer(PlayerCtrl, (int)DS2SOffsets.PlayerTypeOffset);
            PlayerMapData = CreateChildPointer(PlayerGravity, (int)DS2SOffsets.PlayerMapDataOffset2, (int)DS2SOffsets.PlayerMapDataOffset3);
            Bonfire = CreateChildPointer(BaseA, (int)DS2SOffsets.BonfireOffset);
            BonfireLevels = CreateChildPointer(Bonfire, (int)DS2SOffsets.BonfireLevelsOffset1, (int)DS2SOffsets.BonfireLevelsOffset2);

            LevelUpSoulsParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.LevelUpSoulsParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            WeaponParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.WeaponParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            WeaponReinforceParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.WeaponReinforceParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            CustomAttrSpecParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.CustomAttrSpecParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            ArmorParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.ArmorParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            ArmorReinforceParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.ArmorReinforceParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            ItemParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset3, (int)DS2SOffsets.ItemParamOffset, (int)DS2SOffsets.ParamDataOffset2);

            BaseB = CreateBasePointer(BasePointerFromSetupPointer(BaseBSetup));
            Connection = CreateChildPointer(BaseB, (int)DS2SOffsets.ConnectionOffset);

            Camera = CreateBasePointer(Handle + 0x160B8D0, (int)DS2SOffsets.CameraOffset1);
            Camera2 = CreateChildPointer(Camera, (int)DS2SOffsets.CameraOffset2);

            GetLevelRequirements();
            WeaponParamOffsetDict = BuildOffsetDictionary(WeaponParam, "WEAPON_PARAM");
            WeaponReinforceParamOffsetDict = BuildOffsetDictionary(WeaponReinforceParam, "WEAPON_REINFORCE_PARAM");
            CustomAttrOffsetDict = BuildOffsetDictionary(CustomAttrSpecParam, "CUSTOM_ATTR_SPEC_PARAM");
            ArmorReinforceParamOffsetDict = BuildOffsetDictionary(ArmorReinforceParam, "ARMOR_REINFORCE_PARAM");
            ItemParamOffsetDict = BuildOffsetDictionary(ItemParam, "ITEM_PARAM");
            UpdateStatsProperties();
        }

       

        

        private void DS2Hook_OnUnhooked(object sender, PHEventArgs e)
        {
            Version = "Not Hooked";
        }

        public void UpdateMainProperties()
        {
            OnPropertyChanged(nameof(ID));
            OnPropertyChanged(nameof(Online));
            //OnPropertyChanged(nameof(LastBonfireObj));
        }
        public void UpdateStatsProperties()
        {
            OnPropertyChanged(nameof(SoulLevel));
            OnPropertyChanged(nameof(Souls));
            OnPropertyChanged(nameof(SoulMemory));
            OnPropertyChanged(nameof(SoulMemory2));
            OnPropertyChanged(nameof(HollowLevel));
            OnPropertyChanged(nameof(SinnerLevel));
            OnPropertyChanged(nameof(SinnerPoints));
            OnPropertyChanged(nameof(Vigor));
            OnPropertyChanged(nameof(Endurance));
            OnPropertyChanged(nameof(Vitality));
            OnPropertyChanged(nameof(Attunement));
            OnPropertyChanged(nameof(Strength));
            OnPropertyChanged(nameof(Dexterity));
            OnPropertyChanged(nameof(Adaptability));
            OnPropertyChanged(nameof(Intelligence));
            OnPropertyChanged(nameof(Faith));
        }

        public void UpdatePlayerProperties()
        {
            OnPropertyChanged(nameof(Health));
            OnPropertyChanged(nameof(HealthMax));
            OnPropertyChanged(nameof(HealthCap));
            OnPropertyChanged(nameof(Stamina));
            OnPropertyChanged(nameof(MaxStamina));
            OnPropertyChanged(nameof(TeamType));
            OnPropertyChanged(nameof(CharType));
            OnPropertyChanged(nameof(PosX));
            OnPropertyChanged(nameof(PosY));
            OnPropertyChanged(nameof(PosZ));
            OnPropertyChanged(nameof(AngX));
            OnPropertyChanged(nameof(AngY));
            OnPropertyChanged(nameof(AngZ));
            OnPropertyChanged(nameof(Collision));
            OnPropertyChanged(nameof(Gravity));
            OnPropertyChanged(nameof(StableX));
            OnPropertyChanged(nameof(StableY));
            OnPropertyChanged(nameof(StableZ));
        }

        public void UpdateBonfireProperties()
        {
            OnPropertyChanged(nameof(FireKeepersDwelling));
            OnPropertyChanged(nameof(TheFarFire));
            OnPropertyChanged(nameof(TheCrestfallensRetreat));
            OnPropertyChanged(nameof(CardinalTower));
            OnPropertyChanged(nameof(SoldiersRest));
            OnPropertyChanged(nameof(ThePlaceUnbeknownst));
            OnPropertyChanged(nameof(HeidesRuin));
            OnPropertyChanged(nameof(TowerofFlame));
            OnPropertyChanged(nameof(TheBlueCathedral));
            OnPropertyChanged(nameof(UnseenPathtoHeide));
            OnPropertyChanged(nameof(ExileHoldingCells));
            OnPropertyChanged(nameof(McDuffsWorkshop));
            OnPropertyChanged(nameof(ServantsQuarters));
            OnPropertyChanged(nameof(StraidsCell));
            OnPropertyChanged(nameof(TheTowerApart));
            OnPropertyChanged(nameof(TheSaltfort));
            OnPropertyChanged(nameof(UpperRamparts));
            OnPropertyChanged(nameof(UndeadRefuge));
            OnPropertyChanged(nameof(BridgeApproach));
            OnPropertyChanged(nameof(UndeadLockaway));
            OnPropertyChanged(nameof(UndeadPurgatory));
            OnPropertyChanged(nameof(PoisonPool));
            OnPropertyChanged(nameof(TheMines));
            OnPropertyChanged(nameof(LowerEarthenPeak));
            OnPropertyChanged(nameof(CentralEarthenPeak));
            OnPropertyChanged(nameof(UpperEarthenPeak));
            OnPropertyChanged(nameof(ThresholdBridge));
            OnPropertyChanged(nameof(IronhearthHall));
            OnPropertyChanged(nameof(EygilsIdol));
            OnPropertyChanged(nameof(BelfrySolApproach));
            OnPropertyChanged(nameof(OldAkelarre));
            OnPropertyChanged(nameof(RuinedForkRoad));
            OnPropertyChanged(nameof(ShadedRuins));
            OnPropertyChanged(nameof(GyrmsRespite));
            OnPropertyChanged(nameof(OrdealsEnd));
            OnPropertyChanged(nameof(RoyalArmyCampsite));
            OnPropertyChanged(nameof(ChapelThreshold));
            OnPropertyChanged(nameof(LowerBrightstoneCove));
            OnPropertyChanged(nameof(HarvalsRestingPlace));
            OnPropertyChanged(nameof(GraveEntrance));
            OnPropertyChanged(nameof(UpperGutter));
            OnPropertyChanged(nameof(CentralGutter));
            OnPropertyChanged(nameof(HiddenChamber));
            OnPropertyChanged(nameof(BlackGulchMouth));
            OnPropertyChanged(nameof(KingsGate));
            OnPropertyChanged(nameof(UnderCastleDrangleic));
            OnPropertyChanged(nameof(ForgottenChamber));
            OnPropertyChanged(nameof(CentralCastleDrangleic));
            OnPropertyChanged(nameof(TowerofPrayer));
            OnPropertyChanged(nameof(CrumbledRuins));
            OnPropertyChanged(nameof(RhoysRestingPlace));
            OnPropertyChanged(nameof(RiseoftheDead));
            OnPropertyChanged(nameof(UndeadCryptEntrance));
            OnPropertyChanged(nameof(UndeadDitch));
            OnPropertyChanged(nameof(Foregarden));
            OnPropertyChanged(nameof(RitualSite));
            OnPropertyChanged(nameof(DragonAerie));
            OnPropertyChanged(nameof(ShrineEntrance));
            OnPropertyChanged(nameof(SanctumWalk));
            OnPropertyChanged(nameof(PriestessChamber));
            OnPropertyChanged(nameof(HiddenSanctumChamber));
            OnPropertyChanged(nameof(LairoftheImperfect));
            OnPropertyChanged(nameof(SanctumInterior));
            OnPropertyChanged(nameof(TowerofPrayer));
            OnPropertyChanged(nameof(SanctumNadir));
            OnPropertyChanged(nameof(ThroneFloor));
            OnPropertyChanged(nameof(UpperFloor));
            OnPropertyChanged(nameof(Foyer));
            OnPropertyChanged(nameof(LowermostFloor));
            OnPropertyChanged(nameof(TheSmelterThrone));
            OnPropertyChanged(nameof(IronHallwayEntrance));
            OnPropertyChanged(nameof(OuterWall));
            OnPropertyChanged(nameof(AbandonedDwelling));
            OnPropertyChanged(nameof(ExpulsionChamber));
            OnPropertyChanged(nameof(InnerWall));
            OnPropertyChanged(nameof(LowerGarrison));
            OnPropertyChanged(nameof(GrandCathedral));
        }

        public void UpdateInternalProperties()
        {
            //var addr = SpeedFactorAccel.Resolve();
            //var bytes = SpeedFactorAccel.ReadBytes(0x0, 24);

            //foreach (var num in bytes)
            //{
            //    Debug.Write($"{num.ToString("X2")} ");
            //}
            OnPropertyChanged(nameof(Head));
            OnPropertyChanged(nameof(Chest));
            OnPropertyChanged(nameof(Arms));
            OnPropertyChanged(nameof(Legs));
            OnPropertyChanged(nameof(RightHand1));
            OnPropertyChanged(nameof(RightHand2));
            OnPropertyChanged(nameof(RightHand3));
            OnPropertyChanged(nameof(LeftHand1));
            OnPropertyChanged(nameof(LeftHand2));
            OnPropertyChanged(nameof(LeftHand3));
        }

        public IntPtr BasePointerFromSetupPointer(PHPointer pointer)
        {
            var readInt = pointer.ReadInt32(DS2SOffsets.BasePtrOffset1);
            return pointer.ReadIntPtr(readInt + DS2SOffsets.BasePtrOffset2);
        }

        public IntPtr BasePointerFromSetupBabyJ(PHPointer pointer)
        {
            return pointer.ReadIntPtr(0x0121D4D0 + DS2SOffsets.BasePtrOffset2);
        }



        #region Player
        public int Health
        {
            get => Loaded ? PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerCtrl.HP) : 0;
            set 
            {
                if (Reading || !Loaded) return;
                PlayerCtrl.WriteInt32((int)DS2SOffsets.PlayerCtrl.HP, value);
            }
        }
        public int HealthMax
        {
            get => Loaded ? PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerCtrl.HPMax) : 0;
            set => PlayerCtrl.WriteInt32((int)DS2SOffsets.PlayerCtrl.HPMax, value);
        }
        public int HealthCap
        {
            get 
            {
                if (!Loaded) return 0;
                var cap = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerCtrl.HPCap);
                return cap < HealthMax ? cap : HealthMax;
            }
            set => PlayerCtrl.WriteInt32((int)DS2SOffsets.PlayerCtrl.HPCap, value);
        }
        public float Stamina
        {
            get => Loaded ? PlayerCtrl.ReadSingle((int)DS2SOffsets.PlayerCtrl.SP) : 0;
            set 
            { 
                if (Reading || !Loaded) return;
                PlayerCtrl.WriteSingle((int)DS2SOffsets.PlayerCtrl.SP, value);
            }
        }
        public float MaxStamina
        {
            get => Loaded ? PlayerCtrl.ReadSingle((int)DS2SOffsets.PlayerCtrl.SPMax) : 0;
            set => PlayerCtrl.WriteSingle((int)DS2SOffsets.PlayerCtrl.SPMax, value);
        }
        public byte NetworkPhantomID
        {
            get => Loaded ? PlayerType.ReadByte((int)DS2SOffsets.PlayerType.ChrNetworkPhantomId) : (byte)0;
            set => PlayerType.WriteByte((int)DS2SOffsets.PlayerType.ChrNetworkPhantomId, value);
        }
        public byte TeamType
        {
            get => Loaded ? PlayerType.ReadByte((int)DS2SOffsets.PlayerType.TeamType) : (byte)0;
            //set => PlayerType.WriteByte((int)DS2SOffsets.PlayerType.TeamType, value);
        }
        public byte CharType
        {
            get => Loaded ? PlayerType.ReadByte((int)DS2SOffsets.PlayerType.CharType) : (byte)0;
            //set => PlayerType.WriteByte((int)DS2SOffsets.PlayerType.CharType, value);
        }
        public float PosX
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.PosX) : 0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerPosition.WriteSingle((int)DS2SOffsets.PlayerPosition.PosX, value);
            }
        }
        public float PosY
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.PosY) : 0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerPosition.WriteSingle((int)DS2SOffsets.PlayerPosition.PosY, value);
            }
        }
        public float PosZ
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.PosZ) : 0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerPosition.WriteSingle((int)DS2SOffsets.PlayerPosition.PosZ, value);
            }
        }
        public float AngX
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.AngX) : 0;
            set => PlayerPosition.WriteSingle((int)DS2SOffsets.PlayerPosition.AngX, value);
        }
        public float AngY
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.AngY) : 0;
            set => PlayerPosition.WriteSingle((int)DS2SOffsets.PlayerPosition.AngY, value);
        }
        public float AngZ
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.AngZ) : 0;
            set => PlayerPosition.WriteSingle((int)DS2SOffsets.PlayerPosition.AngZ, value);
        }
        public float StableX
        {
            get => Loaded ? PlayerMapData.ReadSingle((int)DS2SOffsets.PlayerMapData.WarpX1) : 0;
            set
            {
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpX1, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpX2, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpX3, value);
            }
        }
        public float StableY
        {
            get => Loaded ? PlayerMapData.ReadSingle((int)DS2SOffsets.PlayerMapData.WarpY1) : 0;
            set
            {
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpY1, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpY2, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpY3, value);
            }
        }
        public float StableZ
        {
            get => Loaded ? PlayerMapData.ReadSingle((int)DS2SOffsets.PlayerMapData.WarpZ1) : 0;
            set
            {
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpZ1, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpZ2, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpZ3, value);
            }
        }
        public byte[] CameraData
        {
            get => Camera2.ReadBytes((int)DS2SOffsets.Camera.CamStart, 0x4C);
            set => Camera2.WriteBytes((int)DS2SOffsets.Camera.CamStart, value);
        }
        public float CamX
        {
            get => Camera2.ReadSingle((int)DS2SOffsets.Camera.CamX);
            set => Camera2.WriteSingle((int)DS2SOffsets.Camera.CamX, value);
        }
        public float CamY
        {
            get => Camera2.ReadSingle((int)DS2SOffsets.Camera.CamY);
            set => Camera2.WriteSingle((int)DS2SOffsets.Camera.CamY, value);
        }
        public float CamZ
        {
            get => Camera2.ReadSingle((int)DS2SOffsets.Camera.CamZ);
            set => Camera2.WriteSingle((int)DS2SOffsets.Camera.CamZ, value);
        }
        public float Speed
        {
            set 
            {
                if (!Loaded) return;
                PlayerCtrl.WriteSingle((int)DS2SOffsets.PlayerCtrl.SpeedModifier, value); 
            }
        }
        public bool Gravity
        {
            get => Loaded ? !PlayerGravity.ReadBoolean((int)DS2SOffsets.Gravity.Gravity) : true;
            set => PlayerGravity.WriteBoolean((int)DS2SOffsets.Gravity.Gravity, !value);
        }
        public bool Collision
        {
            get => Loaded ? NetworkPhantomID != 18 && NetworkPhantomID != 19 : true;
            set
            {
                if (Reading || !Loaded) return;
                if (value)
                    NetworkPhantomID = 0;
                else
                    NetworkPhantomID = 18;
            }
        }
        public int LastBonfireID
        {
            get => Loaded ? Bonfire.ReadInt32((int)DS2SOffsets.Bonfire.LastSetBonfire) : 0;
            set => Bonfire.WriteInt32((int)DS2SOffsets.Bonfire.LastSetBonfire, value);
        }
        //public DS2SBonfire LastBonfireObj
        //{
        //    get => Loaded ? DS2SBonfire.All.FirstOrDefault(b => b.ID == LastBonfireID) : new DS2SBonfire(0, "None");
        //    //set => Bonfire.WriteInt32((int)DS2SOffsets.Bonfire.LastSetBonfire, value.ID);
        //}

        public bool Online
        {
            get => Hooked && Connection != null ? Connection.ReadInt32((int)DS2SOffsets.Connection.Online) > 0 : false;
        }
        #endregion

        #region Stats
        public string Name
        {
            get => Loaded ? PlayerName.ReadString((int)DS2SOffsets.PlayerName.Name, Encoding.Unicode, 0x22) : "";
            set
            {
                if (Reading || !Loaded) return;
                if (Name == value) return;
                PlayerName.WriteString((int)DS2SOffsets.PlayerName.Name, Encoding.Unicode, 0x22, value);
                OnPropertyChanged(nameof(Name));
            }
        }

        public byte Class
        {
            get => Loaded ? PlayerBaseMisc.ReadByte((int)DS2SOffsets.PlayerBaseMisc.Class) : (byte)255;
            set
            {
                if (Reading || !Loaded) return;
                PlayerBaseMisc.WriteByte((int)DS2SOffsets.PlayerBaseMisc.Class, value);
            }
        }
        public int SoulLevel
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2SOffsets.Attributes.SoulLevel) : 0;
            set => PlayerParam.WriteInt32((int)DS2SOffsets.Attributes.SoulLevel, value);
        }
        public int SoulMemory
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2SOffsets.PlayerParam.SoulMemory) : 0;
            set => PlayerParam.WriteInt32((int)DS2SOffsets.PlayerParam.SoulMemory, value);
        }
        public int SoulMemory2
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2SOffsets.PlayerParam.SoulMemory2) : 0;
            set => PlayerParam.WriteInt32((int)DS2SOffsets.PlayerParam.SoulMemory2, value);
        }
        public byte SinnerLevel
        {
            get => Loaded ? PlayerParam.ReadByte((int)DS2SOffsets.PlayerParam.SinnerLevel) : (byte)0;
            set => PlayerParam.WriteByte((int)DS2SOffsets.PlayerParam.SinnerLevel, value);
        }
        public byte SinnerPoints
        {
            get => Loaded ? PlayerParam.ReadByte((int)DS2SOffsets.PlayerParam.SinnerPoints) : (byte)0;
            set => PlayerParam.WriteByte((int)DS2SOffsets.PlayerParam.SinnerPoints, value);
        }
        public byte HollowLevel
        {
            get => Loaded ? PlayerParam.ReadByte((int)DS2SOffsets.PlayerParam.HollowLevel) : (byte)0;
            set => PlayerParam.WriteByte((int)DS2SOffsets.PlayerParam.HollowLevel, value);
        }
        public int Souls
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2SOffsets.PlayerParam.Souls) : 0;
        }

        public short Vigor
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.VGR) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.VGR, value);
                UpdateSoulLevel();
            }
        }
        public short Endurance
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.END) : (short)0;
            set 
            { 
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.END, value);
                UpdateSoulLevel();
            }
        }
        public short Vitality
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.VIT) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.VIT, value);
                UpdateSoulLevel();
            }
        }
        public short Attunement
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.ATN) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.ATN, value);
                UpdateSoulLevel();
            }
        }
        public short Strength
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.STR) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.STR, value);
                UpdateSoulLevel();
            }
        }
        public short Dexterity
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.DEX) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.DEX, value);
                UpdateSoulLevel();
            }
        }
        public short Adaptability
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.ADP) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.ADP, value);
                UpdateSoulLevel();
            }
        }
        public short Intelligence
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.INT) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.INT, value);
                UpdateSoulLevel();
            }
        }
        public short Faith
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.FTH) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.FTH, value);
                UpdateSoulLevel();
            }
        }
        public void GiveSouls(int souls)
        {
            var asm = (byte[])DS2SAssembly.AddSouls.Clone();

            var bytes = BitConverter.GetBytes(PlayerParam.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x6, 8);
            bytes = BitConverter.GetBytes(souls);
            Array.Copy(bytes, 0, asm, 0x11, 4);
            bytes = BitConverter.GetBytes(GiveSoulsFunc.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x17, 8);
            Execute(asm);
        }
        private void UpdateSoulLevel()
        {
            var charClass = DS2SClass.All.FirstOrDefault(c => c.ID == Class);
            if (charClass == null) return;

            var soulLevel = GetSoulLevel(charClass);
            SoulLevel = soulLevel;
            var reqSoulMemory = GetRequiredSoulMemory(soulLevel, charClass.SoulLevel);
            if (reqSoulMemory > SoulMemory)
            {
                SoulMemory = reqSoulMemory;
                SoulMemory2 = reqSoulMemory;
            }
        }
        private int GetSoulLevel(DS2SClass charClass)
        {
            int sl = charClass.SoulLevel;
            sl += Vigor - charClass.Vigor;
            sl += Attunement - charClass.Attunement;
            sl += Vitality - charClass.Vitality;
            sl += Endurance - charClass.Endurance;
            sl += Strength - charClass.Strength;
            sl += Dexterity - charClass.Dexterity;
            sl += Adaptability - charClass.Adaptability;
            sl += Intelligence - charClass.Intelligence;
            sl += Faith - charClass.Faith;
            return sl;
        }
        public void ResetSoulMemory()
        {
            var charClass = DS2SClass.All.FirstOrDefault(c => c.ID == Class);
            if (charClass == null) return;

            var soulLevel = GetSoulLevel(charClass);
            var reqSoulMemory = GetRequiredSoulMemory(soulLevel, charClass.SoulLevel);

            SoulMemory = reqSoulMemory;
            SoulMemory2 = reqSoulMemory;
        }
        private int GetRequiredSoulMemory(int SL, int baseSL)
        {
            int soulMemory = 0;
            for (int i = baseSL; i < SL; i++)
            {
                var index = i <= 850 ? i : 850;
                soulMemory += Levels[index];
            }
            return soulMemory;
        }

        public static List<int> Levels;
        private void GetLevelRequirements()
        {
            Levels = new List<int>();
            var paramName = LevelUpSoulsParam.ReadString(0xC, Encoding.UTF8, 0x18);
            if (paramName != "CHR_LEVEL_UP_SOULS_PARAM")
                throw new InvalidOperationException("Incorrect Param Pointer: LEVEL_UP_SOULS_PARAM");

            var tableLength = LevelUpSoulsParam.ReadInt32((int)DS2SOffsets.Param.TableLength);
            var paramID = 0x40;
            var paramOffset = 0x48;
            var nextParam = 0x18;
            var slOffset = 0x8;

            while (paramID < tableLength)
            {
                var soulReqOffset = LevelUpSoulsParam.ReadInt32(paramOffset);
                var soulReq = LevelUpSoulsParam.ReadInt32(soulReqOffset + slOffset);
                Levels.Add(soulReq);

                paramID += nextParam;
                paramOffset += nextParam;
            }
        }

        #endregion

        #region Items
        public void GetItem(int item, short amount, byte upgrade, byte infusion)
        {
            if (Properties.Settings.Default.SilentItemGive)
                GiveItemSilently(item, amount, upgrade, infusion);
            else
                GiveItem(item, amount, upgrade, infusion);
        }

        private void GiveItem(int item, short amount, byte upgrade, byte infusion)
        {
            var itemStruct = Allocate(0x8A);

            Kernel32.WriteBytes(Handle, itemStruct + 0x4, BitConverter.GetBytes(item));
            Kernel32.WriteBytes(Handle, itemStruct + 0x8, BitConverter.GetBytes(float.MaxValue));
            Kernel32.WriteBytes(Handle, itemStruct + 0xC, BitConverter.GetBytes(amount));
            Kernel32.WriteByte(Handle, itemStruct + 0xE, upgrade);
            Kernel32.WriteByte(Handle, itemStruct + 0xF, infusion);

            var asm = (byte[])DS2SAssembly.GetItem.Clone();

            var bytes = BitConverter.GetBytes(0x1);
            Array.Copy(bytes, 0, asm, 0x9, 4);
            bytes = BitConverter.GetBytes(itemStruct.ToInt64());
            Array.Copy(bytes, 0, asm, 0xF, 8);
            bytes = BitConverter.GetBytes(AvailableItemBag.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x1C, 8);
            bytes = BitConverter.GetBytes(ItemGiveFunc.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x29, 8);
            bytes = BitConverter.GetBytes(0x1);
            Array.Copy(bytes, 0, asm, 0x36, 4);
            bytes = BitConverter.GetBytes(itemStruct.ToInt64());
            Array.Copy(bytes, 0, asm, 0x3C, 8);
            bytes = BitConverter.GetBytes(ItemStruct2dDisplay.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x54, 8);
            bytes = BitConverter.GetBytes(ItemGiveWindow.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x66, 8);
            bytes = BitConverter.GetBytes(DisplayItem.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x70, 8);

            Execute(asm);
            Free(itemStruct);
        }

        private void GiveItemSilently(int item, short amount, byte upgrade, byte infusion)
        {
            var itemStruct = Allocate(0x8A);
            Kernel32.WriteBytes(Handle, itemStruct + 0x4, BitConverter.GetBytes(item));
            Kernel32.WriteBytes(Handle, itemStruct + 0x8, BitConverter.GetBytes(float.MaxValue));
            Kernel32.WriteBytes(Handle, itemStruct + 0xC, BitConverter.GetBytes(amount));
            Kernel32.WriteByte(Handle, itemStruct + 0xE, upgrade);
            Kernel32.WriteByte(Handle, itemStruct + 0xF, infusion);

            var asm = (byte[])DS2SAssembly.GetItemNoMenu.Clone();

            var bytes = BitConverter.GetBytes(0x1);
            Array.Copy(bytes, 0, asm, 0x6, 4);
            bytes = BitConverter.GetBytes(itemStruct.ToInt64());
            Array.Copy(bytes, 0, asm, 0xC, 8);
            bytes = BitConverter.GetBytes(AvailableItemBag.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x19, 8);
            bytes = BitConverter.GetBytes(ItemGiveFunc.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x26, 8);

            Execute(asm);
            Free(itemStruct);
        }
        #endregion

        #region Params

        private static Dictionary<int, int> WeaponParamOffsetDict;
        private static Dictionary<int, int> WeaponReinforceParamOffsetDict;
        private static Dictionary<int, int> CustomAttrOffsetDict;
        private static Dictionary<int, int> ArmorParamOffsetDict;
        private static Dictionary<int, int> ArmorReinforceParamOffsetDict;
        private static Dictionary<int, int> ItemParamOffsetDict;

        private Dictionary<int, int> BuildOffsetDictionary(PHPointer pointer, string expectedParamName)
        {
            var dictionary = new Dictionary<int, int>();
            var paramName = pointer.ReadString((int)DS2SOffsets.Param.ParamName, Encoding.UTF8, 0x18);
            if (paramName != expectedParamName)
                throw new InvalidOperationException($"Incorrect Param Pointer: {expectedParamName}");

            var tableLength = pointer.ReadInt32((int)DS2SOffsets.Param.TableLength);
            var paramID = 0x40;
            var paramOffset = 0x48;
            var nextParam = 0x18;

            while (paramID < tableLength)
            {
                var weaponID = pointer.ReadInt32(paramID);
                var weaponParamOffset = pointer.ReadInt32(paramOffset);
                dictionary.Add(weaponID, weaponParamOffset);

                paramID += nextParam;
                paramOffset += nextParam;
            }

            return dictionary;
        }

        internal int GetMaxUpgrade(DS2SItem item)
        {
            switch (item.Type)
            {
                case DS2SItem.ItemType.Weapon:
                    return GetWeaponMaxUpgrade(item.ID);
                case DS2SItem.ItemType.Armor:
                    return GetArmorMaxUpgrade(item.ID);
                case DS2SItem.ItemType.Item:
                case DS2SItem.ItemType.Ring:
                    return 0;
            }

            return 0;
        }

        internal int GetMaxQuantity(DS2SItem item)
        {
            switch (item.Type)
            {
                case DS2SItem.ItemType.Ring:
                case DS2SItem.ItemType.Weapon:
                case DS2SItem.ItemType.Armor:
                    return 1;
                case DS2SItem.ItemType.Item:
                    return GetMaxItemQuantity(item.ID);
            }

            return 0;
        }
        private int GetArmorMaxUpgrade(int id)
        {
            return ArmorReinforceParam.ReadInt32(ArmorReinforceParamOffsetDict[id - 10000000] + (int)DS2SOffsets.ArmorReinforceParam.MaxUpgrade);
        }
        private int GetWeaponMaxUpgrade(int id)
        {
            var reinforceParamID = WeaponParam.ReadInt32(WeaponParamOffsetDict[id] + (int)DS2SOffsets.WeaponParam.ReinforceID);
            return WeaponReinforceParam.ReadInt32(WeaponReinforceParamOffsetDict[reinforceParamID] + (int)DS2SOffsets.WeaponReinforceParam.MaxUpgrade);
        }
        private int GetMaxItemQuantity(int id)
        {
            return ItemParam.ReadInt16(ItemParamOffsetDict[id] + (int)DS2SOffsets.ItemParam.MaxHeld);
        }

        internal List<DS2SInfusion> GetWeaponInfusions(int id)
        {
            var infusions = new List<DS2SInfusion>();
            var reinforceParamID = WeaponParam.ReadInt32(WeaponParamOffsetDict[id] + (int)DS2SOffsets.WeaponParam.ReinforceID);
            var customAttrID = WeaponReinforceParam.ReadInt32(WeaponReinforceParamOffsetDict[reinforceParamID] + (int)DS2SOffsets.WeaponReinforceParam.CustomAttrID);
            var bitField = CustomAttrSpecParam.ReadInt32(CustomAttrOffsetDict[customAttrID]);

            if (bitField == 0)
                return new List<DS2SInfusion>() { DS2SInfusion.Normal };

            if ((bitField & 1) != 0)
                infusions.Add(DS2SInfusion.Normal);

            if ((bitField & 2) != 0)
                infusions.Add(DS2SInfusion.Fire);

            if ((bitField & 4) != 0)
                infusions.Add(DS2SInfusion.Magic);

            if ((bitField & 8) != 0)
                infusions.Add(DS2SInfusion.Lightning);

            if ((bitField & 16) != 0)
                infusions.Add(DS2SInfusion.Dark);

            if ((bitField & 32) != 0)
                infusions.Add(DS2SInfusion.Poison);

            if ((bitField & 64) != 0)
                infusions.Add(DS2SInfusion.Bleed);

            if ((bitField & 128) != 0)
                infusions.Add(DS2SInfusion.Raw);

            if ((bitField & 256) != 0)
                infusions.Add(DS2SInfusion.Enchanted);

            if ((bitField & 512) != 0)
                infusions.Add(DS2SInfusion.Mundane);

            return infusions;
        }

        #endregion

        #region Bonfires
        public byte FireKeepersDwelling
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.FireKeepersDwelling) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.FireKeepersDwelling, value);
        }
        public byte TheFarFire
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.TheFarFire) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.TheFarFire, value);
        }
        public byte TheCrestfallensRetreat
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.CrestfallensRetreat) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.CrestfallensRetreat, value);
        }
        public byte CardinalTower
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.CardinalTower) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.CardinalTower, value);
        }
        public byte SoldiersRest
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.SoldiersRest) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.SoldiersRest, value);
        }
        public byte ThePlaceUnbeknownst
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ThePlaceUnbeknownst) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ThePlaceUnbeknownst, value);
        }
        public byte HeidesRuin
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.HeidesRuin) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.HeidesRuin, value);
        }
        public byte TowerofFlame
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.TowerofFlame) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.TowerofFlame, value);
        }
        public byte TheBlueCathedral
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.TheBlueCathedral) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.TheBlueCathedral, value);
        }
        public byte UnseenPathtoHeide
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UnseenPathtoHeide) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UnseenPathtoHeide, value);
        }
        public byte ExileHoldingCells
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ExileHoldingCells) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ExileHoldingCells, value);
        }
        public byte McDuffsWorkshop
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.McDuffsWorkshop) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.McDuffsWorkshop, value);
        }
        public byte ServantsQuarters
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ServantsQuarters) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ServantsQuarters, value);
        }
        public byte StraidsCell
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.StraidsCell) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.StraidsCell, value);
        }
        public byte TheTowerApart
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.TheTowerApart) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.TheTowerApart, value);
        }
        public byte TheSaltfort
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.TheSaltfort) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.TheSaltfort, value);
        }
        public byte UpperRamparts
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UpperRamparts) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UpperRamparts, value);
        }
        public byte UndeadRefuge
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UndeadRefuge) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UndeadRefuge, value);
        }
        public byte BridgeApproach
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.BridgeApproach) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.BridgeApproach, value);
        }
        public byte UndeadLockaway
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UndeadLockaway) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UndeadLockaway, value);
        }
        public byte UndeadPurgatory
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UndeadPurgatory) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UndeadPurgatory, value);
        }
        public byte PoisonPool
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.PoisonPool) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.PoisonPool, value);
        }
        public byte TheMines
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.TheMines) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.TheMines, value);
        }
        public byte LowerEarthenPeak
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.LowerEarthenPeak) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.LowerEarthenPeak, value);
        }
        public byte CentralEarthenPeak
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.CentralEarthenPeak) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.CentralEarthenPeak, value);
        }
        public byte UpperEarthenPeak
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UpperEarthenPeak) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UpperEarthenPeak, value);
        }
        public byte ThresholdBridge
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ThresholdBridge) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ThresholdBridge, value);
        }
        public byte IronhearthHall
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.IronhearthHall) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.IronhearthHall, value);
        }
        public byte EygilsIdol
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.EygilsIdol) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.EygilsIdol, value);
        }
        public byte BelfrySolApproach
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.BelfrySolApproach) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.BelfrySolApproach, value);
        }
        public byte OldAkelarre
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.OldAkelarre) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.OldAkelarre, value);
        }
        public byte RuinedForkRoad
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.RuinedForkRoad) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.RuinedForkRoad, value);
        }
        public byte ShadedRuins
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ShadedRuins) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ShadedRuins, value);
        }
        public byte GyrmsRespite
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.GyrmsRespite) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.GyrmsRespite, value);
        }
        public byte OrdealsEnd
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.OrdealsEnd) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.OrdealsEnd, value);
        }
        public byte RoyalArmyCampsite
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.RoyalArmyCampsite) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.RoyalArmyCampsite, value);
        }
        public byte ChapelThreshold
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ChapelThreshold) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ChapelThreshold, value);
        }
        public byte LowerBrightstoneCove
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.LowerBrightstoneCove) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.LowerBrightstoneCove, value);
        }
        public byte HarvalsRestingPlace
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.HarvalsRestingPlace) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.HarvalsRestingPlace, value);
        }
        public byte GraveEntrance
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.GraveEntrance) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.GraveEntrance, value);
        }
        public byte UpperGutter
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UpperGutter) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UpperGutter, value);
        }
        public byte CentralGutter
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.CentralGutter) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.CentralGutter, value);
        }
        public byte HiddenChamber
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.HiddenChamber) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.HiddenChamber, value);
        }
        public byte BlackGulchMouth
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.BlackGulchMouth) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.BlackGulchMouth, value);
        }
        public byte KingsGate
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.KingsGate) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.KingsGate, value);
        }
        public byte UnderCastleDrangleic
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UnderCastleDrangleic) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UnderCastleDrangleic, value);
        }
        public byte ForgottenChamber
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ForgottenChamber) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ForgottenChamber, value);
        }
        public byte CentralCastleDrangleic
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.CentralCastleDrangleic) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.CentralCastleDrangleic, value);
        }
        public byte TowerofPrayer
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.TowerofPrayer) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.TowerofPrayer, value);
        }
        public byte CrumbledRuins
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.CrumbledRuins) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.CrumbledRuins, value);
        }
        public byte RhoysRestingPlace
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.RhoysRestingPlace) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.RhoysRestingPlace, value);
        }
        public byte RiseoftheDead
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.RiseoftheDead) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.RiseoftheDead, value);
        }
        public byte UndeadCryptEntrance
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UndeadCryptEntrance) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UndeadCryptEntrance, value);
        }
        public byte UndeadDitch
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UndeadDitch) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UndeadDitch, value);
        }
        public byte Foregarden
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.Foregarden) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.Foregarden, value);
        }
        public byte RitualSite
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.RitualSite) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.RitualSite, value);
        }
        public byte DragonAerie
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.DragonAerie) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.DragonAerie, value);
        }
        public byte ShrineEntrance
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ShrineEntrance) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ShrineEntrance, value);
        }
        public byte SanctumWalk
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.SanctumWalk) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.SanctumWalk, value);
        }
        public byte PriestessChamber
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.PriestessChamber) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.PriestessChamber, value);
        }
        public byte HiddenSanctumChamber
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.HiddenSanctumChamber) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.HiddenSanctumChamber, value);
        }
        public byte LairoftheImperfect
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.LairoftheImperfect) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.LairoftheImperfect, value);
        }
        public byte SanctumInterior
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.SanctumInterior) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.SanctumInterior, value);
        }
        public byte TowerofPrayerDLC
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.TowerofPrayerDLC) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.TowerofPrayerDLC, value);
        }
        public byte SanctumNadir
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.SanctumNadir) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.SanctumNadir, value);
        }
        public byte ThroneFloor
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ThroneFloor) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ThroneFloor, value);
        }
        public byte UpperFloor
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.UpperFloor) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.UpperFloor, value);
        }
        public byte Foyer
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.Foyer) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.Foyer, value);
        }
        public byte LowermostFloor
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.LowermostFloor) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.LowermostFloor, value);
        }
        public byte TheSmelterThrone
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.TheSmelterThrone) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.TheSmelterThrone, value);
        }
        public byte IronHallwayEntrance
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.IronHallwayEntrance) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.IronHallwayEntrance, value);
        }
        public byte OuterWall
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.OuterWall) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.OuterWall, value);
        }
        public byte AbandonedDwelling
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.AbandonedDwelling) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.AbandonedDwelling, value);
        }
        public byte ExpulsionChamber
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.ExpulsionChamber) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.ExpulsionChamber, value);
        }
        public byte InnerWall
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.InnerWall) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.InnerWall, value);
        }
        public byte LowerGarrison
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.LowerGarrison) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.LowerGarrison, value);
        }
        public byte GrandCathedral
        {
            get => Loaded ? BonfireLevels.ReadByte((int)DS2SOffsets.BonfireLevels.GrandCathedral) : (byte)0;
            set => BonfireLevels.WriteByte((int)DS2SOffsets.BonfireLevels.GrandCathedral, value);
        }

        public void UnlockBonfires()
        {
            foreach (DS2SOffsets.BonfireLevels bonfire in Enum.GetValues(typeof(DS2SOffsets.BonfireLevels)))
            {
                var currentLevel = BonfireLevels.ReadByte((int)bonfire);

                if (bonfire == DS2SOffsets.BonfireLevels.FireKeepersDwelling)
                        continue;

                if (currentLevel == 0)
                    BonfireLevels.WriteByte((int)bonfire, 1);
            }
        }

        #endregion

        #region Internal

        public string Head
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.Head);

                if (DS2SItem.Items.ContainsKey(itemID + 10000000))
                    return DS2SItem.Items[itemID + 10000000];

                return "Unknown";
            }
        }
        public string Chest
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.Chest);

                if (DS2SItem.Items.ContainsKey(itemID + 10000000))
                    return DS2SItem.Items[itemID + 10000000];

                return "Unknown";
            }
        }
        public string Arms
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.Arms);

                if (DS2SItem.Items.ContainsKey(itemID + 10000000))
                    return DS2SItem.Items[itemID + 10000000];

                return "Unknown";
            }
        }
        public string Legs
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.Legs);

                if (DS2SItem.Items.ContainsKey(itemID + 10000000))
                    return DS2SItem.Items[itemID + 10000000];

                return "Unknown";
            }
        }
        public string RightHand1
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.RightHand1);

                if (DS2SItem.Items.ContainsKey(itemID))
                    return DS2SItem.Items[itemID];

                return "Unknown";
            }
        }
        public string RightHand2
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.RightHand2);

                if (DS2SItem.Items.ContainsKey(itemID))
                    return DS2SItem.Items[itemID];

                return "Unknown";
            }
        }
        public string RightHand3
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.RightHand3);

                if (DS2SItem.Items.ContainsKey(itemID))
                    return DS2SItem.Items[itemID];

                return "Unknown";
            }
        }
        public string LeftHand1
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.LeftHand1);

                if (DS2SItem.Items.ContainsKey(itemID))
                    return DS2SItem.Items[itemID];

                return "Unknown";
            }
        }
        public string LeftHand2
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.LeftHand2);

                if (DS2SItem.Items.ContainsKey(itemID))
                    return DS2SItem.Items[itemID];

                return "Unknown";
            }
        }
        public string LeftHand3
        {
            get
            {
                if (!Loaded) return "";
                var itemID = PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerEquipment.LeftHand3);

                if (DS2SItem.Items.ContainsKey(itemID))
                    return DS2SItem.Items[itemID];

                return "Unknown";
            }
        }
        private IntPtr AccelSpeedPtr;
        public float AccelSpeed
        {
            get => AccelSpeedPtr != IntPtr.Zero ? BitConverter.ToSingle(Kernel32.ReadBytes(Handle, AccelSpeedPtr, 0x4), 0x0) : 1f;
            set
            {
                if (AccelSpeedPtr != IntPtr.Zero)
                    Kernel32.WriteBytes(Handle, AccelSpeedPtr, BitConverter.GetBytes(value));
            }
        }
        private bool _accelerationStamina;
        public bool AccelerationStamina
        {
            get => _accelerationStamina;
            set
            {
                _accelerationStamina = value;
                if (_accelerationStamina)
                    AccelSpeedPtr = InjectSpeedFactor(SpeedFactorAccel);
            }
        } 
        private IntPtr InjectSpeedFactor(PHPointer speedFactorPointer)
        {
            var asm = (byte[])DS2SAssembly.SpeedFactor.Clone();
            var inject = new byte[0x8];
            var newCode = new byte[0xD];
            Array.Copy(asm, inject, inject.Length);
            Array.Copy(asm, 0x8, newCode, 0x0, newCode.Length);

            var valuePointer = Allocate(sizeof(float));
            Kernel32.WriteBytes(Handle, valuePointer, BitConverter.GetBytes(1f));
            

            var code = Kernel32.VirtualAllocEx(Handle, IntPtr.Zero, (IntPtr)newCode.Length, Kernel32.MEM_COMMIT | Kernel32.MEM_RESERVE, Kernel32.PAGE_EXECUTE_READWRITE);
            Kernel32.WriteBytes(Handle, code, newCode);
            var kek2 = RegisterAbsoluteAOB(GetAoBFromBytes(newCode));
            RescanAOB();
            var heh = kek2.Resolve();
            var kek = speedFactorPointer.Resolve().ToInt64() - BaseAddress.ToInt64();
            var keklol = kek + Handle.ToInt64();
            var pro = Process;
            var lol = GetJumpOffset(speedFactorPointer.Resolve() + inject.Length, code);
            return valuePointer;
        }

        private byte?[] GetAoBFromBytes(byte[] incomingBytes)
        {
            var bytes = new byte?[incomingBytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                    bytes[i] = incomingBytes[i];
            }

            return bytes;
           
        }

        private short GetJumpOffset(IntPtr srcPtr, IntPtr destPtr)
        {
            var addr1 = srcPtr.ToInt64();
            var addr2 = destPtr.ToInt64();
            var result = addr1 - addr2;
            return (short)result;
        }

        #endregion

        #region Indexer
        //public int this[string attributeName]
        //{
        //    get
        //    {
        //        var prop = GetType().GetProperty(attributeName);
        //        if (prop != null)
        //            return prop;

        //        throw new MissingMemberException("DSHook", attributeName);
        //    }
        //    set
        //    {
        //        //Get each property
        //        var props = typeof(DS2SHook).GetProperties();
        //        foreach (var prop in props)
        //        {
        //            //Check if it has a ControlAttribute with the same name
        //            //var Attr = prop.GetCustomAttribute<ControlAttribute>();
        //            //if (Attr != null && Attr.Name == attributeName)
        //            //{
        //            //    if (prop.PropertyType.Equals(typeof(byte)))
        //            //        prop.SetValue(this, (byte)value, null); //Set the properties value
        //            //    else
        //            //        prop.SetValue(this, value, null); //Set the properties value

        //            //    return;
        //            //}
        //        }
        //        throw new MissingMemberException("DSHook", attributeName);

        //    }
        //}
        #endregion
    }
}