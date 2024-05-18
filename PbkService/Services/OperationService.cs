using PbkService.Auxiliaries.Exceptions.Outlet;
using PbkService.Auxiliaries.Exceptions.Operation;
using PbkService.Auxiliaries.Exceptions.User;
using PbkService.Auxiliaries;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.ViewModels;
using X.PagedList;

namespace PbkService.Services
{
    public class OperationService(OperationRepository operationRepository, UserRepository userRepository, OutletRepository outletRepository)
    {
        private readonly OperationRepository _operationRepository = operationRepository;
        private readonly UserRepository _userRepository = userRepository;
        private readonly OutletRepository _outletRepository = outletRepository;

        public OperationDTO GetById(int id)
        {
            Operation operation = _operationRepository.GetById(id) ?? throw new OperationNotExists($"Операция с id = {id} не найдена.");
            DisplayModel<int> outlet = new() { Id = operation.OutletId, DisplayName = operation.Outlet.Name };
            OperationDTO operationDTO = new() { Id = operation.Id, Outlet = outlet, Sum = operation.Sum };
            return operationDTO;
        }

        public List<OperationDTO> GetByUser(string username)
        {
            User user = _userRepository.GetByUsername(username) ?? throw new UserUsernameNotExists($"Пользователя с ником {username} не существует.");
            List<Operation> operations = [.. user.Operations];
            List<OperationDTO> operationsDTO = [];
            foreach (Operation operation in operations)
            {
                DisplayModel<int> outlet = new() { Id = operation.OutletId, DisplayName = operation.Outlet.Name };
                OperationDTO operationDTO = new() { Id = operation.Id, Outlet = outlet, Sum = operation.Sum };
                operationsDTO.Add(operationDTO);
            }
            return operationsDTO;
        }

        public int Create(OperationDTO operationDTO, string username)
        {
            User user = _userRepository.GetByUsername(username) ?? throw new UserUsernameNotExists($"Пользователя с ником {username} не существует.");
            Outlet outlet = _outletRepository.GetById(operationDTO.Outlet.Id) ?? throw new OutletNotExists($"Торговая точка с id = {operationDTO.Outlet.Id} не найдена.");
            List<Operation> userOperations = [.. user.Operations];
            if (userOperations.Any(o => o.OutletId == outlet.Id))
            {
                throw new OperationOutletExists($"Операция с торговой точкой c id = {outlet.Id} существует.");
            }
            Operation operation = new()
            {
                User = user,
                Outlet = outlet,
                Sum = operationDTO.Sum
            };
            _operationRepository.Create(operation);
            _operationRepository.Save();
            return operation.Id;
        }

        public List<int> Create(List<OperationDTO> operationsDTO, string username)
        {
            User user = _userRepository.GetByUsername(username) ?? throw new UserUsernameNotExists($"Пользователя с ником {username} не существует.");
            List<Operation> operations = [];
            List<Operation> userOperations = [.. user.Operations];
            foreach (OperationDTO operationDTO in operationsDTO)
            {
                Outlet outlet = _outletRepository.GetById(operationDTO.Outlet.Id) ?? throw new OutletNotExists($"Торговая точка с id = {operationDTO.Outlet.Id} не найдена.");
                if (userOperations.Any(o => o.OutletId == outlet.Id))
                {
                    throw new OperationOutletExists($"Операция с торговой точкой c id = {outlet.Id} существует.");
                }
                Operation operation = new()
                {
                    User = user,
                    Outlet = outlet,
                    Sum = operationDTO.Sum
                };
                operations.Add(operation);
            }
            _operationRepository.Create(operations);
            _operationRepository.Save();
            List<int> ids = operations.Select(o => o.Id).ToList();
            return ids;
        }

        public void Update(OperationDTO operationDTO, string username)
        {
            Operation operation = _operationRepository.GetById(operationDTO.Id) ?? throw new OperationNotExists($"Операция с id = {operationDTO.Id} не найдена.");
            User user = _userRepository.GetByUsername(username) ?? throw new UserUsernameNotExists($"Пользователя с ником {username} не существует.");
            Outlet outlet = _outletRepository.GetById(operationDTO.Outlet.Id) ?? throw new OutletNotExists($"Торговая точка с id = {operationDTO.Outlet.Id} не найдена.");
            List<Operation> userOperations = [.. user.Operations];
            if (userOperations.Any(o => o.OutletId == outlet.Id && o.Id != operation.Id))
            {
                throw new OperationOutletExists($"Операция с торговой точкой c id = {outlet.Id} существует.");
            }
            operation.Outlet = outlet;
            operation.Sum = operationDTO.Sum;
            _operationRepository.Update(operation);
            _operationRepository.Save();
        }

        public void Delete(int id)
        {
            Operation operation = _operationRepository.GetById(id) ?? throw new OperationNotExists($"Операция с id = {id} не найдена.");
            _operationRepository.Delete(operation);
            _operationRepository.Save();
        }

        public void Delete(int[] ids)
        {
            List<Operation> operations = [];
            foreach (int id in ids)
            {
                Operation operation = _operationRepository.GetById(id) ?? throw new OperationNotExists($"Операция с id = {id} не найдена.");
                operations.Add(operation);
            }
            _operationRepository.Delete(operations);
            _operationRepository.Save();
        }
    }
}
