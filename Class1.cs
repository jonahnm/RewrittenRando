using BepInEx;
using System;
using HarmonyLib;
using BepInEx.Logging;
using System.Collections.Generic;
using Nautilus.Json.Attributes;
using Nautilus.Json;
using Nautilus.Handlers;
using System.Linq;
using System.IO;

namespace RewrittenRando
{
    [BepInPlugin("com.sn.hamlet.rewrittenrando","Aci's Randomizer Rewritten","1.0.1")]
    public class RewrittenRando : BaseUnityPlugin
    {
        public static ManualLogSource logger;
        public static Dictionary<TechType, TechType> currentrandompool;
        public static List<TechType> vals = new List<TechType>();
        public static List<TechType> keys = new List<TechType>();
        public static List<TechType> unusedTT = new List<TechType>()
        {
            TechType.PrecursorIonCrystalMatrix,
            TechType.PrecursorKey_Red,
            TechType.PrecursorKey_White,
            TechType.CurrentGenerator,
            TechType.KooshZoneEgg,
            TechType.GrandReefsEgg,
            TechType.GrassyPlateausEgg,
            TechType.KelpForestEgg,
            TechType.LavaZoneEgg,
            TechType.MushroomForestEgg,
            TechType.SafeShallowsEgg,
            TechType.TwistyBridgesEgg,
            TechType.ReefbackEgg,
            TechType.JumperEgg,
            TechType.LithiumIonBattery,
            TechType.Nanowires,
            TechType.Thermometer,
            TechType.AminoAcids,
            TechType.BatteryAcidOld,
            TechType.CarbonOld,
            TechType.CompostCreepvine,
            TechType.EmeryOld,
            TechType.EthanolOld,
            TechType.EthyleneOld,
            TechType.FlintOld,
            TechType.HydrogenOld,
            TechType.Lodestone,
            TechType.Magnesium,
            TechType.MembraneOld,
            TechType.MercuryOre,
            TechType.SandLoot,
            TechType.Uranium,
            TechType.HullReinforcementModule,
            TechType.HullReinforcementModule2,
            TechType.HullReinforcementModule3,
            TechType.BasaltChunk,
            TechType.Accumulator,
            TechType.Bioreactor,
            TechType.Centrifuge,
            TechType.FragmentAnalyzer,
            TechType.NuclearReactor,
            TechType.ObservatoryOld,
            TechType.SpecimenAnalyzer,
            TechType.Drill,
            TechType.DiamondBlade,
            TechType.PowerGlide,
            TechType.Terraformer,
            TechType.Transfuser,
            TechType.SeamothReinforcementModule,
            TechType.Signal,
            TechType.Fragment,
            TechType.FragmentAnalyzer,
            TechType.FragmentAnalyzerBlueprintOld,
            TechType.TerraformerFragment,
            TechType.TransfuserFragment,
            TechType.OrangePetalsPlantSeed,
            TechType.LabContainer,
            TechType.LabContainer2,
            TechType.LabContainer3,
            TechType.LabEquipment1,
            TechType.LabEquipment2,
            TechType.LabEquipment3,
            TechType.ArcadeGorgetoy,
            TechType.PosterAurora,
            TechType.StarshipSouvenir,
            TechType.Poster,
            TechType.PosterKitty,
            TechType.PosterExoSuit1,
            TechType.PosterExoSuit2,
            TechType.Cap1,
            TechType.Cap2
        };
        public static List<TechType> unwantedTT = new List<TechType>() {
            TechType.Player,
            TechType.None,
            TechType.PDA,
            TechType.BuildBot,
            TechType.GenericEgg,
            TechType.Workbench,
            TechType.DiveHatch,
            TechType.Fabricator,
            TechType.Aquarium,
            TechType.Locker,
            TechType.Spotlight,
            TechType.DevTestItem,
            TechType.SolarPanel,
            TechType.Sign,
            TechType.PowerTransmitter,
            TechType.Accumulator,
            TechType.Bioreactor,
            TechType.ThermalPlant,
            TechType.NuclearReactor,
            TechType.SmallLocker,
            TechType.Bench,
            TechType.PictureFrame,
            TechType.PlanterBox,
            TechType.PlanterPot,
            TechType.PlanterShelf,
            TechType.FarmingTray,
            TechType.FiltrationMachine,
            TechType.Techlight,
            TechType.Radio,
            TechType.PlanterPot3,
            TechType.PlanterPot2,
            TechType.MedicalCabinet,
            TechType.SingleWallShelf,
            TechType.WallShelves,
            TechType.Bed1,
            TechType.Bed2,
            TechType.NarrowBed,
            TechType.BatteryCharger,
            TechType.PowerCellCharger,
            TechType.Incubator,
            TechType.EnzymeCureBall,
            TechType.Centrifuge,
            TechType.StarshipCircuitBox,
            TechType.StarshipDesk,
            TechType.StarshipChair,
            TechType.StarshipMonitor,
            TechType.StarshipChair2,
            TechType.StarshipChair3,
            TechType.LuggageBag,
            TechType.CoffeeVendingMachine,
            TechType.BarTable,
            TechType.Trashcans,
            TechType.LabTrashcan,
            TechType.VendingMachine,
            TechType.LabCounter,
            TechType.CrashedShip,
            TechType.Audiolog,
            TechType.Signal,
            TechType.LootSensorMetal,
            TechType.LootSensorLithium,
            TechType.Grabcrab,
            TechType.CrashHome,
            TechType.Creepvine,
            TechType.HoopfishSchool,
            TechType.RockPuncher,
            TechType.Skyray,
            TechType.SkyrayNonRoosting,
            TechType.SeaEmperor,
            TechType.SeaEmperorBaby,
            TechType.SeaEmperorJuvenile,
            TechType.SeaEmperorLeviathan,
            TechType.SeaDragon,
            TechType.SeaDragonSkeleton,
            TechType.PrecursorSeaDragonSkeleton,
            TechType.Reefback,
            TechType.ReefbackAdvancedStructure,
            TechType.ReefbackBaby,
            TechType.ReefbackShell,
            TechType.ReefbackTissue,
            TechType.GhostLeviathan,
            TechType.GhostLeviathanJuvenile,
            TechType.SmallKoosh,
            TechType.MediumKoosh,
            TechType.LargeKoosh,
            TechType.HugeKoosh,
            TechType.MembrainTree,
            TechType.BigCoralTubes,
            TechType.HeatArea,
            TechType.BloodRoot,
            TechType.BloodVine,
            TechType.BloodGrass,
            TechType.BlueBarnacle,
            TechType.BallClusters,
            TechType.BarnacleSuckers,
            TechType.BlueBarnacleCluster,
            TechType.BlueCoralTubes,
            TechType.RedGrass,
            TechType.GreenGrass,
            TechType.Mohawk,
            TechType.GreenReeds,
            TechType.JellyPlant,
            TechType.PurpleFan,
            TechType.PurpleTentacle,
            TechType.RedSeaweed,
            TechType.SmallFan,
            TechType.SmallFanCluster,
            TechType.TreeMushroom,
            TechType.BlueCluster,
            TechType.BrownTubes,
            TechType.PinkFlower,
            TechType.PurpleRattle,
            TechType.PinkMushroom,
            TechType.BulboTree,
            TechType.PurpleVasePlant,
            TechType.OrangeMushroom,
            TechType.FernPalm,
            TechType.HangingFruitTree,
            TechType.PurpleVegetablePlant,
            TechType.MelonPlant,
            TechType.BluePalm,
            TechType.GabeSFeather,
            TechType.SeaCrown,
            TechType.OrangePetalsPlant,
            TechType.EyesPlant,
            TechType.RedGreenTentacle,
            TechType.PurpleStalk,
            TechType.RedBasketPlant,
            TechType.RedBush,
            TechType.RedConePlant,
            TechType.ShellGrass,
            TechType.SpottedLeavesPlant,
            TechType.RedRollPlant,
            TechType.PurpleBranches,
            TechType.SnakeMushroom,
            TechType.FloatingStone,
            TechType.BlueAmoeba,
            TechType.RedTipRockThings,
            TechType.BlueTipLostRiverPlant,
            TechType.BlueLostRiverLilly,
            TechType.LargeFloater,
            TechType.Boulder,
            TechType.PurpleBrainCoral,
            TechType.HangingStinger,
            TechType.SpikePlant,
            TechType.BrainCoral,
            TechType.CoveTree,
            TechType.MonsterSkeleton,
            TechType.SeaDragonSkeleton,
            TechType.ReaperSkeleton,
            TechType.CaveSkeleton,
            TechType.HugeSkeleton,
            TechType.PrecursorKeyTerminal,
            TechType.PrecursorTeleporter,
            TechType.PrecursorEnergyCore,
            TechType.PrecursorThermalPlant,
            TechType.PrecursorWarper,
            TechType.PrecursorFishSkeleton,
            TechType.PrecursorScanner,
            TechType.PrecursorLabCacheContainer1,
            TechType.PrecursorLabCacheContainer2,
            TechType.PrecursorLabTable,
            TechType.PrecursorSeaDragonSkeleton,
            TechType.PrecursorSensor,
            TechType.PrecursorPipeRoomIncomingPipe,
            TechType.PrecursorPipeRoomOutgoingPipe,
            TechType.PrecursorSurfacePipe,
            TechType.PrecursorLostRiverBrokenAnchor,
            TechType.PrecursorLostRiverLabRays,
            TechType.PrecursorLostRiverLabBones,
            TechType.PrecursorLostRiverLabEgg,
            TechType.PrecursorLostRiverProductionLine,
            TechType.PrecursorLostRiverWarperParts,
            TechType.MapRoomCamera,
            TechType.RocketStage1,
            TechType.RocketStage2,
            TechType.RocketStage3,
            TechType.TimeCapsule,
            TechType.RadiationLeakPoint,
            TechType.Wreck,
            TechType.Databox,
            TechType.LEDLight,
            TechType.Bloom,
            TechType.Shocker,
            TechType.SeaTreader,
            TechType.Enamel,
            TechType.EnyzmeCloud,
            TechType.Crash,
            TechType.Mesmer,
            TechType.LavaLarva,
            TechType.Gasopod,
            TechType.CaveCrawler,
            TechType.GenericJeweledDisk,
            TechType.Shuttlebug,
            TechType.Marki1,
            TechType.Marki2,
            TechType.StarshipCargoCrate,
            TechType.OpalGem,
            TechType.Jumper,
            TechType.Stalker,
            TechType.StasisSphere,
            TechType.GhostRayBlue,
            TechType.GhostRayRed,
            TechType.Slime,
            TechType.Jellyray,
            TechType.LavaLizard,
            TechType.Sandshark,
            TechType.SandLoot,
            TechType.Blighter,
            TechType.Biter,
            TechType.ReefbackDNA,
            TechType.BoneShark,
            TechType.ToyCar,
            TechType.GreenJeweledDisk,
            TechType.Rockgrub,
            TechType.SpineEel,
            TechType.EatMyDiction,
            TechType.ReaperLeviathan,
            TechType.Cutefish,
            TechType.Crabsnake,
            TechType.Warper,
            TechType.WarperSpawner,
            TechType.JackSepticEye,
            TechType.Graphene,
            TechType.PurpleJeweledDisk,
            TechType.Fiber,
            TechType.ExosuitClawArmModule,
            TechType.ExosuitDrillArmModule,
            TechType.ExosuitGrapplingArmModule,
            TechType.ExosuitTorpedoArmModule,
            TechType.MesmerEgg,
            TechType.CyclopsFabricator,
            TechType.ShockerEgg,
            TechType.CrabSquid,
            TechType.Bleeder,
            TechType.CrabsnakeEgg,
            TechType.JellyrayEgg,
            TechType.EscapePod,
            TechType.Unobtanium,
            TechType.CoralShellPlate,
            TechType.ProcessUranium,
            TechType.RabbitRay,
            TechType.RedJeweledDisk,
            TechType.PrecursorDroid,
            TechType.BlueJeweledDisk
        };
        public static List<TechType> vehiclekeys = new List<TechType>() { TechType.Cyclops, TechType.Exosuit, TechType.RocketBase , TechType.Seamoth};
        public static List<TechType> vehiclevals = new List<TechType>() { TechType.Cyclops, TechType.Exosuit, TechType.RocketBase, TechType.Seamoth };
        public static Dictionary<TechType, TechType> vehiclepool;
        public static List<Int3> octreekeys = new List<Int3>();
        public static List<Int3> octreevals = new List<Int3>();
        public static Dictionary<Int3,Int3> octreepool = new Dictionary<Int3,Int3>();
        public static Harmony harmony = new Harmony("com.sn.hamlet.rewrittenrando");
        void Awake()
        {
            harmony.PatchAll();
            logger = Logger;
            var data = SaveDataHandler.RegisterSaveDataCache<DropRandomizerData>();
            foreach (TechType TT in Enum.GetValues(typeof(TechType)))
            {
                if (unusedTT.Contains(TT) || unwantedTT.Contains(TT) || vehiclekeys.Contains(TT))
                    continue;
                else if (TT.ToString().ToLower().Contains("old") || TT.ToString().ToLower().Contains("placeholder") || TT.ToString().ToLower().Contains("chunk") || TT.ToString().ToLower().Contains("drillable") || TT.ToString().ToLower().Contains("blueprint") || TT.ToString().ToLower().Contains("fragment") || TT.ToString().ToLower().Contains("undiscover") || TT.ToString().ToLower().Contains("analysis") || TT.ToString().ToLower().Contains("prison") || TT.ToString().ToLower().Contains("base") || TT.ToString().ToLower().Contains("hullplate"))
                    continue;
                vals.Add(TT);
                keys.Add(TT);
            }
            vals.Add(TechType.KooshChunk);
            vals.Add(TechType.CoralChunk);
            keys.Add(TechType.CoralChunk);
            keys.Add(TechType.KooshChunk);
            vals.Add(TechType.Gold);
            keys.Add(TechType.Gold);
            vals.Sort((x, y) => string.Compare(x.ToString(), y.ToString()));
            data.OnFinishedLoading += (object sender, JsonFileEventArgs e) =>
            {
                var randodata = e.Instance as DropRandomizerData;
                if (data.hassavedinsave)
                {
                    currentrandompool = randodata.randompool;
                    vehiclepool = randodata.randomvehicles;
                    octreepool = randodata.randomoctree;
                }
                else
                {
                    var newpoolkeys = Shuffler<TechType>.Shuffle(keys);
                    keys = newpoolkeys.ToList();
                    currentrandompool = keys.Zip(vals, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
                    var newvehiclekeys = Shuffler<TechType>.Shuffle(vehiclekeys);
                    vehiclepool = newvehiclekeys.ToList().Zip(vehiclevals, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SNcheatsheet.txt");
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        using (TextWriter tw = new StreamWriter(fs))
                        {

                            foreach (var kvp in currentrandompool)
                            {
                                tw.WriteLine($"{kvp.Key} : {kvp.Value}");
                            }
                            tw.WriteLine("Vehicle: \n");
                            foreach(var kvp in vehiclepool)
                            {
                                tw.WriteLine($"{kvp.Key} : {kvp.Value}");
                            }
                        }
                    }
                }
            };
            data.OnStartedSaving += (object sender, JsonFileEventArgs e) => {
                var randodata = e.Instance as DropRandomizerData;
                randodata.randompool = currentrandompool;
                randodata.randomoctree = octreepool;
                randodata.hassavedinsave = true;
            };
        }

        [FileName("droprandomizer")]
        public class DropRandomizerData : SaveDataCache
        {
            public bool hassavedinsave = false;
            public Dictionary<TechType, TechType> randompool;
            public Dictionary<TechType, TechType> randomvehicles;
            public Dictionary<TechType, TechType> randomfrags;
            public Dictionary<Int3, Int3> randomoctree;
        }
    }
    //Found on stackoverflow- https://stackoverflow.com/questions/56378647/fisher-yates-shuffle-in-c-sharp
    public static class Shuffler<T>
    {
        private static Random r = new Random();

