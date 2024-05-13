using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.User;
using PbkService.Auxiliaries.Exceptions.UserCard;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.ViewModels;

namespace PbkService.Services
{
    public class UserCardService(UserCardRepository userCardRepository, UserRepository userRepository, AlgorithmService algorithmService)
    {
        private readonly UserCardRepository _userCardRepository = userCardRepository;
        private readonly UserRepository _userRepository = userRepository;
        private readonly AlgorithmService _algorithmService = algorithmService;

        public UserCardDTO GetById(int id, string username)
        {
            User user = _userRepository.GetByUsername(username) ?? throw new UserUsernameNotExists($"Пользователя с ником {username} не существует.");
            UserCard userCard = _userCardRepository.Get(id, user.Id) ?? throw new UserCardNotExists($"Пользовательская карта с id = {id} не найдена.");
            DisplayModel<int> card = new() { Id = userCard.CardId, DisplayName = userCard.Card.Name };
            DisplayModel<int> bank = new() { Id = userCard.Card.BankId, DisplayName = userCard.Card.Bank.Name };
            DisplayModel<int> typeCard = new() { Id = userCard.Card.TypeCardId, DisplayName = userCard.Card.TypeCard.Name };
            UserCardDTO userCardDTO = new()
            {
                Card = card,
                Bank = bank,
                TypeCard = typeCard
            };
            return userCardDTO;
        }

        public List<UserCardDTO> GetByUser(string username)
        {
            User user = _userRepository.GetByUsername(username) ?? throw new UserUsernameNotExists($"Пользователя с ником {username} не существует.");
            List<UserCard> userCards = [.. user.UserCards];
            if (userCards.Count == 0)
            {
                return [];
            }
            List<UserCardDTO> userCardsDTO = [];
            foreach (UserCard userCard in userCards)
            {
                DisplayModel<int> card = new() { Id = userCard.CardId, DisplayName = userCard.Card.Name };
                DisplayModel<int> bank = new() { Id = userCard.Card.BankId, DisplayName = userCard.Card.Bank.Name };
                DisplayModel<int> typeCard = new() { Id = userCard.Card.TypeCardId, DisplayName = userCard.Card.TypeCard.Name };
                UserCardDTO userCardDTO = new()
                {
                    Card = card,
                    Bank = bank,
                    TypeCard = typeCard
                };
                userCardsDTO.Add(userCardDTO);
            }
            return userCardsDTO;
        }

        public List<UserCardDTO> ExecuteAlgorithm(string username)
        {
            User user = _userRepository.GetByUsername(username) ?? throw new UserUsernameNotExists($"Пользователя с ником {username} не существует.");
            if (user.UserCards.Count > 0)
            {
                _userCardRepository.Delete(user.UserCards);
                _userCardRepository.Save();
            }
            if (user.Operations.Count == 0)
            {
                return [];
            }
            List<UserCard> userCards = _algorithmService.ExecuteAlgorithm(user.Operations, user);
            List<UserCardDTO> userCardsDTO = [];
            foreach (UserCard userCard in userCards)
            {
                DisplayModel<int> card = new() { Id = userCard.CardId, DisplayName = userCard.Card.Name };
                DisplayModel<int> bank = new() { Id = userCard.Card.BankId, DisplayName = userCard.Card.Bank.Name };
                DisplayModel<int> typeCard = new() { Id = userCard.Card.TypeCardId, DisplayName = userCard.Card.TypeCard.Name };
                UserCardDTO userCardDTO = new()
                {
                    Card = card,
                    Bank = bank,
                    TypeCard = typeCard
                };
                userCardsDTO.Add(userCardDTO);
            }
            return userCardsDTO;
        }
    }
}
