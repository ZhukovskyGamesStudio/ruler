public class Dancers : DeckScript {
    public Dancers(DeckConfig config) : base(config) { }

    public override bool CheckRule(int[] Pams, int[] PreviousPams, bool isStepping = false, int seed = 0) {
        bool ret = false;

        if (seed >= 0) {
            seed = sequenceIndex % 4;
        }

        switch (seed) {
            case -4:
                ret = Pams[0] == (PreviousPams[0] + 1 + numbers[0] % (PamsVariety[0] - 1)) % PamsVariety[0];
                break;
            case -3:
                ret = true;
                break;
            case -2:
                ret = Pams[1] == (numbers[1] + 1 + numbers[2] % (PamsVariety[0] - 1)) % PamsVariety[1];
                break;
            case -1:
                ret = Pams[1] == numbers[1] % PamsVariety[1];
                break;
            case 0:
                Log("каждой фигуре свой галстук");
                ret = Pams[0] == (Pams[2] + numbers[0]) % PamsVariety[1];
                break;
            case 1:
                Log("цикличные движения", true);
                if (step == 0)
                    ret = CheckRule(Pams, PreviousPams, false, -1);
                if (step == 1)
                    ret = CheckRule(Pams, PreviousPams, false, -2);
                if (step == 2)
                    ret = CheckRule(Pams, PreviousPams, false, -1);
                break;
            case 2:
                Log("дресс код");
                ret = Pams[0] != numbers[0] % PamsVariety[0] && Pams[2] != numbers[1] % PamsVariety[2];
                break;
            case 3:
                Log("собрать трёх друзей", true);
                if (step == 0)
                    ret = CheckRule(Pams, PreviousPams, false, -3);
                if (step == 1)
                    ret = CheckRule(Pams, PreviousPams, false, -4);
                if (step == 2)
                    ret = CheckRule(Pams, PreviousPams, false, -4);
                break;
        }

        if (ret && isStepping) {
            step++;
            step %= 3;
        }

        return ret;
    }
}