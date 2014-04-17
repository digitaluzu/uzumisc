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
			#pragma warning disable 168
			const string title = "Rate Us";
			const string yes = "Rate";
			const string later = "Later";
			const string no = "No Thanks";
			#pragma warning restore 168

#if UNITY_IPHONE
			IOSRateUsPopUp.Create (title, message, yes, later, no);
#elif UNITY_ANDROID
			string url = "http://play.google.com/store/apps/details?id=" + appId;

			AndroidRateUsPopUp.Create (title, message, url, yes, later, no);
#endif
		}

		public static void ShowMailComposer (string url, string subject, string body, bool isHTML)
		{
			Application.OpenURL("mailto:" + url + "?subject=" + subject + "&body=" + body);
		}

		public static void ShowWebpage (string url, bool showControls)
		{
			Application.OpenURL(url);
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
			// Prevent 'unused variable' warning.
			#pragma warning disable 414
			private string _yesString = null;
			private string _noString = null;
			private System.Action <string> _userCallback = null;
			#pragma warning restore 414

			public AlertDialog (string title, string message, string yes, string no, System.Action <string> callback)
			{
#if UNITY_EDITOR
				// Do nothing.
#elif UNITY_IPHONE
				// TODO: iOS Native Plugin order is currently incorrect. Bug report submitted.
				_yesString = no;
				_noString = yes;
				//_yesString = yes;
				//_noString = no;
				_userCallback = callback;

				IOSDialog dialog = IOSDialog.Create (title, message, yes, no);
				dialog.addEventListener (BaseEvent.COMPLETE, EventListenerCallbackImpl);
#elif UNITY_ANDROID
				_yesString = yes;
				_noString = no;
				_userCallback = callback;

				AndroidDialog dialog = AndroidDialog.Create(title, message, yes, no);
				dialog.addEventListener (BaseEvent.COMPLETE, EventListenerCallbackImpl);
#endif
			}

#if UNITY_IPHONE
			private void EventListenerCallbackImpl (CEvent e)
			{
				// Remove listener.
				(e.dispatcher as IOSDialog).removeEventListener (BaseEvent.COMPLETE, EventListenerCallbackImpl);

				if (_userCallback != null) {
					switch ((IOSDialogResult)e.data) {
					case IOSDialogResult.YES:
						_userCallback (_yesString);
						break;
					case IOSDialogResult.NO:
						_userCallback (_noString);
						break;
					}
				}
			}
#elif UNITY_ANDROID
			private void EventListenerCallbackImpl (CEvent e)
			{
				// Remove listener.
				(e.dispatcher as AndroidDialog).removeEventListener (BaseEvent.COMPLETE, EventListenerCallbackImpl);

				if (_userCallback != null) {
					switch ((AndroidDialogResult)e.data) {
					case AndroidDialogResult.YES:
						_userCallback (_yesString);
						break;
					case AndroidDialogResult.NO:
						_userCallback (_noString);
						break;		
					}
				}
			}
#endif
		}
		#endregion
	}
}
