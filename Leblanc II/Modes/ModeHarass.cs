using System;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Leblanc.Champion;
using Color = SharpDX.Color;
using Leblanc.Common;

namespace Leblanc.Modes
{

    internal class ModeHarass
    {
        public static Menu MenuLocal { get; private set; }
        public static Menu MenuToggle { get; private set; }
        private static Spell Q => Champion.PlayerSpells.Q;
        private static Spell W => Champion.PlayerSpells.W;
        private static Spell E => Champion.PlayerSpells.E;
        private static Obj_AI_Hero Target => TargetSelector.GetTarget(Q.Range * 2, TargetSelector.DamageType.Magical);
      
        public static void Init()
        {

            MenuLocal = new LeagueSharp.Common.Menu("Harass", "Harass");
            {
                MenuLocal.AddItem(new MenuItem("Harass.UseQ", "Q:").SetValue(true).SetFontStyle(FontStyle.Regular, Q.MenuColor()));
                MenuLocal.AddItem(new MenuItem("Harass.UseW", "W:").SetValue(new StringList(new []{"Off", "On", "On: After Q"}, 2)).SetFontStyle(FontStyle.Regular, W.MenuColor()));
                MenuLocal.AddItem(new MenuItem("Harass.UseW.Return", "W: Auto Return:").SetValue(new StringList(new[] { "Off", "On: Everytime", "On: Don't Go Back if Enemy Have Chain" }, 2)).SetFontStyle(FontStyle.Regular, W.MenuColor()));
                MenuLocal.AddItem(new MenuItem("Harass.UseE", "E:").SetValue(false).SetFontStyle(FontStyle.Regular, E.MenuColor()));

                MenuToggle = new Menu("Toggle Harass", "Toggle").SetFontStyle(FontStyle.Regular, Color.AliceBlue);
                {
                    MenuToggle.AddItem(new MenuItem("Toggle.UseQ", "Q:").SetValue(false).SetFontStyle(FontStyle.Regular, Q.MenuColor()));
                    MenuToggle.AddItem(new MenuItem("Toggle.UseW", "W:").SetValue(false).SetFontStyle(FontStyle.Regular, W.MenuColor()));
                    MenuToggle.AddItem(new MenuItem("Toggle.UseE", "E:").SetValue(false).SetFontStyle(FontStyle.Regular, E.MenuColor()));
                    MenuLocal.AddSubMenu(MenuToggle);
                }
            }
            Modes.ModeConfig.MenuConfig.AddSubMenu(MenuLocal);

            Game.OnUpdate += GameOnOnUpdate;
        }

        private static void GameOnOnUpdate(EventArgs args)
        {
            if (Modes.ModeConfig.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                ExecuteHarass();
            }

            if (Modes.ModeConfig.MenuKeys.Item("Key.Harass1").GetValue<KeyBind>().Active)
            {
                ExecuteToggle();
            }
        }

        private static void CastW()
        {
            if (!W.CanCast(Target))
            {
                return;
            }
            PlayerSpells.CastW(Target, true);
            return;

            var nAutoReturnBack = MenuLocal.Item("Harass.UseW.Return").GetValue<StringList>().SelectedIndex;
            {
                switch (nAutoReturnBack)
                {
                    case 0:
                    {
                        PlayerSpells.CastW(Target, true);
                        break;
                    }
                    case 1:
                    {
                        PlayerSpells.CastW(Target, true);
                        break;
                    }
                    case 2:
                    {
                        PlayerSpells.CastW(Target, !Target.HasSoulShackle());
                        
                        break;
                    }
                }

                //   PlayerSpells.CastW(Target, true);
            }
        }
        private static void ExecuteHarass()
        {
            if (W.StillJumped())
            {
                W.Cast();
            }

            if (MenuLocal.Item("Harass.UseQ").GetValue<bool>() && Q.CanCast(Target))
            {
                PlayerSpells.CastQ(Target);
            }

            var harassUseW = MenuLocal.Item("Harass.UseW").GetValue<StringList>().SelectedIndex;
            
            if (harassUseW != 0 && W.CanCast(Target))
            {
                if (harassUseW == 1)
                {
                    CastW();
                }
                else if (harassUseW == 2 && Target.HasMarkedWithQ())
                {
                    CastW();
                }
            }

            if (MenuLocal.Item("Harass.UseE").GetValue<bool>() && E.CanCast(Target))
            {
                PlayerSpells.CastE(Target);
            }
        }

        private static void ExecuteToggle()
        {
            if (Modes.ModeConfig.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo || Modes.ModeConfig.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                return;
            }

            if (W.StillJumped())
            {
                W.Cast();
            }

            if (MenuLocal.Item("Toggle.UseQ").GetValue<bool>() && Q.CanCast(Target))
            {
                PlayerSpells.CastQ(Target);
            }

            if (MenuLocal.Item("Toggle.UseW").GetValue<bool>() && W.CanCast(Target))
            {
                PlayerSpells.CastW(Target, true);
                //var nReturn = MenuLocal.Item("Harass.UseW.Return").GetValue<StringList>().SelectedIndex;
                //{
                //    if (nReturn == 0)
                //    {
                //        PlayerSpells.CastW(Target, false);
                //    }

                //    if (nReturn == 1)
                //    {
                //        PlayerSpells.CastW(Target, true);
                //    }

                //    if (nReturn == 2)
                //    {
                //        PlayerSpells.CastW(Target, false);

                //        if (Target.HasSoulShackle())
                //        {
                //            if (Target.Distance(PlayerObjects.LeblancSoulShackle.Object.Position) < E.Range && W.StillJumped())
                //            {
                //                W.Cast();
                //            }
                //        }
                //        else
                //        {
                //            W.Cast();
                //        }

                //    }
                //}
            }

            if (MenuLocal.Item("Toggle.UseE").GetValue<bool>() && E.CanCast(Target))
            {
                PlayerSpells.CastE(Target);
            }
        }
    }
}
