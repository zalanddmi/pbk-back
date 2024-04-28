using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.Shop;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.Requests;
using PbkService.ViewModels;
using X.PagedList;

namespace PbkService.Services
{
    public class ShopService(ShopRepository shopRepository)
    {
        private readonly ShopRepository _shopRepository = shopRepository;

        public ShopDTO GetById(int id)
        {
            Shop? shop = _shopRepository.GetById(id) ?? throw new ShopNotExists($"Магазин с id = {id} не найден.");
            List<DisplayModel<int>> outlets = [];
            if (shop.Outlets != null)
            {
                foreach (Outlet outlet in shop.Outlets)
                {
                    outlets.Add(new DisplayModel<int> { Id = outlet.Id, DisplayName = outlet.Name });
                }
            }
            ShopDTO shopDTO = new(shop.Name, outlets, shop.Id);
            return shopDTO;
        }

        public PbkPagedList<ShopDTO> GetPagedList(GetPagedRequest request)
        {
            IPagedList<Shop> shops = _shopRepository.GetPagedList(request.PageNumber, request.PageSize, request.SearchString);
            List<ShopDTO> shopsDTO = [];
            foreach (Shop shop in shops)
            {
                List<DisplayModel<int>> outlets = [];
                if (shop.Outlets != null)
                {
                    foreach (Outlet outlet in shop.Outlets)
                    {
                        outlets.Add(new DisplayModel<int> { Id = outlet.Id, DisplayName = outlet.Name });
                    }
                }
                ShopDTO shopDTO = new(shop.Name, outlets, shop.Id);
                shopsDTO.Add(shopDTO);
            }
            PbkPagedList<ShopDTO> pagedList = new()
            {
                PageNumber = shops.PageNumber,
                PageSize = shops.PageSize,
                PageCount = shops.PageCount,
                TotalCount = shops.TotalItemCount,
                Items = shopsDTO
            };
            return pagedList;
        }

        public int Create(ShopDTO shopDTO)
        {
            Shop shop = new()
            {
                Name = shopDTO.Name
            };
            int id = _shopRepository.Create(shop);
            return id;
        }

        public void Update(ShopDTO shopDTO)
        {
            Shop? shop = _shopRepository.GetById(shopDTO.Id) ?? throw new ShopNotExists($"Магазин с id = {shopDTO.Id} не найден.");
            shop.Name = shopDTO.Name;
            _shopRepository.Update(shop);
        }

        public void Delete(int id)
        {
            Shop? shop = _shopRepository.GetById(id) ?? throw new ShopNotExists($"Магазин с id = {id} не найден.");
            _shopRepository.Delete(shop);
        }
    }
}
