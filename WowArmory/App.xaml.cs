﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using WowArmory.Controllers;
using WowArmory.Core.BattleNet;
using WowArmory.Core.BattleNet.Models;
using WowArmory.Core.Extensions;
using WowArmory.Core.Managers;
using WowArmory.Enumerations;
using WowArmory.ViewModels;

namespace WowArmory
{
	public partial class App : Application
	{
		//---------------------------------------------------------------------------
		#region --- Properties ---
		//---------------------------------------------------------------------------
		/// <summary>
		/// Provides easy access to the root frame of the Phone Application.
		/// </summary>
		/// <returns>The root frame of the Phone Application.</returns>
		public PhoneApplicationFrame RootFrame { get; private set; }
		//---------------------------------------------------------------------------
		#endregion
		//---------------------------------------------------------------------------


		//---------------------------------------------------------------------------
		#region --- Methods ---
		//---------------------------------------------------------------------------
		/// <summary>
		/// Constructor for the Application object.
		/// </summary>
		public App()
		{
			// Global handler for uncaught exceptions. 
			UnhandledException += Application_UnhandledException;

			// Show graphics profiling information while debugging.
			if ( System.Diagnostics.Debugger.IsAttached )
			{
				// Display the current frame rate counters.
				Application.Current.Host.Settings.EnableFrameRateCounter = true;

				// Show the areas of the app that are being redrawn in each frame.
				//Application.Current.Host.Settings.EnableRedrawRegions = true;

				// Enable non-production analysis visualization mode, 
				// which shows areas of a page that are being GPU accelerated with a colored overlay.
				//Application.Current.Host.Settings.EnableCacheVisualization = true;
			}

			// Standard Silverlight initialization
			InitializeComponent();

			// Phone-specific initialization
			InitializePhoneApplication();
		}

		// Code to execute when the application is launching (eg, from Start)
		// This code will not execute when the application is reactivated
		private void Application_Launching( object sender, LaunchingEventArgs e )
		{
			AuthenticationManager.LoadKeys();
		}

		// Code to execute when the application is activated (brought to foreground)
		// This code will not execute when the application is first launched
		private void Application_Activated( object sender, ActivatedEventArgs e )
		{
			var state = this.RetrieveFromPhoneState();

			if (state != null && state.Count > 0)
			{
				if (state.ContainsKey("CharacterSearch_Realm"))
				{
					ViewModelLocator.CharacterSearchStatic.Realm = (string)state["CharacterSearch_Realm"];
				}
				if (state.ContainsKey("CharacterSearch_Name"))
				{
					ViewModelLocator.CharacterSearchStatic.Name = (string)state["CharacterSearch_Name"];
				}
				if (state.ContainsKey("CharacterDetails_Character"))
				{
					ViewModelLocator.CharacterDetailsStatic.Character = (Character)state["CharacterDetails_Character"];
				}
				if (state.ContainsKey("GuildSearch_Realm"))
				{
					ViewModelLocator.GuildSearchStatic.Realm = (string)state["GuildSearch_Realm"];
				}
				if (state.ContainsKey("GuildSearch_Name"))
				{
					ViewModelLocator.GuildSearchStatic.Name = (string)state["GuildSearch_Name"];
				}
				if (state.ContainsKey("GuildDetails_Guild"))
				{
					ViewModelLocator.GuildDetailsStatic.Guild = (Guild)state["GuildDetails_Guild"];
				}
				if (state.ContainsKey("RealmList_Cache"))
				{
					CacheManager.CachedRealmLists = (Dictionary<Region, RealmList>)state["RealmList_Cache"];
				}
				if (state.ContainsKey("CharacterDetails_CachedItems"))
				{
					ViewModelLocator.CharacterDetailsStatic.CachedItems = (Dictionary<int, Item>)state["CharacterDetails_CachedItems"];
				}
				if (state.ContainsKey("CharacterDetails_CachedGems"))
				{
					ViewModelLocator.CharacterDetailsStatic.CachedGems = (Dictionary<int, Item>)state["CharacterDetails_CachedGems"];
				}
				if (state.ContainsKey("CharacterDetails_ItemToolTip_IsOpen"))
				{
					ViewModelLocator.CharacterDetailsStatic.IsItemToolTipOpen = (bool)state["CharacterDetails_ItemToolTip_IsOpen"];
				}
				if (state.ContainsKey("CharacterDetails_ItemToolTip_ItemContainerControl"))
				{
					ViewModelLocator.CharacterDetailsStatic.ItemContainerControl = (string)state["CharacterDetails_ItemToolTip_ItemContainerControl"];
				}
				if (state.ContainsKey("GroupManagement_Name"))
				{
					ViewModelLocator.GroupManagementStatic.Name = (string)state["GroupManagement_Name"];
				}
			}
		}

