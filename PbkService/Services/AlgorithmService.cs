using PbkService.Models;
using PbkService.Repositories;

namespace PbkService.Services
{
    public class AlgorithmService(CardRepository cardRepository, UserCardRepository userCardRepository)
    {
        private readonly CardRepository _cardRepository = cardRepository;
        private readonly UserCardRepository _userCardRepository = userCardRepository;

        public List<UserCard> ExecuteAlgorithm(IEnumerable<Operation> operations, User user)
        {
            List<MccPbkCategory> mccsCategories = operations.SelectMany(o => o.Outlet.Mcc.MccPbkCategories).ToList();

            IEnumerable<Card?>? cards = _cardRepository.Get()
                .Where(card => card.Cashbacks.Any(c => mccsCategories.Any(mc => mc.PbkCategoryId == c.PbkCategoryId)));

            if (!cards.Any())
            {
                return [];
            }

#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
            Dictionary<Card, decimal> cardsSum = cards.ToDictionary(card => card, card =>
            {
                decimal sumCashback = 0;
                foreach (var cashback in card.Cashbacks)
                {
                    PbkCategory category = cashback.PbkCategory;
                    IEnumerable<string> mccCodes = mccsCategories.Where(mc => mc.PbkCategoryId == category.Id).Select(mc => mc.MccCode);
                    sumCashback += operations.Where(o => mccCodes.Contains(o.Outlet.Mcc.Code)).Sum(o => o.Sum * cashback.Percent / 100);
                }
                return sumCashback;
            });
#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.

            cardsSum = cardsSum.OrderByDescending(cs => cs.Value).ToDictionary(x => x.Key, x => x.Value);

            List<UserCard> userCards = cardsSum.Where(cs => cs.Value >= operations.Sum(o => o.Sum) / 100)
                .Select(cs => new UserCard { Card = cs.Key, User = user }).ToList();

            if (userCards.Count > 10)
            {
                userCards = userCards.Take(10).ToList();
            }

            _userCardRepository.Create(userCards);
            _userCardRepository.Save();

            return userCards;
        }
    }
}
