using Aspose.Cells;
using PbkService.Models;
using PbkService.Repositories;

namespace PbkService.Services
{
    public class MccService(MccRepository repository)
    {
        private readonly MccRepository _repository = repository;

        public List<Mcc> GetAll()
        {
            return _repository.GetMccs();
        }

        public void LoadMccDataFromFileIfExists(string directoryPath)
        {
            string? filePath = Directory.GetFiles(directoryPath, "*.xlsx").FirstOrDefault();
            if (filePath == null)
            {
                Console.WriteLine("В директории отсутствует файл с MCC");
                return;
            }
            LoadMccDataFromFile(filePath);
        }

        public void LoadMccDataFromFile(string filePath)
        {
            Workbook workbook = new(filePath);
            Worksheet worksheet = workbook.Worksheets[0];
            int rows = worksheet.Cells.MaxDataRow;
            for (int i = 1; i < rows + 1; i++)
            {
                string? code = worksheet.Cells[i, 0].Value?.ToString();
                string? name = worksheet.Cells[i, 1].Value?.ToString();
                string? description = worksheet.Cells[i, 2].Value?.ToString();

                if (code == null || name == null)
                {
                    Console.WriteLine("Данные MCC некорректны при загрузке");
                    return;
                }
                _repository.Create(new Mcc
                {
                    Code = code,
                    Name = name,
                    Description = description
                });
            }
            _repository.Save();
            File.Delete(filePath);
        }
    }
}
