using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using Olaf.Common;
using SharpDX;

namespace Olaf.Champion
{
    internal static class PlayerSpells
    {
        public static List<Spell> SpellList = new List<Spell>();

        public static Spell Q, W, E, R;

        public static void Init()
        {
            Q = new Spell(SpellSlot.Q, 980);
            Q.SetSkillshot(0.25f, 75f, 1500f, false, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 325);
            R = new Spell(SpellSlot.R);

            SpellList.AddRange(new[] { Q, W, E, R });
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
        }
        private static int LastSeen(Obj_AI_Base t)
        {
            return Common.AutoBushHelper.EnemyInfo.Find(x => x.Player.NetworkId == t.NetworkId).LastSeenForE;
        }

        static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            var hero = args.Target as Obj_AI_Hero;
            if (hero != null)
            {
                var t = hero;
                if (!t.IsValidTarget(E.Range))
                {
                    return;
                }
                // args.Process = !t.HaveImmortalBuff();

                args.Process = Environment.TickCount - LastSeen(t) >= (Modes.ModeSettings.MenuLocal.Item("Settings.SpellCast.VisibleDelay").GetValue<StringList>().SelectedIndex + 1) * 250;
            }
        }

        public static void CastQ(Obj_AI_Base t, float range = 980)
        {
            if (!Q.IsReady() || !t.IsValidTarget(range))
            {
                return;
            }

            if (t.HaveOlafSlowBuff() && t.IsValidTarget(Orbwalking.GetRealAutoAttackRange(null) + 65))
            {
                return;
            }

            PredictionOutput qPredictionOutput = Q.GetPrediction(t);
            Vector3 castPosition = qPredictionOutput.CastPosition.Extend(ObjectManager.Player.Position, - (ObjectManager.Player.Distance(t.ServerPosition) >= 450 ? 80 : 140));

            if (qPredictionOutput.Hitchance >=
                (ObjectManager.Player.Distance(t.ServerPosition) >= 350 ? HitChance.VeryHigh : HitChance.High) &&
                ObjectManager.Player.Distance(castPosition) < range)
            {
                Q.Cast(castPosition);
            }
        }

        public static void CastE(Obj_AI_Base t)
        {
            if (!E.CanCast(t))
            {
                return;
            }

            E.CastOnUnit(t);
        }
    }
}
