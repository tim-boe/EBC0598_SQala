using System.Linq;
using System.Windows;
using System.Xml.Linq;
using HEADacoustics.API.SQala;

namespace CategoryRatingWithReferenceStep
{
    //The factory has to be exported as seen below. If your factory will not show up in SQala make sure that your .dll is placed inside the Extension.API.SQala folder.
    [ExportStepFactory("CategoryRatingReferenceStep")]
    public class StepFactory : IStepFactory, IStepConfigFactory, IStepConfigEditorFactory<StepConfig>, IStepPreviewFactory, IResultConverterFactory
    {
        // Define some general settings regarding the nature of this step
        public FactorySettings Settings
        {
            get
            {
                var settings = new FactorySettings();
                settings.SetName("Category Rating with Reference Step");
                settings.SetCategory("API Documentation");
                // SQala will only allow selection of sounds for a step if UsesSounds is set to true
                settings.SetUsesSounds(true);
                // SQala will automatically ensure that a user has to select at least the minimum amount of sounds specified here
                settings.SetMinimumNumberOfSounds(2);
                return settings;
            }
        }

        // An IStep is a requirement for a step to be usable. It contains the logic of what is supposed to happen during execution
        public IStep CreateStep()
        {
            return new Step();
        }

        // The IStepConfig contains the individual settings of a specific IStep Instance.
        public IStepConfig CreateStepConfig()
        {
            return new StepConfig();
        }

        // The IStepConfigEditor is optional and allows usage of a property page to configure the IStepConfig
        public IStepConfigEditor CreateStepConfigEditor(StepConfig stepConfig, IEditorEnvironment environment)
        {
            return new StepConfigEditor(stepConfig, environment);
        }

        public FrameworkElement GetStepPreview(XElement readonlyStepConfig, IPreviewEnvironment environment)
        {
            var config = new StepConfig(readonlyStepConfig);
            var previewDataContext = new StepPreviewDataContext(environment.AttributeReferences.First(a => a.Id == config.SelectedAttributeId));
            return new StepView() { DataContext = previewDataContext };
        }

        public IResultConverter CreateResultConverter()
        {
            return new StepResultConverter();
        }
    }
}