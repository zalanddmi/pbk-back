using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.Bank;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.Requests;
using PbkService.ViewModels;
using X.PagedList;

namespace PbkService.Services
{
    public class BankService(BankRepository bankRepository)
    {
        private readonly BankRepository _bankRepository = bankRepository;

        public BankDTO GetBankById(int id)
        {
            Bank? bank = _bankRepository.GetBankById(id) ?? throw new BankNotExists($"Банк с id = {id} не найден.");
            BankDTO bankDTO = new(bank.Name, bank.Id);
            return bankDTO;
        }

        public IEnumerable<BankDTO> GetBanks()
        {
            IEnumerable<Bank?> banks = _bankRepository.GetBanks();
            if (banks == null)
            {
                return [];
            }
            List<BankDTO> banksDTO = [];
            foreach (Bank bank in banks)
            {
                BankDTO bankDTO = new(bank.Name, bank.Id);
                banksDTO.Add(bankDTO);
            }
            return banksDTO;
        }

        public PbkPagedList<BankDTO> GetPagedList(GetPagedRequest request) 
        {
            IPagedList<Bank> banks = _bankRepository.GetPagedBanks(request.PageNumber, request.PageSize, request.SearchString);
            List<BankDTO> bankDTOs = [];
            foreach (Bank bank in banks)
            {
                bankDTOs.Add(new BankDTO(bank.Name, bank.Id));
            }
            PbkPagedList<BankDTO> pagedList = new()
            {
                PageNumber = banks.PageNumber,
                PageSize = banks.PageSize,
                PageCount = banks.PageCount,
                TotalCount = banks.TotalItemCount,
                Items = bankDTOs
            };
            return pagedList;
        }

        public int Create(BankDTO bankDTO)
        {
            Bank bank = new()
            {
                Name = bankDTO.Name
            };
            int id = _bankRepository.Create(bank);
            return id;
        }

        public void Update(BankDTO bankDTO)
        {
            Bank? bank = _bankRepository.GetBankById(bankDTO.Id) ?? throw new BankNotExists($"Банк с id = {bankDTO.Id} не найден.");
            bank.Name = bankDTO.Name;
            _bankRepository.Update(bank);
        }

        public void Delete(int id)
        {
            Bank? bank = _bankRepository.GetBankById(id) ?? throw new BankNotExists($"Банк с id = {id} не найден.");
            _bankRepository.Delete(bank);
        }
    }
}
