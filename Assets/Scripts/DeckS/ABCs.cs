using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABCs : DeckScript {
    public override bool CheckRule(int[] Pams, int[] PreviousPams, bool isStepping = false, int seed = 0) {
        bool ret = false;

        if (seed >= 0) {
            seed = sequenceIndex % 3;
        }

        switch (seed) {
            case -3:
                ret = Pams[0] == PreviousPams[0]; // согласная
                break;
            case -2:
                ret = Pams[1] == 1; // согласная
                break;
            case -1:
                ret = Pams[1] == 0; // гласная
                break;
            case 0:
                Log("гласные/согласные/пробелы");
                ret = PreviousPams == null || Pams[1] != PreviousPams[1];
                break;
            case 1:
                Log("удвоенные согласные", true);
                if (step % 2 == 0)
                    ret = CheckRule(Pams, PreviousPams, false, -2);
                if (step % 2 == 1)
                    ret = CheckRule(Pams, PreviousPams, false, -3);
                break;
            case 2:
                Log("пять букв", true);
                if (step == numbers[5] % 6)
                    ret = Pams[1] == 2;
                else {
                    if (step == 0)
                        ret = CheckRule(Pams, PreviousPams, false, -1 - numbers[0] % 2);
                    if (step == 1)
                        ret = CheckRule(Pams, PreviousPams, false, -1 - numbers[1] % 2);
                    if (step == 2)
                        ret = CheckRule(Pams, PreviousPams, false, -1 - numbers[2] % 2);
                    if (step == 3)
                        ret = CheckRule(Pams, PreviousPams, false, -1 - numbers[3] % 2);
                    if (step == 4)
                        ret = CheckRule(Pams, PreviousPams, false, -1 - numbers[4] % 2);
                }

                break;
        }

        if (ret && isStepping) {
            step++;
            if (step > numbers[5] % 6)
                step = 0;
        }

        return ret;
    }
}