        public static T[] Shuffle(T[] items)
        {
            for (int i = 0; i < items.Length - 1; i++)
            {
                int pos = r.Next(i, items.Length);
                T temp = items[i];
                items[i] = items[pos];
                items[pos] = temp;
            }
            return items;
        }

        public static IList<T> Shuffle(IList<T> items)
        {
            for (int i = 0; i < items.Count - 1; i++)
            {
                int pos = r.Next(i, items.Count);
                T temp = items[i];
                items[i] = items[pos];
                items[pos] = temp;
            }
            return items;
        }
    }
    public static class StuffStuff
    {
        internal static bool filehasterrain(Int3 int3)
        {
            var name = $"compiled-batch-{int3.x}-{int3.y}-{int3.z}.optoctrees";
            var path = Path.Combine(SNUtils.InsideUnmanaged("Build18"),"CompiledOctreesCache",name);
            
            try
            {
                using (var file = File.OpenRead(path))
                {
                    using (var br = new BinaryReader(file))
                    {
                        var somebytes = br.ReadUInt32();
                        if(somebytes != 4)
                        {
                            return false;
                        }
                        while(br.BaseStream.Position < br.BaseStream.Length)
                        {
                            var len = br.ReadUInt16() * 4;
                            if (len > 4)
                            {
                                return true;
                            }
                            br.ReadBytes(len);
                        }
                    }
                }
            } catch(FileNotFoundException)
            {
                return false;
            }
                catch
            {
                RewrittenRando.logger.LogError(path);
                throw;
            }
            return false;
        }
    }
}
