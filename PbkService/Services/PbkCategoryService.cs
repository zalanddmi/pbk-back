using Npgsql.TypeMapping;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.Mcc;
using PbkService.Auxiliaries.Exceptions.PbkCategory;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.Requests;
using PbkService.ViewModels;
using X.PagedList;

namespace PbkService.Services
{
    public class PbkCategoryService(PbkCategoryRepository pbkCategoryRepository, MccRepository mccRepository, MccPbkCategoryRepository mccPbkCategoryRepository)
    {
        private readonly PbkCategoryRepository _pbkCategoryRepository = pbkCategoryRepository;
        private readonly MccRepository _mccRepository = mccRepository;
        private readonly MccPbkCategoryRepository _mccPbkCategoryRepository = mccPbkCategoryRepository;

        public PbkCategoryDTO GetById(int id)
        {
            PbkCategory? category = _pbkCategoryRepository.GetById(id) ?? throw new PbkCategoryNotExists($"Категория с id = {id} не найдена.");
            List<DisplayModel<string>> mccs = [];
            if (category.MccPbkCategories != null)
            {
                foreach (MccPbkCategory mccPbkCategory in category.MccPbkCategories)
                {
                    mccs.Add(new DisplayModel<string> { Id = mccPbkCategory.MccCode, DisplayName = mccPbkCategory.Mcc.Name });
                }
            }
            PbkCategoryDTO categoryDTO = new(category.Name, mccs, category.Id);
            return categoryDTO;
        }

        public PbkPagedList<PbkCategoryDTO> GetPagedList(GetPagedRequest request)
        {
            IPagedList<PbkCategory> categories = _pbkCategoryRepository.GetPagedList(request.PageNumber, request.PageSize, request.SearchString);
            List<PbkCategoryDTO> categoriesDTO = [];
            foreach (PbkCategory category in categories)
            {
                List<DisplayModel<string>> mccs = [];
                if (category.MccPbkCategories != null)
                {
                    foreach (MccPbkCategory mccPbkCategory in category.MccPbkCategories)
                    {
                        mccs.Add(new DisplayModel<string> { Id = mccPbkCategory.MccCode, DisplayName = mccPbkCategory.Mcc.Name });
                    }
                }
                PbkCategoryDTO categoryDTO = new(category.Name, mccs, category.Id);
                categoriesDTO.Add(categoryDTO);
            }
            PbkPagedList<PbkCategoryDTO> pagedList = new()
            {
                PageNumber = categories.PageNumber,
                PageSize = categories.PageSize,
                PageCount = categories.PageCount,
                TotalCount = categories.TotalItemCount,
                Items = categoriesDTO
            };
            return pagedList;
        }

        public int Create(PbkCategoryDTO categoryDTO)
        {
            List<Mcc> mccs = [];
            if (categoryDTO.Mccs.Count != 0)
            {
                foreach (DisplayModel<string> mccDisplay in categoryDTO.Mccs)
                {
                    Mcc mcc = _mccRepository.GetMccByCode(mccDisplay.Id) ?? throw new MccNotExists($"MCC с кодом = {mccDisplay.Id} не найден.");
                    mccs.Add(mcc);
                }
            }
            PbkCategory category = new()
            {
                Name = categoryDTO.Name
            };
            int id = _pbkCategoryRepository.Create(category);
            if (mccs.Count != 0)
            {
                foreach (Mcc mcc in mccs)
                {
                    MccPbkCategory mc = new()
                    {
                        MccCode = mcc.Code,
                        Mcc = mcc,
                        PbkCategoryId = id,
                        PbkCategory = category
                    };
                    _mccPbkCategoryRepository.Create(mc);
                }
            }
            return id;
        }

        public void Update(PbkCategoryDTO categoryDTO)
        {
            PbkCategory? category = _pbkCategoryRepository.GetById(categoryDTO.Id) ?? throw new PbkCategoryNotExists($"Категория с id = {categoryDTO.Id} не найдена.");
            category.Name = categoryDTO.Name;
            IEnumerable<MccPbkCategory?> mcs = _mccPbkCategoryRepository.GetByCategoryId(categoryDTO.Id);
            List<MccPbkCategory> mcsCreate = [];
            List<string> mcsCodeDelete = [];
            if (mcs.Any())
            {
                foreach(DisplayModel<string> mcc in categoryDTO.Mccs)
                {
                    if (!mcs.Any(m => m.MccCode == mcc.Id))
                    {
                        mcsCreate.Add(new MccPbkCategory() { MccCode = mcc.Id, PbkCategoryId = categoryDTO.Id });
                    }
                }
                foreach(MccPbkCategory mc in mcs)
                {
                    if (!categoryDTO.Mccs.Any(m => m.Id == mc.MccCode))
                    {
                        mcsCodeDelete.Add(mc.MccCode);
                    }
                }
            }
            else
            {
                foreach (DisplayModel<string> mcc in categoryDTO.Mccs)
                {
                    mcsCreate.Add(new MccPbkCategory() { MccCode = mcc.Id, PbkCategoryId = categoryDTO.Id });
                }
            }
            _pbkCategoryRepository.Update(category);
            foreach (MccPbkCategory mc in mcsCreate)
            {
                _mccPbkCategoryRepository.Create(mc);
            }
            foreach (string code in mcsCodeDelete)
            {
                _mccPbkCategoryRepository.Delete(categoryDTO.Id, code);
            }
        }

        public void Delete(int id)
        {
            PbkCategory? category = _pbkCategoryRepository.GetById(id) ?? throw new PbkCategoryNotExists($"Категория с id = {id} не найдена.");
            IEnumerable<MccPbkCategory?> mcs = _mccPbkCategoryRepository.GetByCategoryId(category.Id);
            _mccPbkCategoryRepository.Delete(mcs);
            _pbkCategoryRepository.Delete(category);
        }
    }
}
