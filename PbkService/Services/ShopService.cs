using PbkService.Auxiliaries.Exceptions.Shop;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.ViewModels;

namespace PbkService.Services
{
    public class ShopService(ShopRepository shopRepository)
    {
        private readonly ShopRepository _shopRepository = shopRepository;

        public ShopDTO GetShopById(int id)
        {
            Shop? shop = _shopRepository.GetShopById(id) ?? throw new ShopNotExists($"Магазин с id = {id} не найден.");
            ShopDTO shopDTO = new(shop.Name, shop.Id);
            return shopDTO;
        }

        public IEnumerable<ShopDTO> GetShops()
        {
            IEnumerable<Shop?> shops = _shopRepository.GetShops();
            if (shops == null)
            {
                return [];
            }
            List<ShopDTO> shopsDTO = [];
            foreach (Shop shop in shops)
            {
                ShopDTO bankDTO = new(shop.Name, shop.Id);
                shopsDTO.Add(bankDTO);
            }
            return shopsDTO;
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
            Shop? shop = _shopRepository.GetShopById(shopDTO.Id) ?? throw new ShopNotExists($"Магазин с id = {shopDTO.Id} не найден.");
            shop.Name = shopDTO.Name;
            _shopRepository.Update(shop);
        }

        public void Delete(int id)
        {
            Shop? shop = _shopRepository.GetShopById(id) ?? throw new ShopNotExists($"Магазин с id = {id} не найден.");
            _shopRepository.Delete(shop);
        }
    }
}
