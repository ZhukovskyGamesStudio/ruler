public class Colours : DeckScript {
    public Colours(DeckConfig config) : base(config) { }

    public override bool CheckRule(int[] Pams, int[] PreviousPams, bool isStepping = false, int seed = 0) {
        bool ret = false;

        if (seed >= 0) {
            seed %= 4;
        }

        switch (seed) {
            case -2:
                return Pams[0] == PreviousPams[0];
            case -1:
                return true;
            case 0:
                Log("подходит 1,2 или 3 цвета");
                ret = Pams[0] == numbers[0] % PamsVariety[0] || Pams[0] == numbers[1] % PamsVariety[0] ||
                      Pams[0] == numbers[2] % PamsVariety[0];
                break;
            case 1:
                Log("цвет не равен предыдущему");
                ret = PreviousPams == null || Pams[0] != PreviousPams[0];
                break;
            case 2:
                Log("закольцованная последовательность цветов");
                ret = PreviousPams == null ||
                      Pams[0] == (PreviousPams[0] + (numbers[PreviousPams[0]]) % (PamsVariety[0] - 1) + 1) % PamsVariety[0];
                break;
            case 3:
                Log("два цвета подряд", true);
                if (step == 0)
                    ret = CheckRule(Pams, PreviousPams, false, -1);
                else
                    ret = CheckRule(Pams, PreviousPams, false, -2);
                break;
        }

        if (ret && isStepping)
            step = (step == 0 ? 1 : 0);

        return ret;
    }
}