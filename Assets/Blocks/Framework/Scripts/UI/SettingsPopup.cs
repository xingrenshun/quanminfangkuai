using System.Collections;
using System.Collections.Generic;
using TTSDK.SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace BBG
{
	public class SettingsPopup : Popup
	{
		#region Inspector Variables

		[Space]

		[SerializeField] private ToggleSlider	musicToggle = null;
		[SerializeField] private ToggleSlider	soundToggle = null;
		[SerializeField] private CliclImageChage music = null;
		[SerializeField] private CliclImageChage sound = null;

		#endregion

		#region Unity Methods

		private void Start()
		{
			//musicToggle.SetToggle(SoundManager.Instance.IsMusicOn, false);
			//soundToggle.SetToggle(SoundManager.Instance.IsSoundEffectsOn, false);

			//musicToggle.OnValueChanged += OnMusicValueChanged;
			//soundToggle.OnValueChanged += OnSoundEffectsValueChanged;

			music.ChangeImage(SoundManager.Instance.IsMusicOn);
			sound.ChangeImage(SoundManager.Instance.IsSoundEffectsOn);

			music.GetComponent<Button>().onClick.AddListener(() => { music.ClickButton(() => { SoundManager.Instance.SetSoundTypeOnOff(SoundManager.SoundType.Music, music.IsOn); }); });
			sound.GetComponent<Button>().onClick.AddListener(() => { sound.ClickButton(() => { SoundManager.Instance.SetSoundTypeOnOff(SoundManager.SoundType.SoundEffect, sound.IsOn); }); });
		}

		#endregion

		#region Private Methods

		private void OnMusicValueChanged(bool isOn)
		{
			SoundManager.Instance.SetSoundTypeOnOff(SoundManager.SoundType.Music, isOn);
		}


		private void OnSoundEffectsValueChanged(bool isOn)
		{
			SoundManager.Instance.SetSoundTypeOnOff(SoundManager.SoundType.SoundEffect, isOn);
		}

		#endregion
	}
}
