using UnityEngine;

namespace Uzu
{
	// ***************************
	// TODO: Cleanup!!
	//	- unify interfaces and remove platform-specific code from Cy
	// ***************************

	/// <summary>
	/// Miscellaneous services interfaces.
	/// </summary>
	public class Misc
	{
		/// <summary>
		/// Prompts the user to write a review.
		/// </summary>
		public static void AskForReview (string message, string appId)
		{
#if UNITY_IPHONE
			const string title = "Rate This App";
			EtceteraBinding.askForReview (title, message, appId);
#elif UNITY_ANDROID
			const string title = "Rate Us";
			string url = "http://play.google.com/store/apps/details?id=" + appId;
			const string yes = "Rate";
			const string later = "Later";
			const string no = "No Thanks";
			AndroidRateUsPopUp.Create (title, message, url, yes, later, no);
#endif
		}

		/// <summary>
		/// Is an email application set up properly on this device?
		/// </summary>
		public static bool IsMailAvailable ()
		{
#if UNITY_EDITOR
			return false;
#elif UNITY_IPHONE
			return EtceteraBinding.isEmailAvailable();
#elif UNITY_ANDROID
			//We don't use any plugin in android so alwayse return true.
			return true;
#else
			#error Unhandled platform.
#endif // UNITY_IPHONE
		}

		public static void ShowMailComposer (string url, string subject, string body, bool isHTML)
		{
#if UNITY_IPHONE
			EtceteraBinding.showMailComposer(url, subject, body, isHTML);
#else // UNITY_IPHONE
			//By default use OpenURL it should work on any platform
			Application.OpenURL("mailto:" + url + "?subject=" + subject + "&body=" + body);
#endif
		}
		
		public static void ShowWebpage (string url, bool showControls)
		{
#if UNITY_IPHONE
			EtceteraBinding.showWebPage(url, showControls);	
#else 
			//By default use OpenURL it should work on any platform
			Application.OpenURL(url);
#endif
		}
		
		public static void ShowAlert (string title, string message, string[] buttons)
		{
#if UNITY_IPHONE
			EtceteraBinding.showAlertWithTitleMessageAndButtons(title, message, buttons);
#endif // UNITY_IPHONE

			//TODO .. Well the interface is diferent between the two plugin we use for Alert
			//I'll like to fix that but fist let make android work, and fix both Version

		}
		
		public static void RegisterAlertButtonClickedEvent (System.Action<string> eventFunction)
		{
#if UNITY_IPHONE
			EtceteraManager.alertButtonClickedEvent += eventFunction;
#endif // UNITY_IPHONE
		}
		
		public static void UnregisterAlertButtonClickedEvent (System.Action<string> eventFunction)
		{
#if UNITY_IPHONE
			EtceteraManager.alertButtonClickedEvent -= eventFunction;
#endif // UNITY_IPHONE
		}
	}
}
