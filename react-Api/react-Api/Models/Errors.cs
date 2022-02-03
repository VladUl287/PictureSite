namespace react_Api.Models
{
    public static class Errors
    {
        public static readonly Error NotCorrectImage = new("Ошибка изображения.");
        public static readonly Error FileNotExists = new("Такого файла не существует.");
        public static readonly Error NotCorrectEmailOrPassword = new("Неверный email или пароль.");
        public static readonly Error NotCorrectImageSize = new("Некорректные размеры изображения.");
        public static readonly Error UserAlreadyExists = new("Пользователь с таким email уже существует.");
    }
}