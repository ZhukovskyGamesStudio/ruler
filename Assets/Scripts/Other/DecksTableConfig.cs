using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecksTableConfig", menuName = "Scriptable Objects/DecksTableConfig")]
public class DecksTableConfig : ScriptableObject {
    public List<DeckConfig> Decks;
}