using System;
using Gtk;

namespace GUI
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow();
            Button btn = new Button("Yo");
            win.Add(btn);
            win.ShowAll();
            Application.Run();
        }
    }
}
