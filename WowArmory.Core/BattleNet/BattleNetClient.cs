﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using WowArmory.Core.BattleNet.Helpers;
using WowArmory.Core.BattleNet.Models;
using WowArmory.Core.Extensions;
using WowArmory.Core.Managers;

namespace WowArmory.Core.BattleNet
{
	public class BattleNetClient
	{
		//----------------------------------------------------------------------
		#region --- Fields ---
		//----------------------------------------------------------------------
		private static BattleNetClient _instance;
		private Dictionary<CharacterFaction, List<CharacterRace>> _factionRaces;
		//----------------------------------------------------------------------
		#endregion
		//----------------------------------------------------------------------


		//----------------------------------------------------------------------
		#region --- Properties ---
		//----------------------------------------------------------------------
		/// <summary>
		/// Gets the singleton for this instance.
		/// </summary>
		public static BattleNetClient Current
		{
			get { return _instance ?? (_instance = new BattleNetClient(AppSettingsManager.Region, CultureInfo.CurrentUICulture)); }
		}

		/// <summary>
		/// Gets the faction races.
		/// </summary>
		public Dictionary<CharacterFaction, List<CharacterRace>> FactionRaces
		{
			get { return _factionRaces; }
		}

		/// <summary>
		/// Gets or sets the Battle.Net region used to retrieve data from.
		/// </summary>
		/// <value>
		/// The Battle.Net region used to retrieve data from.
		/// </value>
		public Region Region { get; set; }

		/// <summary>
		/// Gets or sets the locale in which the data is retrieved.
		/// </summary>
		/// <value>
		/// The locale in which the data is retrieved.
		/// </value>
		public CultureInfo Locale { get; set; }

		/// <summary>
		/// Gets the Battle.Net region code used in web requests.
		/// </summary>
		public string BattleNetRegionCode
		{
			get
			{
				var regionCode = BattleNetSettings.ResourceManager.GetString(String.Format("BattleNet_Region_{0}", Region));
				return !String.IsNullOrEmpty(regionCode) ? regionCode : "us";
			}
		}

		/// <summary>
		/// Gets the Battle.Net base URI.
		/// </summary>
		public Uri BattleNetBaseUri
		{
			get
			{
				var baseUriTemplate = BattleNetSettings.ResourceManager.GetString("BattleNet_BaseUri");
				return !String.IsNullOrEmpty(baseUriTemplate) ? new Uri(String.Format(baseUriTemplate, BattleNetRegionCode)) : new Uri(String.Format("http://{0}.battle.net/api/wow/", BattleNetRegionCode));
			}
		}

		/// <summary>
		/// Gets the Battle.Net icon URI.
		/// </summary>
		public Uri BattleNetIconUri
		{
			get
			{
				var baseUriTemplate = BattleNetSettings.ResourceManager.GetString("BattleNet_IconUri");
				return !String.IsNullOrEmpty(baseUriTemplate) ? new Uri(String.Format(baseUriTemplate, BattleNetRegionCode)) : new Uri(String.Format("http://{0}.media.blizzard.com/wow/icons/", BattleNetRegionCode));
			}
		}

		/// <summary>
		/// Gets the Battle.Net thumbnail URI.
		/// </summary>
		public Uri BattleNetThumbnailUri
		{
			get
			{
				var baseUriTemplate = BattleNetSettings.ResourceManager.GetString("BattleNet_ThumbnailUri");
				return !String.IsNullOrEmpty(baseUriTemplate) ? new Uri(String.Format(baseUriTemplate, BattleNetRegionCode)) : new Uri(String.Format("http://{0}.battle.net/static-render/{0}/", BattleNetRegionCode));
			}
		}
		//----------------------------------------------------------------------
		#endregion
		//----------------------------------------------------------------------


