using UnityEngine;

public class Domino : DeckScript {
    public Domino(DeckConfig config) : base(config) { }

    public override bool CheckRule(int[] Pams, int[] PreviousPams, bool isStepping = false, int seed = 0) {
        bool ret = false;

        if (seed >= 0) {
            seed = sequenceIndex % 7;
        }

        switch (seed) {
            case -2:
                ret = (numbers[0] % 2 == 0 ? Pams[0] != numbers[0] % PamsVariety[0] : Pams[1] != numbers[0] % PamsVariety[0]);
                break;
            case -1:
                ret = (numbers[0] % 2 == 0 ? Pams[0] == numbers[0] % PamsVariety[0] : Pams[1] == numbers[0] % PamsVariety[0]);
                break;
            case 0:
                Log("верх строго больше или меньше низа");
                ret = (numbers[0] % 2 == 0 ? Pams[0] > Pams[1] : Pams[0] < Pams[1]);
                break;
            case 1:
                Log("разница верх и низ равна числу");
                ret = (Mathf.Abs(Pams[0] - Pams[1]) == 1 + numbers[0] % 3);
                break;
            case 2:
                Log("верх или низ кратны числу");
                ret = (numbers[0] % 2 == 0 ? Pams[0] % (2 + numbers[0] % 2) == 0 : Pams[1] % (numbers[0] % 2 + 2) == 0);
                break;
            case 3:
                Log("верх + низ больше числа");
                ret = (Pams[0] + Pams[1] > 4 + numbers[0] % 4);
                break;
            case 5:
                Log("верх = прошлый низ или наоборот");
                ret = PreviousPams == null || (numbers[0] % 2 == 0 ? Pams[0] == PreviousPams[1] : Pams[1] == PreviousPams[0]);
                break;
            case 6:
                Log("верх или низ = и != числу", true);
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