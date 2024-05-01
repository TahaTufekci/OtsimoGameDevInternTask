using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ColorManager : MonoBehaviour
    {
        [SerializeField] private Button[] colorButtons;
        private Color _currentSelectedColor;
        private Color _normalColor = new Color(156f / 255f, 156f / 255f, 156f / 255f); // Normal color for buttons
        
        private void Start()
        {
            foreach (var colorButton in colorButtons)
            {
                var button = colorButton.GetComponent<Button>();
                button.onClick.AddListener(() => SelectTool(button));
            }
        }
        private void SelectTool(Button clickedButton)
        {
            foreach (var button in colorButtons)
            {
                var buttonColors = button.colors;

                if (button == clickedButton)
                {
                    // Change normal color of the clicked button to magenta to indicate selection
                    buttonColors.selectedColor = Color.magenta;
                    buttonColors.normalColor = Color.magenta;
                }
                else
                {
                    // Set normal color to the default color for other buttons
                    buttonColors.selectedColor = _normalColor;
                    buttonColors.normalColor = _normalColor;
                }

                // Apply the modified colors to the button
                button.colors = buttonColors;
            }
        }
        private void SetSelectedColor(Color newColor)
        {
            _currentSelectedColor = newColor;
            GameManager.Instance.OnColorChanged?.Invoke(newColor);
        }
        public void OnBlueButtonClick()
        {
            SetSelectedColor(Color.blue);
        }
        public void OnRedButtonClick()
        {
            SetSelectedColor(Color.red);
        }
        public void OnGreenButtonClick()
        {
            SetSelectedColor(Color.green);
        }
        public void OnBlackButtonClick()
        {
            SetSelectedColor(Color.black);
        }
        public void OnYellowButtonClick()
        {
            SetSelectedColor(Color.yellow);
        }
    }
    
}