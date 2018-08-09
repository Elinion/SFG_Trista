using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelectionMenu : MonoBehaviour {

	private const string EnglishLocalizationFileName = "localizationText_en.json";
	private const string FrenchLocalizationFileName = "localizationText_fr.json";
	
	public void loadEnglish() {
		LocalizationManager.instance.loadLocalizedText(EnglishLocalizationFileName);
	}
	public void loadFrench() {
		LocalizationManager.instance.loadLocalizedText(FrenchLocalizationFileName);
	}
}
