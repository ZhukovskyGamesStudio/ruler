public static class DeckRuleFactory {
    public static DeckScript GetRuleByType(DeckConfig deckType) {
        return deckType.DeckType switch {
            DeckType.Domino => new Domino(deckType),
            DeckType.ABCs => new ABCs(deckType),
            DeckType.Tutorial => new TutorialDeck(deckType),
            DeckType.Colours => new Colours(deckType),
            DeckType.Dancers => new Dancers(deckType),
            DeckType.Stars => new Stars(deckType),
            DeckType.Flowers => new Flowers(deckType),
            _ => throw new System.ArgumentException("Invalid deck type")
        };
    }
}