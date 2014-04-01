using UnityEngine;
using System.Collections;

/// <summary>
/// User interface dlg box.
/// MEMO: I use an UIpanel because I want to be able to call the UiDlgBox anywhere as a static instance without having to register
/// the instance anywhere. the UiPanel OnInitialize will create the UiDlgBox Instance.
/// How to use:
/// Uzu.UiDlgBox.Instance.Show(//)
/// TODO:
///	Do something about "CyUiNavGroup" to remove dependency from CY
/// </summary>
namespace Uzu
{
	/// <summary>
	/// Generic message box class capable of displaying variable text.
	/// </summary>
	public class UiDlgBox : Uzu.UiPanel
	{
		//TODO I'm not sure the size will work well on all platform yet..
		//Dlg size
		private const int OK_DLG_SIZE = 500;
		private const int YES_NO_DLG_SIZE = 500;
		private const int YES_NO_OTHER_DLG_SIZE = 720;
		//Dlg button size
		private const int OK_DLG_BTN_SIZE = 200;
		private const int YES_NO_BTN_SIZE = 125;
		private const int YES_NO_OTHER_BTN_SIZE = 125;
		//Dlg components
		[SerializeField]
		private UILabel _title;
		[SerializeField]
		private UILabel _body;
		[SerializeField]
		private UIPanel _dimmerScreen;
		[SerializeField]
		private GameObject _background;
		[SerializeField]
		private GameObject _yesButton;
		[SerializeField]
		private GameObject _noButton;
		[SerializeField]
		private GameObject _otherButton;
		/// <summary>
		/// simplely prevent two box to oppen in same time (TODO give this posiblility)
		/// </summary>
		private bool mIsVisible = false;
		/// <summary>
		/// Callback function that will be invoked when the message box is closed.
		/// </summary>
		private   System.Action <string> _callback;
		private   string _responce;

		/// <summary>
		/// Whether the message box is currently visible.
		/// </summary>
		public bool IsVisible { get { return mIsVisible; } }

		/// <summary>
		/// Show a single-button dialog box.
		/// </summary>
		public void Show (string titleText, string bodyText, string okText, System.Action<string> onCompleteCallback)
		{
			if (!mIsVisible) {
				
				mIsVisible = true;
				
				
				NGUITools.SetActive (gameObject, true);
				
				NGUITools.SetActive (_yesButton, true);
				NGUITools.SetActive (_noButton, false);
				NGUITools.SetActive (_otherButton, false);

				//Set the size
				{
					_yesButton.GetComponentInChildren<UISprite> ().width = OK_DLG_BTN_SIZE;
					_background.GetComponent<UISprite> ().width = OK_DLG_SIZE;	
				}

				//Set the Dlg Text
				{
					_title.text = titleText;
					_body.text = bodyText;
	
					_yesButton.GetComponentInChildren<UILabel> ().text = okText;
				}
				
				//Set the Naviation mechanic TODO .... Change this to generic UZU.UiNavItem
				{
					//Disable the bacground menu naviation component
					DisableNavGroup ();
					CyUiNavItem[] nav = _yesButton.GetComponentsInChildren<CyUiNavItem> ();
					for (int i = 0; i < nav.Length; i++) {
						nav [i].LeftItem = nav [i];
						nav [i].RightItem = nav [i];
					}
					
				}
		
				UICamera.selectedObject = _yesButton;
				
				_callback = onCompleteCallback;
				
				animation.Play ("UIDlgOpen", PlayMode.StopAll);

			}
		}

		/// <summary>
		/// Show a yes/no dialog box.
		/// </summary>
		public void Show (string titleText, string bodyText, string yesText, string noText, System.Action<string> onCompleteCallback)
		{
			if (!mIsVisible) {
				
				mIsVisible = true;
				DisableNavGroup ();
				
				NGUITools.SetActive (gameObject, true);
				
				NGUITools.SetActive (_yesButton, true);
				NGUITools.SetActive (_noButton, true);
				NGUITools.SetActive (_otherButton, false);
			
				//Set the size
				{
					_yesButton.GetComponentInChildren<UISprite> ().width = YES_NO_BTN_SIZE;
					_noButton.GetComponentInChildren<UISprite> ().width = YES_NO_BTN_SIZE;
					_background.GetComponent<UISprite> ().width = YES_NO_DLG_SIZE;	
				}
        
				//Set the dlg Text
				{
					_title.text = titleText;
					_body.text = bodyText;

					_yesButton.GetComponentInChildren<UILabel> ().text = yesText;
					_noButton.GetComponentInChildren<UILabel> ().text = noText;
				}
				
				//Set the Naviation mechanic TODO .... Change this to generic UZU.UiNavItem
				{
					//Disable the bacground menu naviation component
					DisableNavGroup ();
					CyUiNavItem[] yesNav = _yesButton.GetComponentsInChildren<CyUiNavItem> ();
					CyUiNavItem[] noNav = _noButton.GetComponentsInChildren<CyUiNavItem> ();
					for (int i = 0; i < yesNav.Length; i++) {
						yesNav [i].LeftItem = noNav [i];
						yesNav [i].RightItem = noNav [i];
						noNav [i].LeftItem = yesNav [i];
						noNav [i].RightItem = yesNav [i];
					}
					
				}
				
				UICamera.selectedObject = _yesButton;
				
				_callback = onCompleteCallback;
				
				animation.Play ("UIDlgOpen", PlayMode.StopAll);
			}
		}

