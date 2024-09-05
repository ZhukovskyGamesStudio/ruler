using UnityEngine;

public static class TranslateManager {
    static int langIndex = -1; // индекс языка: -1-еще не инициализирован, 0-англ, 1 - русский и т.д.

    public static string Translate(string text_) {
        if (langIndex == -1) // начальная инициализация индекса языка при первом вызове
        {
            switch (Application.systemLanguage.ToString()) {
                case "English":
                    langIndex = 0;
                    break;
                case "Russian":
                    langIndex = 1;
                    break;
                // продолжить для других языков ....
            }
        }

        for (int i = 0; i < labels.GetLength(0); i++) {
            if (text_ == labels[i, 0]) return labels[i, langIndex];
        }

        return text_;
    }

    // здесь будут все наши тексты
    static string[,] labels = {
        {
            "Rulerrr", // англ.
            "Линейкаaaa" // русс.
        }, {
            "Password\n(6-8 chars)",
            "Пароль\n(6-8 симв.)"
        }, {
            "Sign Up",
            "Регистрация"
        }, {
            "Sign In",
            "Войти"
        }
        // ...... ну и т.д., если языков больше, то в каждом блоке будет больше строчек
    };
}