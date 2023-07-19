#if USE_ARCWEAVE

using System;
using System.Collections.Generic;
using UnityEngine;
using Language.Lua;

namespace PixelCrushers.DialogueSystem.ArcweaveSupport
{

    /// <summary>
    /// Implements Arcscript built-in functions for the Dialogue System's Lua environment.
    /// Notes:
    /// - Uses LuaInterpreter.
    /// - Does not implement reset() or resetAll().
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class ArcweaveLua : Saver
    {
        [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
        public bool unregisterOnDisable = false;

        public override void OnEnable()
        {
            base.OnEnable();
            Lua.RegisterFunction(nameof(abs), null, SymbolExtensions.GetMethodInfo(() => abs(0)));
            Lua.RegisterFunction(nameof(sqr), null, SymbolExtensions.GetMethodInfo(() => sqr(0)));
            Lua.RegisterFunction(nameof(sqrt), null, SymbolExtensions.GetMethodInfo(() => sqrt(0)));
            Lua.RegisterFunction(nameof(random), null, SymbolExtensions.GetMethodInfo(() => random()));
            Lua.RegisterFunction(nameof(visits), this, SymbolExtensions.GetMethodInfo(() => visits(string.Empty)));
            Lua.environment.Register(nameof(roll), roll);
            Lua.environment.Register(nameof(show), show);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (unregisterOnDisable)
            {
                Lua.UnregisterFunction(nameof(abs));
                Lua.UnregisterFunction(nameof(sqr));
                Lua.UnregisterFunction(nameof(sqrt));
                Lua.UnregisterFunction(nameof(random));
                Lua.UnregisterFunction(nameof(roll));
                Lua.UnregisterFunction(nameof(show));
            }
        }

        public static double abs(double n)
        {
            return Mathf.Abs((float)n);
        }

        public static double sqr(double n)
        {
            return (n * n);
        }

        public static double sqrt(double n)
        {
            return Mathf.Sqrt((float)n);
        }

        public static double random()
        {
            return UnityEngine.Random.value; // Note: Returns 1 inclusive, but Arcscript random() is 1 exclusive.
        }

        public static LuaValue roll(LuaValue[] values)
        {
            int m = (int)(values[0] as LuaNumber).Number;
            int n = (values.Length > 1 && values[1] is LuaNumber) ? (int)(values[1] as LuaNumber).Number : 1;
            double result = 0;
            for (int i = 0; i < (int)n; i++)
            {
                result += UnityEngine.Random.Range(1, (int)m + 1);
            }
            return new LuaNumber(result);
        }

        public static LuaValue show(LuaValue[] values)
        {
            var s = string.Empty;
            foreach (var value in values)
            {
                s += value.ToString();
            }
            return new LuaString(s);
        }

        #region visits()

        [Serializable]
        public class SaveData
        {
            public List<string> guids = new List<string>();
            public List<int> visits = new List<int>();
        }

        protected Dictionary<string, int> visitsDict = new Dictionary<string, int>();
        protected string lastTextGuid = string.Empty;

        protected virtual void OnConversationLine(Subtitle subtitle)
        {
            if (string.IsNullOrEmpty(subtitle.formattedText.text)) return;
            lastTextGuid = Field.LookupValue(subtitle.dialogueEntry.fields, "Guid");
            if (string.IsNullOrEmpty(lastTextGuid)) return;
            if (visitsDict.ContainsKey(lastTextGuid))
            {
                visitsDict[lastTextGuid]++;
            }
            else
            {
                visitsDict.Add(lastTextGuid, 1);
            }
        }

        public double visits(string id)
        {
            int count;
            var guid = !string.IsNullOrEmpty(id) ? id : lastTextGuid;
            return visitsDict.TryGetValue(guid, out count) ? count : 0;
        }

        public override string RecordData()
        {
            var data = new SaveData();
            foreach (var kvp in visitsDict) 
            {
                data.guids.Add(kvp.Key);
                data.visits.Add(kvp.Value);
            }
            return SaveSystem.Serialize(data);
        }

        public override void ApplyData(string s)
        {
            if (string.IsNullOrEmpty(s)) return;
            var data = SaveSystem.Deserialize<SaveData>(s);
            if (data == null) return;
            visitsDict.Clear();
            for (int i = 0; i < Mathf.Min(data.guids.Count, data.visits.Count); i++)
            {
                visitsDict.Add(data.guids[i], data.visits[i]);
            }
        }

        #endregion

    }
}

#endif
