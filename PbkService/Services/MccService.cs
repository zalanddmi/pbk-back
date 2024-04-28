using Aspose.Cells;
using PbkService.Auxiliaries;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.Requests;
using PbkService.ViewModels;
using X.PagedList;

namespace PbkService.Services
{
    public class MccService(MccRepository repository)
    {
        private readonly MccRepository _repository = repository;

        public PbkPagedList<MccDTO> GetPagedList(GetPagedRequest request)
        {
            IPagedList<Mcc> mccs = _repository.GetPagedList(request.PageNumber, request.PageSize, request.SearchString);
            List<MccDTO> mccsDTO = [];
            foreach (Mcc mcc in mccs)
            {
                mccsDTO.Add(new MccDTO(mcc.Code, mcc.Name, mcc.Description));
            }
            PbkPagedList<MccDTO> pagedList = new()
            {
                PageNumber = mccs.PageNumber,
                PageSize = mccs.PageSize,
                PageCount = mccs.PageCount,
                TotalCount = mccs.TotalItemCount,
                Items = mccsDTO
            };
            return pagedList;
        }

        public void LoadMccDataFromFile(IFormFile formFile)
        {
            if (!formFile.FileName.EndsWith(".xlsx"))
            {
                throw new InvalidDataException("Формат файла не соответствует формату Excel");
            }
            using Stream stream = formFile.OpenReadStream();
            Workbook workbook = new(stream);
            Worksheet worksheet = workbook.Worksheets[0];
            int rows = worksheet.Cells.MaxDataRow;
            for (int i = 1; i <= rows; i++)
            {
                string? code = worksheet.Cells[i, 0].Value?.ToString();
                string? name = worksheet.Cells[i, 1].Value?.ToString();
                string? description = worksheet.Cells[i, 2].Value?.ToString();

                if (code == null || name == null)
                {
                    throw new InvalidDataException("Данные MCC некорректны");
                }
                Mcc? mcc = _repository.GetMccByCode(code);
                if (mcc == null)
                {
                    _repository.Create(new Mcc
                    {
                        Code = code,
                        Name = name,
                        Description = description
                    });
                }
                else
                {
                    mcc.Name = name;
                    mcc.Description = description;
                    _repository.Update(mcc);
                }
            }
            _repository.Save();
        }
    }
}
