namespace PbkService.Requests.Enums
{
    public enum LoginTypes
    {
        /// <summary>
        /// Вход по нику (логину)
        /// </summary>
        Username = 0,
        /// <summary>
        /// Вход по почте
        /// </summary>
        Email = 1,
        /// <summary>
        /// Вход по номеру телефона
        /// </summary>
        PhoneNumber = 2
    }
}
