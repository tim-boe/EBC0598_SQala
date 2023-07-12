using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using HEADacoustics.API.SQala;

namespace CategoryRatingWithReferenceStep
{
    public class StepResultConverter : IResultConverter
    {
        public DataTable ConvertToDataTable(IEnumerable<IStepParticipation> rawResults, XElement readonlyStepConfig, IResultExportEnvironment environment)
        {
            var config = new StepConfig(readonlyStepConfig);
            var referenceSoundDisplayName = environment.SoundReferences.First(s => s.Id == config.ReferenceSoundId)?.DisplayName;
            var results = rawResults.ToList();

            int numberOfSoundsWithoutReference = environment.SoundReferences.Count() - 1;
            string[] columnLabels = new string[numberOfSoundsWithoutReference];
            string[] rowLabels = new string[results.Count];
            string[,] entries = new string[rowLabels.Length, columnLabels.Length];

            for (int i = 0; i < results.Count; i++)
            {
                var ratings = results[i].Result.Data.Elements().ToList();
                for (int j = 0; j < ratings.Count; j++)
                {
                    if (j == 0)
                        rowLabels[i] = results[i].ParticipantDisplayName;

                    var rating = new Rating(ratings[j]);
                    entries[i, j] = rating.Result;
                    if (i == 0)
                        columnLabels[j] = environment.SoundReferences.First(s => s.Id == rating.SoundId)?.DisplayName;
                }
            }

            return new DataTable("Example Results", rowLabels, columnLabels, entries,
                $"Reference Sound was: {referenceSoundDisplayName}");
        }
    }
}