		//----------------------------------------------------------------------
		#region --- Constructor ---
		//----------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="BattleNetClient"/> class.
		/// </summary>
		public BattleNetClient(Region region, CultureInfo locale)
		{
			Region = region;
			Locale = locale;

			_factionRaces = new Dictionary<CharacterFaction, List<CharacterRace>>();

			var allianceFactions = new List<CharacterRace>();
			var hordeFactions = new List<CharacterRace>();
			
			allianceFactions.Add(CharacterRace.Draenei);
			allianceFactions.Add(CharacterRace.Dwarf);
			allianceFactions.Add(CharacterRace.Gnome);
			allianceFactions.Add(CharacterRace.Human);
			allianceFactions.Add(CharacterRace.NightElf);
			allianceFactions.Add(CharacterRace.Worgen);
			hordeFactions.Add(CharacterRace.BloodElf);
			hordeFactions.Add(CharacterRace.Goblin);
			hordeFactions.Add(CharacterRace.Orc);
			hordeFactions.Add(CharacterRace.Tauren);
			hordeFactions.Add(CharacterRace.Troll);
			hordeFactions.Add(CharacterRace.Undead);

			_factionRaces.Add(CharacterFaction.Alliance, allianceFactions);
			_factionRaces.Add(CharacterFaction.Horde, hordeFactions);
		}
		//----------------------------------------------------------------------
		#endregion
		//----------------------------------------------------------------------


		//----------------------------------------------------------------------
		#region --- Methods ---
		//----------------------------------------------------------------------
		/// <summary>
		/// Gets the basic character information for the specified realm and character name.
		/// </summary>
		/// <param name="realmName">The name of the realm.</param>
		/// <param name="characterName">The name of the character.</param>
		/// <param name="action">The action to execute once the response was received.</param>
		public void GetCharacterAsync(string realmName, string characterName, Action<Character> action)
		{
			GetCharacterAsync(realmName, characterName, CharacterFields.Basic, action);
		}

		/// <summary>
		/// Gets the character information for the specified realm and character name.
		/// </summary>
		/// <param name="realmName">The name of the realm.</param>
		/// <param name="characterName">The name of the character.</param>
		/// <param name="fields">Specifies the information to retrieve for this character.</param>
		/// <param name="action">The action to execute once the response was received.</param>
		public void GetCharacterAsync(string realmName, string characterName, CharacterFields fields, Action<Character> action)
		{
			var fieldsQueryString = BuildCharacterFieldsQueryString(fields);
			var apiMethod = new Uri(BattleNetBaseUri, String.Format(BattleNetSettings.BattleNet_Api_Character, realmName, characterName, fieldsQueryString)).ToString();
			CallApiMethodAsync(apiMethod, jsonResult =>
			{
				var character = JsonConvert.DeserializeObject<Character>(jsonResult);
				character.Region = Region;
				action(character);
			});
		}

		/// <summary>
		/// Gets the realm list for the specified region.
		/// </summary>
		/// <param name="region">The region to receive the realm list for.</param>
		/// <param name="action">The action to execute once the response was received.</param>
		public void GetRealmListAsync(Region region, Action<RealmList> action)
		{
			var oldRegion = Region;
			Region = region;
			var apiMethod = new Uri(BattleNetBaseUri, BattleNetSettings.BattleNet_Api_RealmStatus).ToString();
			Region = oldRegion;
			CallApiMethodAsync(apiMethod, jsonResult =>
			{
				var realmList = JsonConvert.DeserializeObject<RealmList>(jsonResult.Replace("n/a", "notavailable"));
				realmList.Region = Region;
				action(realmList);
			});
		}

		/// <summary>
		/// Gets the thumbnail url for the specified path.
		/// </summary>
		/// <param name="thumbnailPath">The path to the thumbnail.</param>
		/// <returns>
		/// The thumbnail url for the specified path.
		/// </returns>
		public string GetThumbnailUrl(string thumbnailPath)
		{
			return new Uri(BattleNetThumbnailUri, thumbnailPath).ToString();
		}

