using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
   [SerializeField]
   private Text _buildVersionText;

   private void Awake() {
      _buildVersionText.text = "Zhukovsky Games Ver" + Application.version;
   }
}
