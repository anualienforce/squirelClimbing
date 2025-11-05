using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private string rateUsURL = "https://play.google.com/store/apps/details?id=com.yourcompany.yourgame";
    [SerializeField] private string privacyPolicyURL = "https://yourwebsite.com/privacy-policy";

    // Called by your "Rate Us" button OnClick
    public void OnRateUsButton()
    {
        Application.OpenURL(rateUsURL);
    }

    // Called by your "Privacy Policy" button OnClick
    public void OnPrivacyPolicyButton()
    {
        Application.OpenURL(privacyPolicyURL);
    }
}
