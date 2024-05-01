using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ToolManager : MonoBehaviour
    {
        [SerializeField] private Button[] toolButtons;
        private Color normalColor = new Color(156f / 255f, 156f / 255f, 156f / 255f); // Normal color for buttons

        private void Start()
        {
            if (GameManager.Instance.Tools.Count != toolButtons.Length)
            {
                Debug.Log("Mismatch between toolData and buttons array lengths!");
                return;
            }

            for (var i = 0; i < GameManager.Instance.Tools.Count; i++)
            {
                var button = toolButtons[i].GetComponent<Button>();
                var data = GameManager.Instance.Tools[i];
                button.onClick.AddListener(() => SelectTool(button, data.ToolType));
            }
        }

        private void SelectTool(Button clickedButton,ToolType toolName)
        {
            foreach (var button in toolButtons)
            {
                var buttonColors = button.colors;

                if (button == clickedButton)
                {
                    // Change normal color of the clicked button to yellow
                    buttonColors.selectedColor = Color.yellow;
                    buttonColors.normalColor = Color.yellow;
                }
                else
                {
                    // Set normal color to the default color for other buttons
                    buttonColors.selectedColor = normalColor;
                    buttonColors.normalColor = normalColor;
                }

                // Apply the modified colors to the button
                button.colors = buttonColors;
            }
            GameManager.Instance.OnToolChanged?.Invoke(toolName);
        }
    }
}