using UnityEngine;

namespace Uzu
{
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

		/// <summary>
		/// Shows an alert dialog, triggering the callback
		/// after the user makes a decision.
		/// Callback passes the button name as a parameter.
		/// </summary>
		public static void ShowAlert (string title, string message, string yes, string no, System.Action <string> callback)
		{
			new AlertDialog (title, message, yes, no, callback);
		}

		#region Implementation.
		private class AlertDialog
		{
			private string _yesString = null;
			private string _noString = null;
			private System.Action <string> _userCallback = null;

			public AlertDialog (string title, string message, string yes, string no, System.Action <string> callback)
			{
#if UNITY_EDITOR
				UiDlgBox.Instance.Show(title,message,yes,no,callback);	
#elif UNITY_IPHONE
				_userCallback = callback;

				EtceteraManager.alertButtonClickedEvent += EventListenerCallbackImpl;

				{
					string[] buttons = { yes, no };
					EtceteraBinding.showAlertWithTitleMessageAndButtons(title, message, buttons);
				}
				
#elif UZU_GAMESTICK
				UiDlgBox.Instance.Show(title,message,yes,no,callback);		
#elif UNITY_ANDROID
				_yesString = yes;
				_noString = no;
				_userCallback = callback;

				AndroidDialog dialog = AndroidDialog.Create(title, message, yes, no);
				dialog.addEventListener (BaseEvent.COMPLETE, EventListenerCallbackImpl);
#endif
			}

#if UNITY_IPHONE
			private void EventListenerCallbackImpl (string buttonName)
			{
				// Remove listener.
				EtceteraManager.alertButtonClickedEvent -= EventListenerCallbackImpl;

				if (_userCallback != null) {
					_userCallback (buttonName);
				}
			}
#elif UNITY_ANDROID
			private void EventListenerCallbackImpl (CEvent e)
			{
				// Remove listener.
				(e.dispatcher as AndroidDialog).removeEventListener (BaseEvent.COMPLETE, EventListenerCallbackImpl);

				if (_userCallback != null) {
					string callbackStr = string.Empty;

					switch ((AndroidDialogResult)e.data) {
					case AndroidDialogResult.YES:
						callbackStr = _yesString;
						break;
					case AndroidDialogResult.NO:
						callbackStr = _noString;
						break;		
					}

					if (!string.IsNullOrEmpty (callbackStr)) {
						_userCallback (callbackStr);
					}
					else {
						Debug.LogWarning ("Unhandled dialog result.");
					}
				}
			}
#endif
		}
		#endregion
	}
}
