using PbkService.Auxiliaries.Exceptions.Outlet;
using PbkService.Auxiliaries;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.Requests;
using PbkService.ViewModels;
using X.PagedList;
using PbkService.Auxiliaries.Exceptions.Shop;
using PbkService.Auxiliaries.Exceptions.Mcc;

namespace PbkService.Services
{
    public class OutletService(OutletRepository outletRepository, ShopRepository shopRepository, MccRepository mccRepository)
    {
        private readonly OutletRepository _outletRepository = outletRepository;
        private readonly ShopRepository _shopRepository = shopRepository;
        private readonly MccRepository _mccRepository = mccRepository;

        public OutletDTO GetById(int id)
        {
            Outlet? outlet = _outletRepository.GetById(id) ?? throw new OutletNotExists($"Торговая точка с id = {id} не найдена.");
            DisplayModel<int> shop = new() { Id = outlet.ShopId, DisplayName = outlet.Shop.Name };
            DisplayModel<string> mcc = new () { Id = outlet.MccCode, DisplayName = outlet.Mcc.Name };
            OutletDTO outletDTO = new(outlet.Name, shop, mcc, outlet.Id);
            return outletDTO;
        }

        public PbkPagedList<OutletDTO> GetPagedList(GetPagedRequest request)
        {
            IPagedList<Outlet> outlets = _outletRepository.GetPagedList(request.PageNumber, request.PageSize, request.SearchString);
            List<OutletDTO> outletsDTO = [];
            foreach (Outlet outlet in outlets)
            {
                DisplayModel<int> shop = new() { Id = outlet.ShopId, DisplayName = outlet.Shop.Name };
                DisplayModel<string> mcc = new() { Id = outlet.MccCode, DisplayName = outlet.Mcc.Name };
                outletsDTO.Add(new OutletDTO(outlet.Name, shop, mcc, outlet.Id));
            }
            PbkPagedList<OutletDTO> pagedList = new()
            {
                PageNumber = outlets.PageNumber,
                PageSize = outlets.PageSize,
                PageCount = outlets.PageCount,
                TotalCount = outlets.TotalItemCount,
                Items = outletsDTO
            };
            return pagedList;
        }

        public int Create(OutletDTO outletDTO)
        {
            Shop shop = _shopRepository.GetById(outletDTO.Shop.Id) ?? throw new ShopNotExists($"Магазин с id = {outletDTO.Shop.Id} не найден.");
            Mcc mcc = _mccRepository.GetMccByCode(outletDTO.Mcc.Id) ?? throw new MccNotExists($"MCC с кодом = {outletDTO.Mcc.Id} не найден.");
            Outlet outlet = new()
            {
                Name = outletDTO.Name,
                ShopId = shop.Id,
                Shop = shop,
                MccCode = mcc.Code,
                Mcc = mcc
            };
            int id = _outletRepository.Create(outlet);
            return id;
        }

        public void Update(OutletDTO outletDTO)
        {
            Outlet? outlet = _outletRepository.GetById(outletDTO.Id) ?? throw new OutletNotExists($"Торговая точка с id = {outletDTO.Id} не найдена.");
            Shop shop = _shopRepository.GetById(outletDTO.Shop.Id) ?? throw new ShopNotExists($"Магазин с id = {outletDTO.Shop.Id} не найден.");
            Mcc mcc = _mccRepository.GetMccByCode(outletDTO.Mcc.Id) ?? throw new MccNotExists($"MCC с кодом = {outletDTO.Mcc.Id} не найден.");
            outlet.Name = outletDTO.Name;
            outlet.ShopId = shop.Id;
            outlet.Shop = shop;
            outlet.MccCode = mcc.Code;
            outlet.Mcc = mcc;
            _outletRepository.Update(outlet);
        }

        public void Delete(int id)
        {
            Outlet? outlet = _outletRepository.GetById(id) ?? throw new OutletNotExists($"Торговая точка с id = {id} не найдена.");
            _outletRepository.Delete(outlet);
        }
    }
}
