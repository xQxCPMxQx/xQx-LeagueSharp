using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using Olaf.Common;

namespace Olaf.Modes
{
    internal class ModeSettings
    {
        public static Menu MenuLocal { get; private set; }
        public static Menu MenuSkins { get; private set; }
        public static Menu MenuSpellE { get; private set; }
        public static Menu MenuFlame { get; private set; }
        public static Menu MenuSpellR { get; private set; }
        public static void Init(Menu MenuParent)
        {
            MenuLocal = new Menu("Settings", "Settings");
            MenuParent.AddSubMenu(MenuLocal);

            MenuSpellE = new Menu("E Settings:", "MenuE").SetFontStyle(FontStyle.Regular, Champion.PlayerSpells.E.MenuColor());
            {
                string[] strE = new string[1000/250];
                for (var i = 250; i <= 1000; i += 250)
                {
                    strE[i/250 - 1] = i + " ms.";
                }
                MenuSpellE.AddItem(new MenuItem("Settings.SpellCast.VisibleDelay", "Cast delay for Instanly Visible Enemy:").SetValue(new StringList(strE, 0))).SetTooltip("Exp: Rengar, Shaco, Wukong, Kha'Zix, Vayne").SetFontStyle(FontStyle.Regular, Champion.PlayerSpells.Q.MenuColor());
                MenuSpellE.AddItem(new MenuItem("MenuE.E.Auto", "Auto-Use (If Enemy Hit)").SetValue(new StringList(new []{"Off", "On"}, 1))).SetFontStyle(FontStyle.Regular, Champion.PlayerSpells.Q.MenuColor());
            }
            MenuLocal.AddSubMenu(MenuSpellE);

            MenuFlame = new Menu("Flame", "Flame");
            MenuFlame.AddItem(new MenuItem("Flame.Laugh", "After Kill:").SetValue(new StringList(new[] {"Off", "Joke", "Taunt", "Laugh", "Random"}, 4)));
            MenuFlame.AddItem(new MenuItem("Flame.Ctrl6", "After Kill: Show Champion Point Icon (Ctrl + 6)").SetValue(new StringList(new[] { "Off", "On" })));
            
            Common.CommonManaManager.Init(MenuLocal);
        }
    }
}
