﻿using Microsoft.Win32;

namespace TweakUIX.Tweaks.Desktop
{
    internal class TaskbarMM : TweaksBase
    {
        private static readonly ErrorHelper logger = ErrorHelper.Instance;

        private const string keyName = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const int desiredValue = 0;

        public override string ID()
        {
            return "Hide Taskbar on multiple monitors";
        }

        public override string Info()
        {
            return "This will hide secondary taskbars.";
        }

        public override bool CheckTweak()
        {
            return !(
               RegistryHelper.IntEquals(keyName, "MMTaskbarEnabled", desiredValue)
             );
        }

        public override bool DoTweak()
        {
            try
            {
                Registry.SetValue(keyName, "MMTaskbarEnabled", desiredValue, RegistryValueKind.DWord);

                WindowsHelper.RunCmd("/c taskkill /f /im explorer.exe");
                WindowsHelper.RunCmd("/c start explorer.exe");

                logger.Log("- Taskbar on secondary monitor has been disabled.");
                logger.Log(keyName);
                return true;
            }
            catch
            { }

            return false;
        }

        public override bool UndoTweak()
        {
            try
            {
                Registry.SetValue(keyName, "MMTaskbarEnabled", "1", RegistryValueKind.DWord);

                WindowsHelper.RunCmd("/c taskkill /f /im explorer.exe");
                WindowsHelper.RunCmd("/c start explorer.exe");

                logger.Log("- Taskbar on secondary monitor has been enabled.");
                return true;
            }
            catch
            { }

            return false;
        }
    }
}