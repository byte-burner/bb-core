using ConsoleTables;
using net_iot_core.Shared.ResultHandling;
using net_iot_util.Constants;

namespace net_iot_util.Managers
{
    public static class PromptManager
    {
        public static void PrintComplexTypeList<T>(IEnumerable<T> items)
        {
            var ct = ConsoleTable.From<T>(items);
            ct.Write();
        }

        public static void PrintSimpleTypeList<T>(IEnumerable<T> items, string columnName)
        {
            if (items == null)
            {
                return;
            }

            var ct = new ConsoleTable(columnName);
            items.ToList().ForEach(i => ct.AddRow(i));
            ct.Write();
        }

        public static int HandleServiceResult(ServiceResult result, string successMessage)
        {
            if (result.IsError)
            {
                foreach (var error in result.Errors)
                {
                    Console.Error.WriteLine($"ERR: {error.Code} - {error.Message}");
                }

                return ExitCodes.EXIT_CODE_FAILURE;
            }

            Console.WriteLine(successMessage);

            return ExitCodes.EXIT_CODE_SUCCESS;
        }
    }
}

