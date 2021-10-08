namespace NetWork
{
    class IgnoreFile
    {
        public static void M()
        {
            var ahkk = AutoHotkey.Interop.AutoHotkeyEngine.Instance;
            ahkk.GetVar("shut");
        }
        static void Main(string[] args)
        {
            M();
        }
    }
}
