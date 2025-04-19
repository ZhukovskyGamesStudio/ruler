public class Stars : DeckScript {
    public Stars(DeckConfig config) : base(config) { }

    public override bool CheckRule(int[] Pams, int[] PreviousPams, bool isStepping = false, int seed = 0) {
        bool ret = false;

        if (seed >= 0) {
            seed = sequenceIndex % 8;
        }

        switch (seed) {
            case -2:
                ret = PreviousPams == null || Pams[1] < PreviousPams[1];
                break;
            case -1:
                ret = PreviousPams == null || Pams[1] > PreviousPams[1];
                break;
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
            case 3:
                Log("подходит 1,2 или 3 фигуры");
                ret = Pams[1] == numbers[0] % PamsVariety[1] || Pams[1] == numbers[1] % PamsVariety[1] ||
                      Pams[1] == numbers[2] % PamsVariety[1];
                break;
            case 4:
            case 5:
                Log("фигура не равна предыдущей");
                ret = PreviousPams == null || Pams[1] != PreviousPams[1];
                break;
            case 6:
            case 7:
                Log("больше / меньше углов", true);
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