		/// <summary>
		/// Gets the thumbnail from the specified path.
		/// </summary>
		/// <param name="thumbnailPath">The path to the thumbnail.</param>
		/// <returns>
		/// The thumbnail from the specified path.
		/// </returns>
		public ImageSource GetThumbnail(string thumbnailPath)
		{
			return CacheManager.GetImageSourceFromCache(GetThumbnailUrl(thumbnailPath));
		}

		/// <summary>
		/// Gets the icon from the specified path.
		/// </summary>
		/// <param name="iconPath">The path to the icon.</param>
		/// <param name="iconSize">The desired size of the icon.</param>
		/// <returns>
		/// The icon from the specified path.
		/// </returns>
		public ImageSource GetIcon(string iconPath, IconSize iconSize = IconSize.Large)
		{
			var iconUrl = new Uri(BattleNetIconUri, String.Format("{0}/{1}.jpg", GetSettingValue(iconSize), iconPath)).ToString();
			return CacheManager.GetImageSourceFromCache(iconUrl);
		}

		/// <summary>
		/// Builds the fields query string from the specified fields object.
		/// </summary>
		/// <param name="e">The fields object to build the query string from.</param>
		/// <returns>
		/// The fields query string from the specified fields object.
		/// </returns>
		internal string BuildCharacterFieldsQueryString(CharacterFields fields)
		{
			var fieldQueryString = fields.GetValues().Cast<CharacterFields>().Where(value => (fields & value) == value).Aggregate(String.Empty, (current, value) => String.Format("{0}{1}{2}", current, !String.IsNullOrEmpty(current) ? "," : String.Empty, EnumHelper.GetApiUrlFieldName(value)));
			return !String.IsNullOrEmpty(fieldQueryString) ? String.Format("?fields={0}", fieldQueryString) : String.Empty;
		}

		/// <summary>
		/// Handler which asynchronously calls the specified api method.
		/// </summary>
		/// <param name="apiMethod">The API method to call.</param>
		/// <param name="action">The json result action.</param>
		internal void CallApiMethodAsync(string apiMethod, Action<string> action)
		{
			try
			{
				var apiMethodLocalized = String.Format("{0}{1}locale={2}", apiMethod, apiMethod.Contains("?") ? "&" : "?", Locale.Name.Replace("-", "_").ToLower());
				var request = (HttpWebRequest)WebRequest.Create(apiMethodLocalized);
				request.Headers["user-agent"] = BattleNetSettings.WebRequest_Header_UserAgent;
				request.AllowAutoRedirect = true;

				request.BeginGetResponse(delegate(IAsyncResult result)
				{
					try
					{
						WebResponse response;

						// in some cases the Battle.Net community api may return an exception (404/500 error)
						// we just ignore it and try to read the content of the response
						try
						{
							response = request.EndGetResponse(result);
						}
						catch (WebException ex)
						{
							response = ex.Response;
						}

						using (var stream = response.GetResponseStream())
						{
							var streamReader = new StreamReader(stream);
							var jsonResultString = streamReader.ReadToEnd();
							Deployment.Current.Dispatcher.BeginInvoke(() => action(jsonResultString));
						}
					}
					catch (Exception ex)
					{
						Deployment.Current.Dispatcher.BeginInvoke(() => action(null));
					}
				}, null);
			}
			catch(Exception ex)
			{
				// TODO: need to implement error handling
				throw ex;
			}
		}

		/// <summary>
		/// Gets the configured setting for the specified icon size.
		/// </summary>
		/// <param name="iconSize">The icon size to retrieve the configured setting for.</param>
		/// <returns>
		/// The configured setting for the specified icon size.
		/// </returns>
		internal string GetSettingValue(IconSize iconSize)
		{
			var resourceName = String.Format("BattleNet_IconSize_{0}", iconSize);
			return BattleNetSettings.ResourceManager.GetString(resourceName);
		}
		//----------------------------------------------------------------------
		#endregion
		//----------------------------------------------------------------------
	}
}