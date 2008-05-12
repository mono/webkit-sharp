using System;
using Gtk;
using GtkSharp;
using WebKit;


namespace FunnyBrowser
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
			string url = (args.Length > 0) ? args[0] : "";

			Application.Init ();
			MainWindow window = new MainWindow (url);
			window.Show ();
			Application.Run ();
		}
	}

	public class MainWindow: Gtk.Window
	{
		private string url = "http://www.google.com/";
		private Toolbar toolbar = null;
		private WebKit.WebView webview = null;
		private Gtk.Statusbar statusbar = null;

		public MainWindow (string url): base (Gtk.WindowType.Toplevel)
		{
			if (url != "")
				this.url = url;

			InitializeControls ();
			webview.Open (this.url);
		}
		
		private void InitializeControls ()
		{
			SetDefaultSize (700, 500);
			DeleteEvent += new DeleteEventHandler (OnDeleteEvent);

			Gtk.VBox vbox = new VBox (false, 1);

			toolbar = new Toolbar ();
			toolbar.ToolbarStyle = ToolbarStyle.Both;
			toolbar.Orientation = Orientation.Horizontal;
			toolbar.ShowArrow = true;
			vbox.PackStart (toolbar, false, false, 0);

			webview = new WebView ();

			Gtk.ScrolledWindow scroll = new Gtk.ScrolledWindow ();
			scroll.Add (webview);
			scroll.SetPolicy (PolicyType.Automatic, PolicyType.Automatic);
			vbox.PackStart (scroll);
			
			statusbar = new Gtk.Statusbar ();
			vbox.PackEnd (statusbar, false, true, 0);

			this.Add (vbox);
			this.ShowAll ();
		}
	
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
	}
}

