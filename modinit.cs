using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Audio;
using STS2RitsuLib.Interop;
using Teamy.character.cards.ancient;
using Teamy.character.cards.basic;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace Teamy
{
    [ModInitializer(nameof(Init))]
    public class Modinit
    {
        // 你的modid
        public const string ModId = "Teamy";
        public static readonly Logger Logger = RitsuLibFramework.CreateLogger(ModId);
        public static readonly ConcurrentDictionary<string, PackedScene> ModSceneCache = new();

        public static void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);
            // 自动注册内容
            ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);
            RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<Boss, BossPlaus>();
        }
        static void LoadScenes() {
            //你的场景字符串列表
            var paths = new List<string> {
                "res://scenes/vfx/fps.tscn",
                "res://scenes/vfx/Fist.tscn",
                "res://scenes/vfx/Elect.tscn",
                "res://scenes/vfx/kill.tscn",
                "res://scenes/vfx/electic.tscn",
                "res://scenes/vfx/intangible.tscn",
                "res://scenes/vfx/wogua.tscn",
            };
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/BossDead.wav");
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/Fire.wav");
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/Grape.wav");
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/Four.wav");
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/Hourse.wav");
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/Cliff.wav");
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/ZhengQi.wav");
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/Mind.wav");
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/Head.wav");
            FmodStudioStreamingFiles.TryPreloadAsSound("res://audio/Build.wav");
            foreach (var path in paths) {
                if (ModSceneCache.ContainsKey(path)) continue;
                var scene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Reuse);
                if (scene != null) {
                    ModSceneCache[path] = scene;
                }
            }
        }
    }
}
