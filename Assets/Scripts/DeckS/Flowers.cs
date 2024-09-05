using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowers : DeckScript {
    public override bool CheckRule(int[] Pams, int[] PreviousPams, bool isStepping = false, int seed = 0) {
        bool ret = false;

        if (seed >= 0) {
            seed = sequenceIndex % 2;
        }

        switch (seed) {
            case -2:
                ret = (PreviousPams[0] == numbers[0] % PamsVariety[0])
                    ? (Pams[0] == (numbers[0] % PamsVariety[0] + 1 + numbers[1] % (PamsVariety[0] - 2)) % PamsVariety[0])
                    : Pams[0] == numbers[0] % PamsVariety[0];
                break;
            case -1:
                ret = Pams[0] == numbers[0] % PamsVariety[0] ||
                      Pams[0] == (numbers[0] % PamsVariety[0] + 1 + numbers[1] % (PamsVariety[0] - 2)) % PamsVariety[0];
                break;
            case 0:
                Log("два или три цветка");
                ret = numbers[5] % 2 == 0
                    ? Pams[0] == numbers[0] % PamsVariety[0] ||
                      Pams[0] == (numbers[0] % PamsVariety[0] + 1 + numbers[1] % (PamsVariety[0] - 2)) % PamsVariety[0]
                    : Pams[0] == numbers[0] % PamsVariety[0] ||
                      Pams[0] == (numbers[0] % PamsVariety[0] + 1 + numbers[1] % 3) % PamsVariety[0] || Pams[0] ==
                      (numbers[0] % PamsVariety[0] + 4 + numbers[1] % (PamsVariety[0] - 4)) % PamsVariety[0];
                break;
            case 1:
                Log("букет из двух цветов", true);
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