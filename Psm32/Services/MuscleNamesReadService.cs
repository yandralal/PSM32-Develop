using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Psm32.Models;
using Serilog;

namespace Psm32.Services;

public interface IMuscleNamesReadService
{
    IEnumerable<MuscleName> ReadMuscleNames();
}
public class MuscleNamesReadService : IMuscleNamesReadService
{
    private static readonly string MUSCLE_NAMES_FILE_PATH = "Resources/NMES_Muscle_Names.csv";
    private readonly ILogger logger;

    public MuscleNamesReadService(ILogger logger)
    {
        this.logger = logger;
    }

    public IEnumerable<MuscleName> ReadMuscleNames()
    {
        try
        {
            using (var reader = new StreamReader(MUSCLE_NAMES_FILE_PATH))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<MuscleName>().ToList() ?? new List<MuscleName>();
            }
        }
        catch (Exception ex)
        {
            logger.Error($"Failed to read Muscle Names file from `{MUSCLE_NAMES_FILE_PATH}`, Error {ex.Message}");
            throw new Exception("Failed to read Muscle Names");
        }
    }
}
