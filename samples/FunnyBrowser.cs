//
// WebKit# - WebKit bindings for Mono
//
// Author: 
//   Everaldo Canuto <ecanuto@novell.com>
//
// Copyright (c) 2008 Novell, Inc. All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


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
		const string APP_NAME = "FunnyBrowser";

		private string url = "http://www.google.com/";
		private Toolbar toolbar = null;
		private WebKit.WebView webview = null;
		private Gtk.Statusbar statusbar = null;

		public MainWindow (string url): base (Gtk.WindowType.Toplevel)
		{
			if (url != "")
				this.url = url;

			InitializeWidgets ();
			webview.Open (this.url);
		}
		
		private void InitializeWidgets ()
		{
			this.Title = "";
			this.SetDefaultSize (700, 500);
			this.DeleteEvent += new DeleteEventHandler (OnDeleteEvent);

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

		/*public string Caption
		{
			get { return base.Title; }
			set {
				string fmt_str = (value != "") ? "{1} - {0}" : "{0}";
				
				base.Title = String.Format (fmt_str, APP_NAME, value);
			}
		}*/
	
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
	}
}

