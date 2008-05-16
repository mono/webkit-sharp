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
		
		private Gtk.VBox vbox = null;
		private Gtk.Toolbar toolbar = null;
		private Gtk.Toolbar findbar = null;
		private Gtk.Entry uri_entry = null;
		private Gtk.Entry find_entry = null;
		private WebKit.WebView webview = null;
		private Gtk.Statusbar statusbar = null;
		
		private Gtk.Action action_back;
		private Gtk.Action action_forward;
		private Gtk.Action action_reload;
		private Gtk.Action action_stop;
		private Gtk.Action action_jump;

		public MainWindow (string url): base (Gtk.WindowType.Toplevel)
		{
			if (url != "")
				this.url = url;

			CreateWidgets ();
			webview.Open (this.url);
		}
		
		private void CreateWidgets ()
		{
			this.Title = APP_NAME;
			this.SetDefaultSize (700, 500);
			this.DeleteEvent += new DeleteEventHandler (OnDeleteEvent);

			CreateActions ();
			CreateToolbar ();
			CreateWebView ();
			CreateFindbar ();
			CreateStatusBar ();
					
			Gtk.ScrolledWindow scroll = new Gtk.ScrolledWindow ();
			scroll.Add (webview);

			vbox = new Gtk.VBox (false, 1);
			vbox.PackStart (toolbar, false, false, 0);
			vbox.PackStart (scroll);
			//vbox.PackStart (findbar, false, false, 0);
			vbox.PackEnd (statusbar, false, true, 0);

			this.Add (vbox);
			this.ShowAll ();
		}
		
		private void CreateActions ()
		{
			action_back    = new Gtk.Action("go-back",    "Go Back",    null, "gtk-go-back");
			action_forward = new Gtk.Action("go-forward", "Go Forward", null, "gtk-go-forward");
			action_reload  = new Gtk.Action("reload",     "Reload",     null, "gtk-refresh");
			action_stop    = new Gtk.Action("stop",       "Stop",       null, "gtk-stop");
			action_jump    = new Gtk.Action("jump",       "Jump",       null, "gtk-jump-to");

			action_back.Activated    += new EventHandler(on_back_activate);
			action_forward.Activated += new EventHandler(on_forward_activate);
			action_reload.Activated  += new EventHandler(on_reload_activate);
			action_stop.Activated    += new EventHandler(on_stop_activate);
			action_jump.Activated    += new EventHandler(on_uri_activate);
		}
		
		private void CreateToolbar ()
		{
			// UrlEntry
			uri_entry = new Gtk.Entry ();
			uri_entry.Activated += new EventHandler(on_uri_activate);

			Gtk.ToolItem uri_item = new Gtk.ToolItem ();
			uri_item.Expand = true;
			uri_item.Add (uri_entry);

			// Toolbar
			toolbar = new Toolbar ();
			toolbar.ToolbarStyle = ToolbarStyle.Icons;
			toolbar.Orientation = Orientation.Horizontal;
			toolbar.ShowArrow = true;

			// Toolbar Itens
			toolbar.Add (action_back.CreateToolItem());
			toolbar.Add (action_forward.CreateToolItem());
			toolbar.Add (action_reload.CreateToolItem());
			toolbar.Add (action_stop.CreateToolItem());
			toolbar.Add (uri_item);
			toolbar.Add (action_jump.CreateToolItem());
		}

		private void CreateWebView ()
		{
			webview = new WebView ();
			webview.Editable = false;
			webview.TitleChanged += new TitleChangedHandler (OnTitleChanged);
			webview.HoveringOverLink += new HoveringOverLinkHandler (OnHoveringOverLink);
			webview.LoadCommitted += new LoadCommittedHandler (OnLoadCommitted);
			webview.LoadFinished += new LoadFinishedHandler (OnLoadFinished);
		}

		private void CreateStatusBar ()
		{
			statusbar = new Gtk.Statusbar ();
		}

		private void CreateFindbar ()
		{
			// FindEntry
			find_entry = new Gtk.Entry ();
			//find_entry.Activated += new EventHandler(on_uri_activate);

			Gtk.ToolItem find_item = new Gtk.ToolItem ();
			//find_item.Expand = true;
			find_item.Add (find_entry);

			// Toolbar
			findbar = new Toolbar ();
			findbar.ToolbarStyle = ToolbarStyle.Icons;
			findbar.Orientation = Orientation.Horizontal;
			findbar.ShowArrow = true;

			// Toolbar Itens
			findbar.Add (action_stop.CreateToolItem());
			findbar.Add (find_item);
			findbar.Add (action_back.CreateToolItem());
			findbar.Add (action_forward.CreateToolItem());
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs args)
		{
			Application.Quit ();
			args.RetVal = true;
		}
		
		private void OnTitleChanged (object o, TitleChangedArgs args)
		{
			if (args.Title == String.Empty)
				this.Title = APP_NAME;
			else
				this.Title = String.Format ("{0} - {1}", args.Title, APP_NAME);
		}

		private void OnHoveringOverLink (object o, HoveringOverLinkArgs args)
		{
			statusbar.Pop (1);
			if (args.Link != null) {
				statusbar.Push (1, args.Link);
			}
		}

		private void OnLoadCommitted (object o, LoadCommittedArgs args)
		{
			action_back.Sensitive = webview.CanGoBack ();
			action_forward.Sensitive = webview.CanGoForward ();

			uri_entry.Text = args.Frame.Uri;
		}

		private void OnLoadFinished (object o, LoadFinishedArgs args)
		{
			//
		}

		private void on_back_activate (object o, EventArgs args)
		{
			webview.GoBack ();
		}

		private void on_forward_activate (object o, EventArgs args)
		{
			webview.GoForward ();
		}

		private void on_reload_activate (object o, EventArgs args)
		{
			webview.Reload ();
		}

		private void on_stop_activate (object o, EventArgs args)
		{
			webview.StopLoading ();
		}

		private void on_uri_activate (object o, EventArgs args)
		{
			webview.Open (uri_entry.Text);
		}
	}
}

