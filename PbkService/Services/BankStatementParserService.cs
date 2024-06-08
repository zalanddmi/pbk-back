using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using PbkService.Auxiliaries;
using PbkService.ViewModels;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PbkService.Services
{
    public class BankStatementParserService(OutletService outletService, OperationService operationService)
    {
        public List<int> ParseStatement(string? username, IFormFile file)
        {
            StringBuilder text = new();
            using Stream stream = file.OpenReadStream();
            using PdfReader reader = new(stream);
            using PdfDocument document = new(reader);
            for (int page = 1; page <= document.GetNumberOfPages(); page++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string pageText = PdfTextExtractor.GetTextFromPage(document.GetPage(page), strategy);
                text.Append(pageText);
            }

            var operations = GetOperations(text.ToString());
            var dtos = new List<OperationDTO>(operations.Count);
            foreach (var operation in operations)
            {
                var outlet = outletService.GetByName(operation.Key);
                if (outlet != null)
                {
                    dtos.Add(new OperationDTO
                    {
                        Outlet = new DisplayModel<int>
                        {
                            Id = outlet.Id
                        },
                        Sum = operation.Value
                    });
                }
            }

            return operationService.Create(dtos, username);
        }


        private Dictionary<string, decimal> GetOperations(string text)
        {
            var operations = new Dictionary<string, decimal>();

            MatchCollection matches = Regex.Matches(text, @"^(.*) (.*) (.*) (.*) (.*) (.*) Оплата в (.*)$", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                if (!operations.ContainsKey(match.Groups[7].ToString()))
                {
                    operations.Add(match.Groups[7].ToString(), decimal.Parse(match.Groups[5].ToString(), CultureInfo.InvariantCulture));
                }
                else
                {
                    operations[match.Groups[7].ToString()] += decimal.Parse(match.Groups[5].ToString(), CultureInfo.InvariantCulture);
                }
            }

            return operations;
        }
    }
}
