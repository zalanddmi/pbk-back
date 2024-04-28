using PbkService.Auxiliaries.Exceptions.TypeCard;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.ViewModels;

namespace PbkService.Services
{
    public class TypeCardService(TypeCardRepository typeCardRepository)
    {
        private readonly TypeCardRepository _typeCardRepository = typeCardRepository;

        public TypeCardDTO GetById(int id)
        {
            TypeCard? typeCard = _typeCardRepository.GetById(id) ?? throw new TypeCardNotExists($"Тип карты с id = {id} не найден.");
            TypeCardDTO typeCardDTO = new(typeCard.Name, typeCard.Id);
            return typeCardDTO;
        }

        public IEnumerable<TypeCardDTO> Get()
        {
            IEnumerable<TypeCard?> typeCards = _typeCardRepository.Get();
            if (typeCards == null)
            {
                return [];
            }
            List<TypeCardDTO> typeCardsDTO = [];
            foreach (TypeCard typeCard in typeCards)
            {
                TypeCardDTO typeCardDTO = new(typeCard.Name, typeCard.Id);
                typeCardsDTO.Add(typeCardDTO);
            }
            return typeCardsDTO;
        }

        public int Create(TypeCardDTO typeCardDTO)
        {
            TypeCard typeCard = new()
            {
                Name = typeCardDTO.Name
            };
            int id = _typeCardRepository.Create(typeCard);
            return id;
        }

        public void Update(TypeCardDTO typeCardDTO)
        {
            TypeCard? typeCard = _typeCardRepository.GetById(typeCardDTO.Id) ?? throw new TypeCardNotExists($"Тип карты с id = {typeCardDTO.Id} не найден.");
            typeCard.Name = typeCardDTO.Name;
            _typeCardRepository.Update(typeCard);
        }

        public void Delete(int id)
        {
            TypeCard? typeCard = _typeCardRepository.GetById(id) ?? throw new TypeCardNotExists($"Тип карты с id = {id} не найден.");
            _typeCardRepository.Delete(typeCard);
        }
    }
}
