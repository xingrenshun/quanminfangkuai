using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BBG
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class CurrencyText : MonoBehaviour
	{
		#region Inspector Variables

		[SerializeField] private string	currencyId			= "";
		[SerializeField] private bool	displayZeroString	= false;
		[SerializeField] private string	zeroString			= "";

		#endregion

		#region Member Variables

		private TextMeshProUGUI uiText;

		#endregion

		#region Unity Methods

		private void Start()
		{
			uiText = gameObject.GetComponent<TextMeshProUGUI>();

			UpdateAmountText();

			CurrencyManager.Instance.OnCurrencyChanged += OnCurrencyChanged;
		}

		#endregion

		#region Private Methods

		private void OnCurrencyChanged(string id)
		{
			if (currencyId == id)
			{
				UpdateAmountText();
			}
		}

		private void UpdateAmountText()
		{
			int amount = CurrencyManager.Instance.GetAmount(currencyId);

			uiText.text = (amount == 0 && displayZeroString) ? zeroString : amount.ToString();
		}

		#endregion
	}
}