		// Code to execute when the application is deactivated (sent to background)
		// This code will not execute when the application is closing
		private void Application_Deactivated( object sender, DeactivatedEventArgs e )
		{
			// save all settings
			IsolatedStorageManager.Save();

			this.SaveToPhoneState("CharacterSearch_Realm", ViewModelLocator.CharacterSearchStatic.Realm);
			this.SaveToPhoneState("CharacterSearch_Name", ViewModelLocator.CharacterSearchStatic.Name);
			this.SaveToPhoneState("CharacterDetails_Character", ViewModelLocator.CharacterDetailsStatic.Character);
			this.SaveToPhoneState("GuildSearch_Realm", ViewModelLocator.GuildSearchStatic.Realm);
			this.SaveToPhoneState("GuildSearch_Name", ViewModelLocator.GuildSearchStatic.Name);
			this.SaveToPhoneState("GuildDetails_Guild", ViewModelLocator.GuildDetailsStatic.Guild);
			this.SaveToPhoneState("RealmList_Cache", CacheManager.CachedRealmLists);
			this.SaveToPhoneState("CharacterDetails_CachedItems", ViewModelLocator.CharacterDetailsStatic.CachedItems);
			this.SaveToPhoneState("CharacterDetails_CachedGems", ViewModelLocator.CharacterDetailsStatic.CachedGems);
			this.SaveToPhoneState("CharacterDetails_ItemToolTip_IsOpen", ViewModelLocator.CharacterDetailsStatic.IsItemToolTipOpen);
			this.SaveToPhoneState("CharacterDetails_ItemToolTip_ItemContainerControl", ViewModelLocator.CharacterDetailsStatic.ItemContainerControl);
			this.SaveToPhoneState("GroupManagement_Name", ViewModelLocator.GroupManagementStatic.Name);
		}

		// Code to execute when the application is closing (eg, user hit Back)
		// This code will not execute when the application is deactivated
		private void Application_Closing( object sender, ClosingEventArgs e )
		{
			AppSettingsManager.IsFirstTimeUsage = IsolatedStorageManager.GetValue("Temp_Setting_IsFirstTimeUsage", false);
			IsolatedStorageManager.Save();
		}

		// Code to execute if a navigation fails
		private void RootFrame_NavigationFailed( object sender, NavigationFailedEventArgs e )
		{
			if ( System.Diagnostics.Debugger.IsAttached )
			{
				// A navigation has failed; break into the debugger
				System.Diagnostics.Debugger.Break();
			}
		}

		// Code to execute on Unhandled Exceptions
		private void Application_UnhandledException( object sender, ApplicationUnhandledExceptionEventArgs e )
		{
			if ( System.Diagnostics.Debugger.IsAttached )
			{
				// An unhandled exception has occurred; break into the debugger
				System.Diagnostics.Debugger.Break();
			}
		}

		#region Phone application initialization

		// Avoid double-initialization
		private bool phoneApplicationInitialized = false;

		// Do not add any additional code to this method
		private void InitializePhoneApplication()
		{
			if ( phoneApplicationInitialized )
				return;

			// Create the frame but don't set it as RootVisual yet; this allows the splash
			// screen to remain active until the application is ready to render.
			//RootFrame = new PhoneApplicationFrame();
			RootFrame = new RadPhoneApplicationFrame();
			RootFrame.Navigated += CompleteInitializePhoneApplication;

			// Handle navigation failures
			RootFrame.NavigationFailed += RootFrame_NavigationFailed;

			// Ensure we don't initialize again
			phoneApplicationInitialized = true;

			// register all available pages
			ApplicationController.Current.Register(Page.Main, new Uri("/Views/MainPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.Settings, new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.Help, new Uri("/Views/HelpPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.News, new Uri("/Views/NewsPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.RealmList, new Uri("/Views/RealmListPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.CharacterList, new Uri("/Views/CharacterListPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.CharacterSearch, new Uri("/Views/CharacterSearchPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.CharacterDetails, new Uri("/Views/CharacterDetailsPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.GuildList, new Uri("/Views/GuildListPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.GuildSearch, new Uri("/Views/GuildSearchPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.GuildDetails, new Uri("/Views/GuildDetailsPage.xaml", UriKind.Relative));
			ApplicationController.Current.Register(Page.GroupManagement, new Uri("/Views/GroupManagementPage.xaml", UriKind.Relative));
		}

		// Do not add any additional code to this method
		private void CompleteInitializePhoneApplication( object sender, NavigationEventArgs e )
		{
			// Set the root visual to allow the application to render
			if ( RootVisual != RootFrame )
				RootVisual = RootFrame;

			// Remove this handler since it is no longer needed
			RootFrame.Navigated -= CompleteInitializePhoneApplication;
		}

		#endregion
		//---------------------------------------------------------------------------
		#endregion
		//---------------------------------------------------------------------------
	}
}