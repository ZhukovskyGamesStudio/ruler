public class TutorialDeck : DeckScript {
    public TutorialDeck(DeckConfig config) : base(config) { }

    public override bool CheckRule(int[] Pams, int[] PreviousPams, bool isStepping = false, int seed = 0) {
        Log("Чередуются красный и синий");
        return PreviousPams == null || Pams[0] != PreviousPams[0];
    }
}