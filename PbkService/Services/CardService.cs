using PbkService.Auxiliaries;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.Requests;
using PbkService.ViewModels;
using X.PagedList;
using PbkService.ViewModels.Cards;
using PbkService.Auxiliaries.Exceptions.Card;
using PbkService.Auxiliaries.Exceptions.Bank;
using PbkService.Auxiliaries.Exceptions.TypeCard;
using PbkService.Auxiliaries.Exceptions.PbkCategory;
using PbkService.Auxiliaries.Exceptions.Cashback;

namespace PbkService.Services
{
    public class CardService(CardRepository cardRepository,
        BankRepository bankRepository, 
        TypeCardRepository typeCardRepository, 
        CashbackRepository cashbackRepository, 
        PbkCategoryRepository categoryRepository)
    {
        private readonly CardRepository _cardRepository = cardRepository;
        private readonly BankRepository _bankRepository = bankRepository;
        private readonly TypeCardRepository _typeCardRepository = typeCardRepository;
        private readonly CashbackRepository _cashbackRepository = cashbackRepository;
        private readonly PbkCategoryRepository _categoryRepository = categoryRepository;

        public CardDTO GetById(int id)
        {
            Card? card = _cardRepository.GetById(id) ?? throw new CardNotExists($"Карта с id = {id} не найдена.");
            List<CardCashbackDTO> cashbacks = [];
            if (card.Cashbacks != null)
            {
                foreach (Cashback cashback in card.Cashbacks)
                {
                    CardCashbackDTO cashbackDTO = new()
                    {
                        Id = cashback.Id,
                        Category = new DisplayModel<int>() { Id = cashback.PbkCategoryId, DisplayName = cashback.PbkCategory.Name },
                        Percent = cashback.Percent
                    };
                    cashbacks.Add(cashbackDTO);
                }
            }
            DisplayModel<int> bank = new() { Id = card.BankId, DisplayName = card.Bank.Name };
            DisplayModel<int> typeCard = new() { Id = card.TypeCardId, DisplayName = card.TypeCard.Name };
            CardDTO cardDTO = new()
            {
                Id = card.Id,
                Name = card.Name,
                Bank = bank,
                TypeCard = typeCard,
                Cashbacks = cashbacks
            };
            return cardDTO;
        }

        public PbkPagedList<CardDTO> GetPagedList(GetPagedRequest request)
        {
            IPagedList<Card> cards = _cardRepository.GetPaged(request.PageNumber, request.PageSize, request.SearchString);
            List<CardDTO> cardsDTO = [];
            foreach (Card card in cards)
            {
                List<CardCashbackDTO> cashbacks = [];
                if (card.Cashbacks != null)
                {
                    foreach (Cashback cashback in card.Cashbacks)
                    {
                        CardCashbackDTO cashbackDTO = new()
                        {
                            Id = cashback.Id,
                            Category = new DisplayModel<int>() { Id = cashback.PbkCategoryId, DisplayName = cashback.PbkCategory.Name },
                            Percent = cashback.Percent
                        };
                        cashbacks.Add(cashbackDTO);
                    }
                }
                DisplayModel<int> bank = new() { Id = card.BankId, DisplayName = card.Bank.Name };
                DisplayModel<int> typeCard = new() { Id = card.TypeCardId, DisplayName = card.TypeCard.Name };
                CardDTO cardDTO = new()
                {
                    Id = card.Id,
                    Name = card.Name,
                    Bank = bank,
                    TypeCard = typeCard,
                    Cashbacks = cashbacks
                };
                cardsDTO.Add(cardDTO);
            }
            PbkPagedList<CardDTO> pagedList = new()
            {
                PageNumber = cards.PageNumber,
                PageSize = cards.PageSize,
                PageCount = cards.PageCount,
                TotalCount = cards.TotalItemCount,
                Items = cardsDTO
            };
            return pagedList;
        }

        public int Create(CardDTO cardDTO)
        {
            Bank bank = _bankRepository.GetBankById(cardDTO.Bank.Id) ?? throw new BankNotExists($"Банк с id = {cardDTO.Bank.Id} не найден.");
            TypeCard typeCard = _typeCardRepository.GetTypeCardById(cardDTO.TypeCard.Id) ?? throw new TypeCardNotExists($"Тип карты с id = {cardDTO.TypeCard.Id} не найден.");
            List<PbkCategory> categories = [];
            if (cardDTO.Cashbacks.Count != 0)
            {
                foreach (CardCashbackDTO cardCashbackDTO in cardDTO.Cashbacks)
                {
                    PbkCategory category = _categoryRepository.GetPbkCategoryById(cardCashbackDTO.Category.Id) ?? throw new PbkCategoryNotExists($"Категория с id = {cardCashbackDTO.Category.Id} не найдена.");
                    categories.Add(category);
                }
            }
            Card card = new()
            {
                Name = cardDTO.Name,
                BankId = cardDTO.Bank.Id,
                Bank = bank,
                TypeCardId = cardDTO.TypeCard.Id,
                TypeCard = typeCard
            };
            int id = _cardRepository.Create(card);
            if (categories.Count != 0)
            {
                for (int i = 0; i < categories.Count; i++)
                {
                    Cashback cashback = new()
                    {
                        CardId = id,
                        Card = card,
                        PbkCategoryId = categories[i].Id,
                        PbkCategory = categories[i],
                        Percent = cardDTO.Cashbacks[i].Percent
                    };
                    _cashbackRepository.Create(cashback);
                }
            }
            return id;
        }

        public void Update(CardDTO cardDTO)
        {
            Card? card = _cardRepository.GetById(cardDTO.Id) ?? throw new CardNotExists($"Карта с id = {cardDTO.Id} не найдена.");
            Bank? bank = _bankRepository.GetBankById(cardDTO.Bank.Id) ?? throw new BankNotExists($"Банк с id = {cardDTO.Bank.Id} не найден.");
            TypeCard typeCard = _typeCardRepository.GetTypeCardById(cardDTO.TypeCard.Id) ?? throw new TypeCardNotExists($"Тип карты с id = {cardDTO.TypeCard.Id} не найден.");
            List<Cashback> cashbacks = [.. card.Cashbacks];
            List<Cashback> cashbacksCreate = [];
            List<Cashback> cashbacksUpdate = [];
            List<Cashback> cashbacksDelete = [];
            if (cashbacks.Count != 0)
            {
                foreach (CardCashbackDTO cc in cardDTO.Cashbacks)
                {
                    if (cc.Id == 0)
                    {
                        PbkCategory category = _categoryRepository.GetPbkCategoryById(cc.Category.Id) ?? throw new PbkCategoryNotExists($"Категория с id = {cc.Category.Id} не найдена.");
                        cashbacksCreate.Add(new Cashback() { CardId = cardDTO.Id, Card = card, PbkCategoryId = category.Id, PbkCategory = category, Percent = cc.Percent });
                    }
                    else if (cashbacks.Any(c => c.Id == cc.Id))
                    {
                        PbkCategory category = _categoryRepository.GetPbkCategoryById(cc.Category.Id) ?? throw new PbkCategoryNotExists($"Категория с id = {cc.Category.Id} не найдена.");
                        Cashback existingCashback = cashbacks.FirstOrDefault(c => c.Id == cc.Id);
                        if (existingCashback != null)
                        {
                            existingCashback.PbkCategoryId = category.Id;
                            existingCashback.PbkCategory = category;
                            existingCashback.Percent = cc.Percent;
                            cashbacksUpdate.Add(existingCashback);
                        }
                    }
                }

                foreach (Cashback c in cashbacks)
                {
                    if (!cardDTO.Cashbacks.Any(cc => cc.Id == c.Id))
                    {
                        cashbacksDelete.Add(c);
                    }
                }
            }
            else
            {
                foreach (CardCashbackDTO cc in cardDTO.Cashbacks)
                {
                    PbkCategory category = _categoryRepository.GetPbkCategoryById(cc.Category.Id) ?? throw new PbkCategoryNotExists($"Категория с id = {cc.Category.Id} не найдена.");
                    cashbacksCreate.Add(new Cashback() { CardId = cardDTO.Id, Card = card, PbkCategoryId = category.Id, PbkCategory = category, Percent = cc.Percent });
                }
            }
            card.Name = cardDTO.Name;
            card.BankId = cardDTO.Bank.Id;
            card.Bank = bank;
            card.TypeCardId = cardDTO.TypeCard.Id;
            card.TypeCard = typeCard;
            _cardRepository.Update(card);
            foreach (Cashback c in cashbacksCreate)
            {
                _cashbackRepository.Create(c);
            }
            foreach (Cashback c in cashbacksUpdate)
            {
                _cashbackRepository.Update(c);
            }
            foreach (Cashback c in cashbacksDelete)
            {
                _cashbackRepository.Delete(c);
            }
        }

        public void Delete(int id)
        {
            Card? card = _cardRepository.GetById(id) ?? throw new CardNotExists($"Карта с id = {id} не найдена.");
            List<Cashback> cashbacks = [.. card.Cashbacks];
            if (cashbacks.Count != 0)
            {
                _cashbackRepository.Delete(cashbacks);
            }
            _cardRepository.Delete(card);
        }
    }
}