		/// <summary>
		/// Show a yes/no dialog box.
		/// </summary>
		public void Show (string titleText, string bodyText, string yesText, string noText, string otherText, System.Action<string> onCompleteCallback)
		{
			if (!mIsVisible) {
				
				mIsVisible = true;
				DisableNavGroup ();
				
				NGUITools.SetActive (gameObject, true);			
				
				NGUITools.SetActive (_yesButton, true);
				NGUITools.SetActive (_noButton, true);
				NGUITools.SetActive (_otherButton, true);
	
			
				//Set the size
				{
					_yesButton.GetComponentInChildren<UISprite> ().width = YES_NO_OTHER_BTN_SIZE;
					_noButton.GetComponentInChildren<UISprite> ().width = YES_NO_OTHER_BTN_SIZE;
					_otherButton.GetComponentInChildren<UISprite> ().width = YES_NO_OTHER_BTN_SIZE;
					_background.GetComponent<UISprite> ().width = YES_NO_OTHER_DLG_SIZE;	
				}
			
				//Set the dlg text
				{
					_title.text = titleText;
					_body.text = bodyText;
	
					_yesButton.GetComponentInChildren<UILabel> ().text = yesText;
					_noButton.GetComponentInChildren<UILabel> ().text = noText;
					_otherButton.GetComponentInChildren<UILabel> ().text = otherText;
				}
				
				//Set the Naviation mechanic TODO .... Change this to generic UZU.UiNavItem
				{
					//Disable the bacground menu naviation component
					DisableNavGroup ();
					CyUiNavItem yesNav = _yesButton.GetComponent<CyUiNavItem> ();
					CyUiNavItem noNav = _noButton.GetComponent<CyUiNavItem> ();
					CyUiNavItem otherNav = _otherButton.GetComponent<CyUiNavItem> ();
					yesNav.LeftItem = otherNav;
					yesNav.RightItem = noNav;
					noNav.LeftItem = yesNav;
					noNav.RightItem = otherNav;
					otherNav.LeftItem = noNav;
					otherNav.RightItem = yesNav;
				}
				
				UICamera.selectedObject = _yesButton;
				
				_callback = onCompleteCallback;
				
				animation.Play ("UIDlgOpen", PlayMode.StopAll);
			}
		}
		
		#region Implementation
		#region NavGroup TODO Move this to generic Uzu.UiNavGrou 
		/// <summary>
		/// We don't want to navigate in the background menu when the Dlg is open so let's disable navigation in the current panel
		/// </summary>
		CyUiNavGroup[] _navGroup;

		public void DisableNavGroup ()
		{
			//TODO this will cause problem if we have more that one UI manager with navigation group in the scene
			_navGroup = ((Uzu.UiPanel)(this.OwnerManager.CurrentPanel)).GetComponentsInChildren<CyUiNavGroup> (false);
			for (int i = 0; i < _navGroup.Length; i++) {
				_navGroup [i].enabled = false;
			}
		}

		public void RestoreNavGroup ()
		{
			for (int i = 0; i < _navGroup.Length; i++) {
				_navGroup [i].enabled = true;
			}	
		}
		#endregion
		
		#region Event
		/// <summary>
		/// "Yes" or "OK" button press.
		/// </summary>
		void OnButtonPress (GameObject go, bool state)
		{       
			//Respond only to new press down
			if (state) {
				_responce = go.GetComponentInChildren<UILabel> ().text;
				animation.Play ("UIDlgClose", PlayMode.StopAll);
				UICamera.selectedObject = null;
			}
		}

		public void OnCloseAnimationEnd ()
		{	   
			mIsVisible = false;
			RestoreNavGroup ();
			
			NGUITools.SetActive (gameObject, false);
	
			_callback (_responce);
		}
		#endregion

		#region MonoBehavior
		public void Update ()
		{
			if (cInput.GetButtonDown (CyInputAction.SELECT)) {
				CyUiNavGroup group = this.GetComponent <CyUiNavGroup> ();
				if (group != null) {
					CyUiNavItem activeItem = group.ActiveItem;
					if (activeItem != null) {
						UIEventListener.Get (activeItem.gameObject).onPress (activeItem.gameObject, true);
					}
				} 
			}
		}
		#endregion
		
		#region Singleton implementation.
		private static UiDlgBox _instance;

		public static UiDlgBox Instance {
			get { return _instance; }
		}

		public override void OnInitialize ()
		{
			if (_yesButton != null)
				UIEventListener.Get (_yesButton).onPress = OnButtonPress;
			if (_noButton != null)
				UIEventListener.Get (_noButton).onPress = OnButtonPress;
			if (_otherButton != null)
				UIEventListener.Get (_otherButton).onPress = OnButtonPress;
			_instance = this;
		}

		protected void OnDestroy ()
		{
			if (_instance != this) {
				return;
			}		
			_instance = null;
		}

		#endregion
		#endregion
	}
}